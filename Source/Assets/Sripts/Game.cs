using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace War
{
    public class Game : MonoBehaviour
    {
        //Current round number used with max rounds
        int RoundNumber = 0;

        //the current deck
        Deck MyDeck = null;

        //the current table
        Table MyTable = null;

        //the banner used to show messages 
        Banner MyBanner = null;

        //the visual for showing the current/max rounds
        RoundCounter MyRoundCounter = null;

        //all the players
        List<Player> Players = new List<Player>();

        void Start()
        {
            Init();
        }
        
        void Init()
        {
            //make sure we have a deck
            MyDeck = Deck.GetInstance();
            if(MyDeck == null)
            {
                Debug.LogError("Unable to get the deck!");
                return;
            }

            //make sure we have a table
            MyTable = Table.GetInstance();
            if(MyDeck == null)
            {
                Debug.LogError("Unable to get the table!");
                return;
            }

            //get our banner for messages
            MyBanner = GameObject.FindObjectOfType<Banner>();
            if(MyBanner == null)
            {
                Debug.LogError("Unable to find banner");
            }

            MyRoundCounter = GameObject.FindObjectOfType<RoundCounter>();
            if(MyRoundCounter == null)
            {
                Debug.LogError("Unable to find round counter");
            }

            MyTable.Init();

            //set up the deck
            MyDeck.CreateCards();
            MyDeck.ShuffleCards();
        }

        void DealOutCards()
        {
            //split the deck based on the num players
            int numCards = MyDeck.NumCards / Settings.NumPlayers;

            //give each player there chunck of cards
            for (int i = 0; i < Settings.NumPlayers; i++)
            {
                var player = transform.Find("Player" + i);
                if(player == null)
                    continue;

                var playerComp = player.GetComponent<Player>();
                if(playerComp)
                {
                    playerComp.Init();
                    playerComp.AddCards(MyDeck.GetCards(numCards));
                    Players.Add(playerComp);
                    Debug.Log("Adding player " + player.name);
                }
            }
        }

        //button event to start the game
        public void Battle(GameObject self)
        {
            //the button passes self in so you can just turn it off that way
            self.SetActive(false);

            //deal out the cards to the players
            DealOutCards();

            //on battle evaluate the round
            EvaluateRound();
        }

        //evaluates the round to see if we have a win/lose condition
        void EvaluateRound()
        {
            if(Players[0].Cards.Count < 1 || Players[1].Cards.Count < 1)
            {
                ShowGameResults();
                return;
            }

            if(RoundNumber >= Settings.MaxNumRounds)
            {
                ShowGameResults();
                return;
            }

            //update and check the round number
            ++RoundNumber;
            
            //update the round number
            MyRoundCounter.UpdateRound(RoundNumber);

            //the game isnt over keep going
            StartCoroutine(WarRoutine());
        }

        //show the game results of who won
        void ShowGameResults()
        {
            //this part would have to change based on Num Players...but so would a bunch of logic
            // check who won
            if(Players[0].Cards.Count == 0)
            {
                //make sure who wins gets the remaining cards from the table
                Players[1].AddCards(MyTable.GetCards());  
                MyBanner.ShowBanner(string.Format(Settings.PlayerWins, 2));
            }
            else if(Players[1].Cards.Count == 0)
            {
                //make sure who wins gets the remaining cards from the table
                Players[0].AddCards(MyTable.GetCards());  
                MyBanner.ShowBanner(string.Format(Settings.PlayerWins, 1));
            }
            else if(RoundNumber >= Settings.MaxNumRounds)
            {
                //max rounds reached, who ever has most cards win
                if(Players[0].Cards.Count > Players[1].Cards.Count)
                {
                    MyBanner.ShowBanner(string.Format(Settings.MaxRoundsWin, 1));
                }
                else if(Players[0].Cards.Count < Players[1].Cards.Count)
                {
                    MyBanner.ShowBanner(string.Format(Settings.MaxRoundsWin, 1));
                }
                else
                {
                    //same number of cards at max rounds is a tie
                    MyBanner.ShowBanner(Settings.MaxRoundsTie);
                }
            }
            else
            {
                //should never get in here unless player count changes from 2 to more
                //then we need more logic anyway
                //this should be an assert
                MyBanner.ShowBanner("No idea what happened!");
            }
        }

        //coroutine to run until max rounds or a player is out of cards
        IEnumerator WarRoutine()
        {
            //get the result from the round of war
            var result = War();

            //wait for the round time
            yield return new WaitForSeconds(Settings.RoundTime);

            // > 0 means someone won so give them the cards
            if(result >= 0)
            {
                Players[result].AddCards(MyTable.GetCards());  
            }
        
            EvaluateRound();
        }

        int War(Settings.CardPlayType type = Settings.CardPlayType.BATTLE)
        {
            //save the player numbers to read easier (hard coded for two players)
            int Player1 = 0;
            int Player2 = 1;

            //get a card if possible from each player
            var Player1Card = Players[Player1].GetCard();
            var Player2Card = Players[Player2].GetCard();

            //add any cards to the table
            if(Player1Card)
                MyTable.AddCard(Player1, type, Player1Card);
            
            if(Player2Card)
                MyTable.AddCard(Player2, type, Player2Card);

            //if the player didnt have a card the game is over bail out
            if(Player1Card == null || Player2Card == null)
            {
                //Debug.LogError("null card");
                return -1;
            }

            //Debug.Log(Player1Card.CValue + " of " + Player1Card.CSuit + " vs " + Player2Card.CValue + " of " + Player2Card.CSuit);

            //return the player that wins
            if(Player1Card.CValue > Player2Card.CValue)
            {
                //Debug.Log("Player 1 wins");
                return 0;
            }
            //player 2 wins
            else if(Player2Card.CValue > Player1Card.CValue)
            {
                //Debug.Log("Player 2 wins");
                return 1;
            }
            //Tie WAR
            else
            {
                //TIME FOR WAR
                //get the num cards for war from each player
                var Player1Cards = Players[Player1].GetCards(Settings.NumCardsForWar);
                var Player2Cards = Players[Player2].GetCards(Settings.NumCardsForWar);

                //add any cards to the table
                MyTable.AddWarCards(Player1, Player1Cards);
                MyTable.AddWarCards(Player2, Player2Cards);

                //check for lose condition
                if(Player1Cards.Count != Settings.NumCardsForWar || Player2Cards.Count != Settings.NumCardsForWar)
                {
                    //Debug.LogError("not enough cards for war");
                    return -1;
                }

                //now add the war battle cards and evaluate like normal
                return War(Settings.CardPlayType.WAR_BATTLE);
            }
        }
    }
}