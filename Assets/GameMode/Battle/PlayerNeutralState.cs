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

	override protected void SetupState()
	{

	}

	override public void UpdateState()
	{
		DealerSpeak.SceneInstance.SetDialogue("It's your move");
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
