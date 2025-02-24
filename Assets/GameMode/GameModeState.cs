using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeState
{
	protected GameMode m_gameMode;

	public void SetGameModeRef(GameMode gameMode)
	{
		m_gameMode = gameMode;
	}

	virtual public void SetupState()
	{

	}

	virtual public void UpdateState()
	{

	}

	virtual public void EndState()
	{

	}

	virtual public bool PlayerCanDrag()
	{
		return false;
	}
}
