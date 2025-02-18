using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBattleGameMode : GameMode
{
	public CardList PlayerDecklist;
	public Pile PlayerDeck;
	public Pile PlayerHand;
	public bool DrawACard;

	private Dealer m_dealer;

	override public void GameSetup()
	{

		Debug.Log("Setting up regular battle game mode");

		m_dealer = GetComponent<Dealer>();
		Debug.Assert(m_dealer != null);

		m_dealer.GenerateDeck(PlayerDecklist, PlayerDeck);

		m_dealer.Queue(new MoveCardAction(PlayerDeck.NthCardFromTop(0), PlayerHand));
		m_dealer.Queue(new MoveCardAction(PlayerDeck.NthCardFromTop(1), PlayerHand));
		m_dealer.Queue(new MoveCardAction(PlayerDeck.NthCardFromTop(2), PlayerHand));
	}

	override public void WhileQueueEmpty()
	{
		Debug.Assert(m_dealer != null);
		Debug.Log("Queue Empty");
	}

	private void Update()
	{
		if (DrawACard)
		{
			m_dealer.Queue(new MoveCardAction(PlayerDeck.NthCardFromTop(0), PlayerHand));
			DrawACard = false;
		}
	}
}
