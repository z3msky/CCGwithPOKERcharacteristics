using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Suit
{
	DIAMONDS,
	CLUBS,
	HEARTS,
	SPADES,
	JOKER
}

public class SuitUtil
{
	public static string SuitUnicode(Suit suit)
	{
		string s = "Joker";

		switch (suit)
		{
			case Suit.SPADES:
				s = "♠";
				break;
			case Suit.HEARTS:
				s = "♥";
				break;
			case Suit.CLUBS:
				s = "♣";
				break;
			case Suit.DIAMONDS:
				s = "♦";
				break;

		}

		return s;
	}
}
