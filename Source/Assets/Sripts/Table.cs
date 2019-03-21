using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace War
{
    public class Table : MonoBehaviour
    {
        //keep x number of player sections for the table
        PlayerTableSection[] PlayerSections = null;

        //pool of cards currently on the table
        List<Card> CardPool = new List<Card>();

        public void Init()
        {
            //set up each player table section
            PlayerSections = new PlayerTableSection[Settings.NumPlayers];
            for (int i = 0; i < Settings.NumPlayers; i++)
            {
                PlayerSections[i] = new PlayerTableSection();

                var playerHolder =  transform.Find("PlayerHolder" + (i + 1));
                if(playerHolder == null)
                {
                    Debug.LogError("player " + (i  + 1) + " not found");
                    continue;
                }

                PlayerSections[i].BattleHolder = playerHolder.Find("BattleHolder");
                if(PlayerSections[i].BattleHolder == null)
                {
                    Debug.LogError("player " + i + " battle holder not found");
                    continue;
                }

                PlayerSections[i].WarBattleHolder = playerHolder.Find("WarBattleHolder");
                if(PlayerSections[i].WarBattleHolder == null)
                {
                    Debug.LogError("player " + i + " war battle holder not found");
                    continue;
                }

                PlayerSections[i].WarHolders = new List<Transform>();
                for (int h = 0; h < Settings.NumCardsForWar; h++)
                {
                    var holder = playerHolder.Find("WarHolder" + (h + 1));
                    if(holder == null)
                    {
                        Debug.LogError("player " + h + " war holder " + (h + 1) + " not found");
                        continue;
                    }

                    PlayerSections[i].WarHolders.Add(holder);
                }
            }            
        }

        static Table Instance = null;
        public static Table GetInstance()
        {
            //if its the first time getting this instance, then we need to find it and set it.
            //Instance is static so it will remain active the lifetime of the application.
            if (Instance == null)
                Instance = GameObject.Find ("Table").GetComponent<Table> ();

            return Instance;
        }

        //add a card to the table based on the play type and player Id
        public void AddCard(int playerId, Settings.CardPlayType type, Card card)
        {
            if(card == null)
                return;
            
            if(playerId >= PlayerSections.Length)
            {
                Debug.LogError("Player Id " + playerId + " is not valid, selection size is " + PlayerSections.Length);
                return;
            }

            CardPool.Add(card);

            switch (type)
            {
                case Settings.CardPlayType.BATTLE:
                    card.transform.SetParent(PlayerSections[playerId].BattleHolder, false);
                    card.ShowFront();
                    card.transform.SetAsLastSibling();

                    break;
                case Settings.CardPlayType.WAR_BATTLE:
                    card.transform.SetParent(PlayerSections[playerId].WarBattleHolder, false);
                    card.ShowFront();
                    card.transform.SetAsLastSibling();
                    break;
                default:
                    Debug.LogError("Unhandled type passed in");
                    break;
            }
        }

        //add x num cards face down to the war section
        public void AddWarCards(int playerId, List<Card> cards)
        {
            if(PlayerSections[playerId].WarHolders.Count != cards.Count)
            {
                Debug.LogError("Got " + cards.Count + " only expecting " + Settings.NumCardsForWar);
                return;
            }

            for (int i = 0; i < cards.Count; i++)
            {
                if(cards[i] == null)
                {
                    Debug.LogError("null card passed in for  war");
                    return;
                }

                cards[i].transform.SetParent(PlayerSections[playerId].WarHolders[i], false);
                cards[i].transform.SetAsLastSibling();

                CardPool.Add(cards[i]);
            }
        }

        public List<Card> GetCards()
        {
            var cards = new List<Card>(CardPool);
            CardPool.Clear();

            return cards;
        }
    }
}