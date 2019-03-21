using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace War
{
	public class Deck : MonoBehaviour 
	{
		/// The cards the deck holds
		List<Card> Cards = new List<Card>();

		public int NumCards
		{
			get { return Cards.Count; }
		}

		static Deck Instance = null;
		public static Deck GetInstance()
		{
			//if its the first time getting this instance, then we need to find it and set it.
			//Instance is static so it will remain active the lifetime of the application.
			if (Instance == null)
				Instance = GameObject.Find ("Deck").GetComponent<Deck> ();

			return Instance;
		}

		/// <summary>
		/// We use this to create all the cards for the current deck to hold 
		/// </summary>
		public void CreateCards()
		{
			//we want to loop though each suit
			for (int i = 0; i < (int)Card.CardSuit.MAX; i++)
			{
				//in each suit we will loop though the total number of card types and create them
				for (int c = 0; c < (int)Card.CardValue.MAX; c++)
				{
					var card = CreateCard((Card.CardSuit)i, (Card.CardValue)c);

					//tell the card to show the back and add them to the list the deck holds
					card.ShowBack ();
					Cards.Add (card);
				}
			}
		}

		public Card CreateCard(Card.CardSuit suit, Card.CardValue value)
		{
			//make sure we can find our prefab in resorces
			var _cardObj = Resources.Load ("Prefabs/Card") as GameObject;
			if (_cardObj == null)
			{
				Debug.LogError ("Unable to find card prefab");
				return null;
			}

			//now instatiate (creates an instance of that prefab and adds it to the scene)
			_cardObj = Instantiate (_cardObj);
			if (_cardObj == null)
			{
				Debug.LogError ("Failed to create card prefab");
				return null;
			}

			//we will be using the "Card" component of the newly created card a few times,
			// GetComponent is expensive so we do it once and store it. Because _card was created in this function 
			//it will get freed up after the funtion and destroyed.
			var _card = _cardObj.GetComponent<Card> ();
			if (_card == null)//if for some reason the card doesnt have a card class/component on it we will just add it.
				_card = _cardObj.AddComponent<Card> ();

			_card.CSuit = suit;
			_card.CValue = value;

			//now we want to set the name of the card so it makes sense as we look at it in the hierarchy
			//and set the parent to the deck, so all the cards get hidden underneath 
			_cardObj.name = _card.CValue.ToString() + "_" + _card.CSuit.ToString ();
			_cardObj.transform.SetParent (transform, false);

			//we will now loop though all the children (possible card types) and 
			//remove the ones we are not set to for this specific card.
			//this makes creating them a little heavier but ends up the cleanest so far for me.
			Transform _child = null;

			//loop though and destroy the kids we are not, that we dont need.
			for (int m = 0; m < _cardObj.transform.childCount; m++) 
			{ 
				_child = _cardObj.transform.GetChild (m);
				if (_child.name != Card.CardBack && _child.name != _cardObj.name)
					Destroy (_child.gameObject);
			}

			return _card;
		}

		/// <summary>
		/// Fisher-Yates shuffle
		/// </summary>
		public void ShuffleCards()
		{
			Card _tempCard;
			for (int i = 0; i < Cards.Count; i++)
			{
				//find a random index and store the card at that index
				int r = i + (int)(Random.Range (0.0f, 1.0f) * (Cards.Count - i));
				_tempCard = Cards [r];

				//set the card at the index we safed to the current loop index
				Cards [r] = Cards [i];

				//now we the current loop index card to the card we saved from the random loop
				Cards [i] = _tempCard;
			}

			//offset the position a little to give the stacked deck look
			float _cardOffset = 0;
			for (int i = 0; i < Cards.Count; i++)
			{
				Cards[i].transform.localPosition = new Vector3 (0, -_cardOffset, _cardOffset);
				_cardOffset += 0.01f;
			}
		}

		public List<Card> GetCards(int numCards)
		{
			var maxCardsToTake = Mathf.Min(numCards, Cards.Count);
			var cards = Cards.GetRange(0, maxCardsToTake);

			Cards.RemoveRange(0, maxCardsToTake);
			return cards;
		}
	}
}