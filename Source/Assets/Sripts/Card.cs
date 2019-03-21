using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace War
{
	public class Card : MonoBehaviour 
	{
		//Use this to avoid hard typing in case the name of the card back changes in the future
		public const string CardBack = "Back";
		
		/// <summary>
		/// enum used to store card suit, we add a NONE option so when the 
		/// card is created it defults to none as a way to know its not set up yet
		/// we add MAX as a way to loop though the list easily 
		/// </summary>
		public enum CardSuit { NONE = -1, HEARTS, SPADES, DIAMONDS, CLUBS, MAX};
		public enum CardValue { NONE = -1, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE, MAX };

		/// <summary>
		/// each card will need to hold a suit and a value
		/// </summary>
		public CardSuit CSuit = CardSuit.NONE;
		public CardValue CValue = CardValue.NONE;

		/// <summary>
		/// flip the card to show the back
		/// </summary>
		public void ShowBack()
		{
			//because we are using a ui system currently, 
			//to show the card back or front we need to set 
			//it as the last sibling to give it priority
			transform.Find (CardBack).SetAsLastSibling ();
		}

		/// <summary>
		/// flip the card to show the front
		/// </summary>
		public void ShowFront()
		{
			transform.Find (CardBack).SetAsFirstSibling ();
		}
	}
}