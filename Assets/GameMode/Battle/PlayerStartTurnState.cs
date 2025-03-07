using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStartTurnState : GameModeState
{
	BattleGameMode m_battle;

	override protected void SetupState()
	{
		m_battle = m_gameMode as BattleGameMode;
		Debug.Assert(m_battle != null);

		m_gameMode.SetDialogueReadout("Draw your cards");
		m_battle.TryDrawUpTo(5);

		foreach (ITurnResettable resettable in GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ITurnResettable>())
		{
			resettable.ResetForTurn();
		}
	}

	override public void UpdateState()
	{
		m_gameMode.SwapState(new PlayerNeutralState());
	}
}
