using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleGameMode : GameMode
{
	public PlayerEnemyCharacter PlayerRef;
	public PlayerEnemyCharacter EnemyRef;

	[Header("Battle Settings")]
	public int StartingLifeTotal = 100;

	[Header("Battle Data")]
	public CardList PlayerDecklist;
	public EnemyPlanData EnemyPlanData;

	[Header("Battle Refs")]
	public Pile PlayerDeck;
	public Pile PlayerHand;
	public UnitRow PlayerBackRow;
	public UnitRow PlayerFrontRow;
	public UnitRow EnemyRow;
	public Slot EnemyGenerateSlot;
	public SelectionSlot SummonSlot;
	public Zone DiscardZone;
	public GameObject SelectorButtonPrefab;
	public GameObject BlockerButtonPrefab;

	public RuntimeEnemyPlan RuntimeEnemyPlan {  get; private set; }
	public int TurnNumber { get; private set; }

	override public void GameSetup()
	{
		Debug.Assert(SelectorButtonPrefab != null, "Battle game mode needs SelectorButtonPrefab");
		//Debug.Log("Setting up regular battle game mode");

		RuntimeEnemyPlan = dealer.GenerateDefaultPlan(Suit.DIAMONDS);

		TurnNumber = 0;
		PlayerRef.MaxLife = StartingLifeTotal;
		EnemyRef.MaxLife = StartingLifeTotal;
		SetZoneOwners();

		//dealer.GenerateDeck(PlayerDecklist, PlayerDeck);
		dealer.GenerateDefaultDeck(PlayerDeck, Suit.HEARTS);
		dealer.GenerateDefaultDeck(PlayerDeck, Suit.SPADES);
		PlayerDeck.Shuffle();

		//StartNextTurn();
		SwapState(new EnemySummonState());
		//SwapState(new PlayerNeutralState());
	}

	override public void UpdateGameMode()
	{
		base.UpdateGameMode();
	}

	private void Update()
	{
	}

	public void ProcessStateBasedEvents()
	{
		//Debug.Log("State-based-Events " + Time.time);
		foreach(IStateBasedEvent item in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IStateBasedEvent>())
		{
			item.CheckStateBasedEvents();
		}
	}

	public void QueueTryDraw(int n = 1)
	{
		if (n < 1) return;

		for (int i = 0; i < n; i++)
		{
			dealer.Queue(new TryDrawOneAction(PlayerDeck, PlayerHand));
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

	public void StartNextTurn()
	{
		TurnNumber++;
		SwapState(new PlayerStartTurnState());
	}

	public void PlayerLose(PlayerEnemyCharacter player)
	{
		dealer.ClearAll();
		if (player == PlayerRef)
		{
			Defeat();
		}

		if (player == EnemyRef)
		{
			Victory();
		}
	}

	private void Victory()
	{
		RestartGamePanel restart = FindAnyObjectByType<RestartGamePanel>(FindObjectsInactive.Include);
		Debug.Assert(restart != null);

		restart.gameObject.SetActive(true);
		restart.SetText("victory");
	}

	private void Defeat()
	{
		RestartGamePanel restart = FindAnyObjectByType<RestartGamePanel>(FindObjectsInactive.Include);
		Debug.Assert(restart != null);

		restart.gameObject.SetActive(true);
		restart.SetText("defeat");
	}
}
