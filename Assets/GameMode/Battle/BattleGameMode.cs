using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGameMode : GameMode
{
	public PlayerStateTracker PlayerRef;
	public PlayerStateTracker EnemyRef;

	public Pile PlayerHand;
	public CardList PlayerDecklist;
	public Pile PlayerDeck;
	public UnitRow PlayerBackRow;
	public UnitRow PlayerFrontRow;
	public UnitRow EnemyRow;
	public SelectionSlot SummonSlot;
	public GameObject SelectorButtonPrefab;
	public GameObject BlockerButtonPrefab;

	public int TurnNumber {  get; private set; }

	override public void GameSetup()
	{
		Debug.Assert(SelectorButtonPrefab != null, "Battle game mode needs SelectorButtonPrefab");
		//Debug.Log("Setting up regular battle game mode");

		PlayerRef.MaxLife = 40;
		EnemyRef.MaxLife = 40;
		SetZoneOwners();

		//DealerRef.GenerateDeck(PlayerDecklist, PlayerDeck);
		m_dealer.GenerateDefaultDeck(PlayerDeck, Suit.HEARTS);
		PlayerDeck.Shuffle();

		SwapState(new PlayerStartTurnState());
		//SwapState(new PlayerNeutralState());
	}

	override public void UpdateGameMode()
	{
		base.UpdateGameMode();
	}

	private void Update()
	{
	}

	public void QueueTryDraw(int n = 1)
	{
		if (n < 1) return;

		for (int i = 0; i < n; i++)
		{
			m_dealer.Queue(new TryDrawOneAction(PlayerDeck, PlayerHand));
		}
	}

	public void TryDrawUpTo(int n)
	{
		int m = n - PlayerHand.Cards.Length;
		if (m > 0)
		{
			QueueTryDraw(m);
		}
	}

	private void SetZoneOwners()
	{
		foreach (Zone zone in EnemyRow.Subzones)
		{
			zone.ZoneOwner = EnemyRef;
		}

		foreach (Zone zone in PlayerBackRow.Subzones)
		{
			zone.ZoneOwner = PlayerRef;
		}

		foreach (Zone zone in PlayerFrontRow.Subzones)
		{
			zone.ZoneOwner = PlayerRef;
		}
	}
}
