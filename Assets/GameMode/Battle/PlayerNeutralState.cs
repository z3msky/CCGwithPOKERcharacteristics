using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNeutralState: GameModeState
{
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
}
