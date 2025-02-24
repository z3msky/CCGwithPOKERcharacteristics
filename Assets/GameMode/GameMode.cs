using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Dealer))]
public class GameMode : MonoBehaviour
{
	public TextMeshProUGUI InstructorReadout;

	public Dealer DealerRef
	{
		get
		{
			return GetComponent<Dealer>();
		}
	}
	public bool PlayerCanDragCards
	{
		get
		{
			return m_state.PlayerCanDrag();
		}
	}
	protected GameModeState m_state;

	virtual public void GameSetup()
	{

	}

	virtual public void UpdateStateMachine()
	{

	}

	public void SwapState(GameModeState state)
	{
		if (m_state != null)
		{
			m_state.EndState();
		}

		state.SetGameModeRef(this);
		m_state = state;
		m_state.SetupState();
	}

	public void SetDialogueReadout(string text)
	{
		InstructorReadout.text = text;
	}
}
