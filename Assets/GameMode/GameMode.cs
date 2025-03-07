using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dealer))]
public class GameMode : MonoBehaviour
{
	public TextMeshProUGUI InstructorReadout;
	public Button NextPhaseButton;

	public Dealer dealer
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
	virtual public GameModeState NextGameModePhase
	{
		get
		{
			return null;
		}
	}

	private GameModeState m_state;

	virtual public void GameSetup()
	{

	}

	virtual public void UpdateGameMode()
	{
		Debug.Assert(dealer != null);
		if (m_state.Started)
		{
			m_state.UpdateState();
		}
		else
		{
			m_state.Setup();
			m_state.Started = true;
		}
	}

	public void SwapState(GameModeState state)
	{
		if (m_state != null)
		{
			m_state.EndState();
		}

		m_state = state;
		m_state.SetGameModeRef(this);
		
		Debug.Assert(m_state != null);
		Debug.Assert(NextPhaseButton != null);
		string buttonLabel;
		if (m_state.UsesNextPhaseButton(out buttonLabel))
		{
			Debug.Assert(m_state.NextGameModePhase != null);
			NextPhaseButton.gameObject.SetActive(true);
			NextPhaseButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonLabel;
			NextPhaseButton.onClick.RemoveAllListeners();
			NextPhaseButton.onClick.AddListener(() => SwapState(m_state.NextGameModePhase));
		}
		else
		{
			NextPhaseButton.gameObject.SetActive(false);
		}

		m_state.Started = false;
	}

	public void SetDialogueReadout(string text)
	{
		InstructorReadout.text = text;
	}
}
