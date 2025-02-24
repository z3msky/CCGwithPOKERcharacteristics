using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGameMode : GameMode
{
	public CardList PlayerDecklist;
	public Pile PlayerDeck;
	public Pile PlayerHand;
	public UnitRow PlayerBackRow;
	public SelectionSlot SummonSlot;
	public GameObject SelectorButtonPrefab;
	public GameObject BlockerButtonPrefab;

	override public void GameSetup()
	{
		Debug.Assert(SelectorButtonPrefab != null, "Battle game mode needs SelectorButtonPrefab");
		Debug.Log("Setting up regular battle game mode");

		DealerRef.GenerateDeck(PlayerDecklist, PlayerDeck);

		DealerRef.Queue(new MoveCardAction(PlayerDeck.NthCardFromTop(0), PlayerHand));
		DealerRef.Queue(new MoveCardAction(PlayerDeck.NthCardFromTop(1), PlayerHand));
		DealerRef.Queue(new MoveCardAction(PlayerDeck.NthCardFromTop(2), PlayerHand));

		SwapState(new PlayerNeutralState());
	}

	override public void UpdateStateMachine()
	{
		Debug.Assert(DealerRef != null);
		m_state.UpdateState();
	}

	private void Update()
	{
	}



	public void DrawOneCard()
	{
		if (PlayerDeck.Cards.Length > 0)
			DealerRef.Queue(new MoveCardAction(PlayerDeck.NthCardFromTop(0), PlayerHand));
	}
}
