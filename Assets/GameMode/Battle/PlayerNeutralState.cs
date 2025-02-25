using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNeutralState: GameModeState
{

	override public GameModeState NextGameModePhase
	{
		get
		{
			return new PlayerDeclareAttackState();
		}
	}

	override public void SetupState()
	{

	}

	override public void UpdateState()
	{
		m_gameMode.SetDialogueReadout("It's your move");
	}

	override public bool PlayerCanDrag()
	{
		return true;
	}

	override public bool UsesNextPhaseButton(out string Label)
	{
		Label = "Combat";
		return true;
	}
}
