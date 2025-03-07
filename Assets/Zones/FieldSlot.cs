using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldSlot : Slot, ITurnResettable
{
	public Zone TraceZone;
	public Image TraceDisplay;
	public Image TotalTracePanel;
	public TextMeshProUGUI TotalCountText;
	public TextMeshProUGUI SpadeCountText;
	public TextMeshProUGUI HeartCountText;
	public TextMeshProUGUI ClubCountText;
	public TextMeshProUGUI DiamondCountText;

	public bool ValidTraceZone;
	public Vector3 AdvancePreviewDirection = new Vector3(0, 50, 0);
	public float PreviewAdvanceRetreat = 0;

	private List<Card> m_traces;
	private Vector3 m_traceImageOffset;

	int m_spade;
	int m_heart;
	int m_club;
	int m_diamond;
	int m_total;
	int m_tracePlayedThisTurn;

	override protected void ZoneTypeStart()
	{
		TraceZone.ZoneName = ZoneName + "-TraceZone";
		TraceZone.DefaultCardHeight = DefaultCardHeight;

		m_traceImageOffset = TotalTracePanel.transform.position - transform.position;
		TotalTracePanel.transform.SetParent(DealerRef.UICanvas.transform, true);

		m_tracePlayedThisTurn = 0;

		UpdateTraceCounts();
	}

	public void ResetForTurn()
	{
		m_tracePlayedThisTurn = 0;
	}

	override protected void ZoneTypeUpdate()
	{
		Debug.Assert(Cards.Length <= 1);

		if (Cards.Length > 0 || !ValidTraceZone)
		{
			TraceDisplay.gameObject.SetActive(false);
			TotalTracePanel.gameObject.SetActive(false);
		}
		else
		{
			TraceDisplay.gameObject.SetActive(true);
			TotalTracePanel.gameObject.SetActive(true);
			UpdateTraceCounts();
		}

		TotalTracePanel.transform.position = transform.position + m_traceImageOffset;
	}

	override protected void ArrangeCards()
	{
		Debug.Assert(Cards.Length <= 1);

		if (Cards.Length > 0)
		{
			Card card = Cards[0];
			UnitTypeComponent unit = card.GetComponent<UnitTypeComponent>();
			if (unit != null)
			{
				card.TargetPosition = this.transform.position + (unit.AdvanceRetreatPreviewOffset * DealerRef.UICanvas.scaleFactor);
			}
			else
			{
				card.TargetPosition = this.transform.position;
			}

		}
	}

	override public bool CanAcceptAsTrace(Card card)
	{
		BattleGameMode battle = DealerRef.GameMode as BattleGameMode;
		Debug.Assert(battle != null, "Can only play as trace in battle");

		if (ZoneOwner != battle.PlayerRef)
		{
			Debug.Log("Zone does not belong to player");
			return false;
		}

		return card.CanBePlayedAsTrace 
			&& Cards.Length == 0 
			&& ValidTraceZone
			&& m_tracePlayedThisTurn < 1;
	}

	override public bool CanAcceptAsCard(Card card)
	{
		return false;
	}

	public override bool CanAcceptAsSummon(Card card)
	{
		UpdateTraceCounts();

		if (Cards.Length > 0) 
			return false;

		// Card play costs
		if (card.IsNumber && m_total >= card.Rank)
		{
			return true;
		}

		if (card.IsAce && TraceZone.Cards.Length == 0)
		{
			return true;
		}

		if (card.IsFace)
		{
			int necessaryCount = card.Rank - 10;
			if (CountUnitsOfSuit(card.Suit) >= necessaryCount)
			{
				return true;
			}
		}


		// default false
		return false;
	}

	private int CountUnitsOfSuit(Suit suit)
	{

		BattleGameMode battle = DealerRef.GameMode as BattleGameMode;
		Debug.Assert(battle != null);

		int result = 0;

		foreach (Zone zonef in battle.PlayerFrontRow.Subzones)
		{
			if (zonef.Cards.Length == 0) continue;
			if (zonef.Cards[0].Suit == suit)
				result++;
		}

		foreach (Zone zoneb in battle.PlayerBackRow.Subzones)
		{
			if (zoneb.Cards.Length == 0) continue;
			if (zoneb.Cards[0].Suit == suit)
				result++;
		}

		//Debug.Log("Counted " +  result + " " + suit.ToString());

		return result;
	}

	public override void PlayCardAsTrace(Card card)
	{
		Debug.Log("Play trace");
		BattleGameMode battle = DealerRef.GameMode as BattleGameMode;
		Debug.Assert(battle != null, "Cannot play as trace during non-battle game mode");

		card.TraceMode = true;
		battle.dealer.Queue(new MoveCardAction(card, TraceZone));
		battle.dealer.SFXManager.PlayPitched(battle.dealer.SFXManager.Library.CoinSound);
		m_tracePlayedThisTurn++;
	}

	public void PlayCardAsSummon(Card card, bool OverrideValidity = false)
	{
		Debug.Assert(CanAcceptAsSummon(card) || OverrideValidity);

		MoveCardAction moveCardAction = new MoveCardAction(card, this, 0.8f);
		moveCardAction.StartActionSound = DealerRef.SFXManager.Library.SummonSound;
		DealerRef.GameMode.dealer.Queue(moveCardAction);
		card.OnEnterArena();
		foreach (Card tc in TraceZone.Cards)
		{
			DealerRef.GameMode.dealer.Queue(new MoveCardAction(tc, card, 0));
		}
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
