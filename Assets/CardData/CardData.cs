using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Card", menuName = "Create New Card Data", order = 0)]
public class CardData : ScriptableObject
{
	[Header("General")]
	public string CardName;
	public string ShortName;
	public Sprite CardArt;
	public CardType[] CardTypes;
	public string RulesText;

	[Range(0, 13)]
	public int Rank;
	public Suit Suit;

	[Header("Unit")]
	public int Power;
	public int Toughness;

	public string RankString
	{
		get
		{
			switch (Rank)
			{
				case 0:
					return "-";
				case 1:
					return "Ace";
				case 11:
					return "Jack";
				case 12:
					return "Queen";
				case 13:
					return "King";
				default:
					return Rank.ToString();
			}
		}
	}

	public bool IsFace {
		get
		{
			return Rank >= 11 & Rank < 13;
		}
	}
	public bool IsNumber
	{
		get
		{
			return Rank >=2 && Rank <= 10;
		}
	}
	public bool IsNonFace
	{
		get
		{
			return !IsFace;
		}
	}
}
