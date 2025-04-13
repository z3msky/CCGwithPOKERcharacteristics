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
	private Color m_defaultSlotColor;

	public AttemptSummonState(Card summonCard)
	{
		m_summonCard = summonCard;
	}

	override protected void SetupState()
	{
		m_battle = m_gameMode as BattleGameMode;
		Debug.Assert(m_battle != null, "Cannot attempt summon in non-battle game mode");
		m_defaultSlotColor = m_battle.SummonSlot.GetComponent<Image>().color;

		m_selectLabel = m_battle.SummonSlot.GetComponent<ZoneLabeller>();
		
		if (m_selectLabel != null )
		{
			m_selectLabel.SetLabel("Summon");
		}


		GameObject cancelButtonObj = GameObject.Instantiate(m_battle.BlockerButtonPrefab, m_battle.dealer.UICanvas.transform);
		m_cancelButton = cancelButtonObj.GetComponent<Button>();
		Debug.Assert(m_cancelButton != null);

		m_cancelButton.onClick.AddListener(() => CancelSummon());

		bool validSlotExists = CreateSelectorButtons();
		DealerSpeak dealerSpeak = DealerSpeak.SceneInstance;

		if (validSlotExists)
		{
			m_battle.SummonSlot.GetComponent<Image>().color = m_defaultSlotColor;
			dealerSpeak.SetDialogue("Select a zone to summon " + m_summonCard.CardName);
			m_battle.dealer.SFXManager.PlayPitched(m_battle.dealer.SFXManager.Library.SelectForSummonSound);
		}
		else
		{
			m_battle.SummonSlot.GetComponent<Image>().color = Color.red;
			if (m_selectLabel != null)
			{
				m_selectLabel.SetLabel("Invalid");
			}

			if (m_summonCard.IsNumber)
				dealerSpeak.SetDialogue("You can only summon a Numbered card to a zone with total pips greater than or equal to its rank.");
			else if (m_summonCard.IsAce)
				dealerSpeak.SetDialogue("You can only summon an Ace to an empty zone");
			else if (m_summonCard.Rank == 11)
				dealerSpeak.SetDialogue("You can only summon a Jack to an empty zone if you control a unit of the Jack's suit.");
			else if (m_summonCard.Rank == 12)
				dealerSpeak.SetDialogue("You can only summon a Queen to an empty zone if you control two units of the Queen's suit.");
			else if (m_summonCard.Rank == 13)
				dealerSpeak.SetDialogue("You can only summon a King to an empty zone if you control three units of the King's suit.");


			m_battle.dealer.SFXManager.PlayPitched(m_battle.dealer.SFXManager.Library.RejectSound);
		}
	}

	override public void UpdateState()
	{
	}

	public override void EndState()
	{
		m_battle.SummonSlot.GetComponent<Image>().color = m_defaultSlotColor;

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
			FieldSlot traceSlot = zone as FieldSlot;
			Debug.Assert(traceSlot != null);

			if (traceSlot.CanAcceptAsSummon(m_summonCard))
			{
				validSlotExists = true;
				GameObject selectorButton = GameObject.Instantiate(m_battle.SelectorButtonPrefab, m_gameMode.dealer.UICanvas.transform);

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
		m_gameMode.SwapState(new PlayerNeutralState());

		Debug.Assert(zone as FieldSlot != null);
		(zone as FieldSlot).PlayCardAsSummon(m_summonCard);
	}

	private void CancelSummon()
	{
		m_gameMode.dealer.Queue(new MoveCardAction(m_summonCard, m_battle.PlayerHand));
		m_gameMode.SwapState(new PlayerNeutralState());
	}
}
