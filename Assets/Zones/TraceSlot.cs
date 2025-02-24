using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraceSlot : Zone
{
	public Zone TraceZone;
	public Image TraceReadoutPanel;
	public TextMeshProUGUI TotalCountText;
	public TextMeshProUGUI SpadeCountText;
	public TextMeshProUGUI HeartCountText;
	public TextMeshProUGUI ClubCountText;
	public TextMeshProUGUI DiamondCountText;

	public bool ValidTraceZone;

	private List<Card> m_traces;
	private Vector3 m_traceImageOffset;


	int m_spade;
	int m_heart;
	int m_club;
	int m_diamond;
	int m_total;

	override protected void ZoneTypeStart()
	{
		TraceZone.ZoneName = ZoneName + "-TraceZone";
		TraceZone.DefaultCardHeight = DefaultCardHeight;

		m_traceImageOffset = TraceReadoutPanel.transform.position - transform.position;
		TraceReadoutPanel.transform.SetParent(DealerRef.UICanvas.transform, true);
	}

	override protected void ZoneTypeUpdate()
	{
		Debug.Assert(Cards.Length <= 1);
		if (Cards.Length > 0)
		{

		}

		UpdateTraceCounts();

		TraceReadoutPanel.transform.position = transform.position + m_traceImageOffset;
	}

	override public bool CanAcceptAsTrace(Card card)
	{
		return card.CanBePlayedAsTrace && Cards.Length == 0 && ValidTraceZone;
	}

	override public bool CanAcceptAsCard(Card card)
	{
		return false;
	}

	public override bool CanAcceptAsSummon(Card card)
	{
		UpdateTraceCounts();

		if (Cards.Length > 0) return false;

		if (card.IsNumber && m_total >= card.Rank)
		{
			return true;
		}

		return false;
	}

	override protected void ArrangeCards()
	{
		Debug.Assert(Cards.Length <= 1);
		if (Cards.Length == 1)
		{
			Cards[0].TargetPosition = transform.position;
		}
	}

	public void PlayCardAsTrace(Card card)
	{
		BattleGameMode battle = DealerRef.GameMode as BattleGameMode;
		Debug.Assert(battle != null, "Cannot play as trace during non-battle game mode");

		FindAnyObjectByType<Dealer>().Queue(new MoveCardAction(card, TraceZone));
		battle.DrawOneCard();
	}

	public void UpdateTraceCounts()
	{
		m_total = 0;
		m_spade = 0;
		m_heart = 0;
		m_club = 0;
		m_diamond = 0;

		foreach (Card card in TraceZone.Cards)
		{
			switch (card.Suit)
			{
				case Suit.SPADES:
					m_spade += card.Rank;
					break;
				case Suit.HEARTS:
					m_heart += card.Rank;
					break;
				case Suit.CLUBS:
					m_club += card.Rank;
					break;
				case Suit.DIAMONDS:
					m_diamond += card.Rank;
					break;
			}
		}

		m_total = m_spade + m_heart + m_club + m_diamond;

		SpadeCountText.text = "x" + m_spade;
		HeartCountText.text = "x" + m_heart;
		ClubCountText.text = "x" + m_club;
		DiamondCountText.text = "x" + m_diamond;
		TotalCountText.text = "x" + m_total;
	}
}
