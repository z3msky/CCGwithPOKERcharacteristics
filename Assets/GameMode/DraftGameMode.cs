using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftGameMode : GameMode
{
	public CardList DebugDeck;
	public Pile EnemyPack;
	public Pile EnemyPile;
	public Pile PlayerPile;
	public SelectionSlot DraftSlot;

	override public void GameSetup()
	{

		Debug.Assert(EnemyPile != null);
		Debug.Assert(PlayerPile != null);
		Debug.Assert(DraftSlot != null);
		Debug.Assert(dealer != null);

		dealer.GenerateDeck(DebugDeck, EnemyPack);
	}

	override public void UpdateGameMode()
	{
		Debug.Assert(dealer != null);
		Debug.Log("Queue Empty");
	}
}
