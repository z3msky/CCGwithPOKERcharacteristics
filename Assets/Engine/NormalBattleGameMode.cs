using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBattleGameMode : GameMode
{
	public CardList PlayerDecklist;
	public Pile PlayerDeck;

	private Dealer m_dealer;

	override public void GameSetup()
	{
		m_dealer = GetComponent<Dealer>();

		m_dealer.GenerateDeck(PlayerDecklist, PlayerDeck);
	}

	override public void WhileQueueEmpty()
	{
		Debug.Assert(m_dealer != null);
	}
}
