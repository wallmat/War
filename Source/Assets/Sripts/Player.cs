using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace War
{
    public class Player : MonoBehaviour
    {
        //the cards the player has currently (cards on the table are not in the players hand)
        public List<Card> Cards { get; private set; }

        //text to show how many cards are left
        Text CardsLeft = null;

        public void Init()
        {
            //set up the new list of cards
            Cards = new List<Card>();

            var cardsLeftHolder = transform.Find("CardsLeftHolder/CardsLeft");
            if(cardsLeftHolder == null)
            {
                Debug.LogError("Unable to find cards left holder", gameObject);
                return;
            }

            CardsLeft = cardsLeftHolder.GetComponent<Text>();
            if(CardsLeft == null)
            {
                Debug.LogError("Unable to get text comp from cards left", gameObject);
            }
        }

        //add cards to the players hand/pile
        public void AddCards(List<Card> cards)
        {
            //add the cards
            Cards.AddRange(cards);
            
            //flip each card over and set its parent/position
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].ShowBack();
                cards[i].transform.SetParent(transform, false);
            }

            //update my cards left text
            UpdateCardsLeft();
        }

        public Card GetCard()
        {
            if(Cards.Count == 0)
            {
                //Debug.LogError("out of cards", gameObject);
                return null;
            }

            var card = Cards[0];
            Cards.Remove(card);

            UpdateCardsLeft();

            return card;
        }

        public List<Card> GetCards(int numCards)
		{
			var maxCardsToTake = Mathf.Min(numCards, Cards.Count);
			var cards = Cards.GetRange(0, maxCardsToTake);
			
			Cards.RemoveRange(0, maxCardsToTake);

            UpdateCardsLeft();
            
			return cards;
		}

        public void PrintCards(int id)
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Debug.LogError(id + ": " + Cards[i].CValue + " of " + Cards[i].CSuit);
            }
        }

        void UpdateCardsLeft()
        {
            CardsLeft.text = Cards.Count.ToString();
        }
    }
}