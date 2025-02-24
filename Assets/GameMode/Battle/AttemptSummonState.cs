using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttemptSummonState : GameModeState
{
	private Card m_summonCard;
	private ZoneLabeller m_selectLabel;
	private BattleGameMode m_battle;
	private List<Button> m_selectorButtons;
	private Button m_cancelButton;

	public AttemptSummonState(Card summonCard)
	{
		m_summonCard = summonCard;
	}

	public override void SetupState()
	{
		m_battle = m_gameMode as BattleGameMode;
		Debug.Assert(m_battle != null, "Cannot attempt summon in non-battle game mode");

		m_selectLabel = m_battle.SummonSlot.GetComponent<ZoneLabeller>();
		
		if (m_selectLabel != null )
		{
			m_selectLabel.SetLabel("Summon");
		}


		GameObject cancelButtonObj = GameObject.Instantiate(m_battle.BlockerButtonPrefab, m_battle.DealerRef.UICanvas.transform);
		m_cancelButton = cancelButtonObj.GetComponent<Button>();
		Debug.Assert(m_cancelButton != null);

		m_cancelButton.onClick.AddListener(() => CancelSummon());

		bool validSlotExists = CreateSelectorButtons();

		if (validSlotExists)
		{
			m_gameMode.SetDialogueReadout("Select a zone to summon " + m_summonCard.CardName);
		}
		else
		{
			m_gameMode.SetDialogueReadout("You cannot play that now");
		}
	}

	override public void UpdateState()
	{
	}

	public override void EndState()
	{
		if (m_selectLabel != null)
		{
			m_selectLabel.SetLabel("Select");
		}

		foreach (Button button in m_selectorButtons)
		{
			GameObject.Destroy(button.gameObject);
		}

		GameObject.Destroy(m_cancelButton.gameObject);
	}

	public bool CreateSelectorButtons()
	{
		UnitRow backrow = m_battle.PlayerBackRow;
		m_selectorButtons = new List<Button>();
		bool validSlotExists = false;

		foreach (Zone zone in backrow.Subzones)
		{
			TraceSlot traceSlot = zone as TraceSlot;
			Debug.Assert(traceSlot != null);

			if (traceSlot.CanAcceptAsSummon(m_summonCard))
			{
				validSlotExists = true;
				GameObject selectorButton = GameObject.Instantiate(m_battle.SelectorButtonPrefab, m_gameMode.DealerRef.UICanvas.transform);

				selectorButton.transform.position = zone.transform.position;
				selectorButton.transform.SetAsLastSibling();
				selectorButton.GetComponent<RectTransform>().sizeDelta = zone.GetComponent<RectTransform>().sizeDelta;
				m_selectorButtons.Add(selectorButton.GetComponent<Button>());

				selectorButton.GetComponent<Button>().onClick.AddListener(() => 
				{ 
					SummonCard(zone);
				});

			}
		}

		return validSlotExists;
	}

	private void SummonCard(Zone zone)
	{
		m_gameMode.DealerRef.Queue(new MoveCardAction(m_summonCard, zone));
		m_gameMode.SwapState(new PlayerNeutralState());
	}

	private void CancelSummon()
	{
		m_gameMode.DealerRef.Queue(new MoveCardAction(m_summonCard, m_battle.PlayerHand));
		m_gameMode.SwapState(new PlayerNeutralState());
	}
}
