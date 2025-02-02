using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card List", menuName = "Create New Card list", order = 0)]
public class CardList : ScriptableObject
{
	[SerializeReference]
	public CardData[] Cards;

	public void GenerateDefaultDeck()
	{
		Cards = new CardData[52];
		
		int i = 0;
		foreach (Suit suit in Enum.GetValues(typeof(Suit)))
		{
			if (suit != Suit.JOKER)
			{
				for (int rank = 1; rank <= 13; rank++)
				{
					CardData card = ScriptableObject.CreateInstance<CardData>();
					card.Suit = suit;
					card.Rank = rank;
					Cards[i] = card;

					i++;
				}
			}
		}

	}
}
