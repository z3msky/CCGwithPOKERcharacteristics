using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeclareAttackState: GameModeState
{

	override public GameModeState NextGameModePhase
	{
		get
		{
			return new PlayerAttackState();
		}
	}

	List<Button> m_selectorButtons;
	List<ZoneBorder> m_selectorBorders;
	private BattleGameMode m_battle;

	override protected void SetupState()
	{
		m_battle = m_gameMode as BattleGameMode;
		Debug.Assert(m_battle != null);

		m_selectorButtons = new List<Button>();
		m_selectorBorders = new List<ZoneBorder>();
		List<UnitTypeComponent> units = new List<UnitTypeComponent>();

		foreach (Zone zone in m_battle.PlayerBackRow.Subzones)
		{
			foreach (Card card in zone.Cards)
			{
				UnitTypeComponent unitTypeComponent = card.GetComponent<UnitTypeComponent>();
				if (unitTypeComponent != null)
				{
					units.Add(unitTypeComponent);
				}
			}
		}

		foreach (Zone zone in m_battle.PlayerFrontRow.Subzones)
		{
			foreach (Card card in zone.Cards)
			{
				UnitTypeComponent unitTypeComponent = card.GetComponent<UnitTypeComponent>();
				if (unitTypeComponent != null)
				{
					units.Add(unitTypeComponent);
				}
			}
		}

		bool validAttackerExists = false;
		foreach (UnitTypeComponent unit in units)
		{
			Zone zone = unit.Card.CurrentZone;
			if (!unit.CanAttack())
			{
				if (unit.Card.EnteredThisTurn)
					m_gameMode.SetDialogueReadout("Units cannot attack on the turn they enter.");
				continue;
			}

			validAttackerExists = true;

			GameObject selectorButton = GameObject.Instantiate(m_battle.SelectorButtonPrefab, m_gameMode.dealer.UICanvas.transform);
			selectorButton.transform.position = zone.transform.position;
			selectorButton.transform.SetAsLastSibling();
			selectorButton.GetComponent<RectTransform>().sizeDelta = zone.GetComponent<RectTransform>().sizeDelta;
			m_selectorButtons.Add(selectorButton.GetComponent<Button>());

			ZoneBorder border = selectorButton.GetComponentInChildren<ZoneBorder>();
			border.transform.SetParent(m_gameMode.dealer.DecorationCanvas.transform);
			m_selectorBorders.Add(border);

			selectorButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				unit.DeclaredAsAttacker = !unit.DeclaredAsAttacker;
				m_battle.dealer.SFXManager.PlayPitched(m_battle.dealer.SFXManager.Library.SelectLow);
			});

			unit.DeclaredAsAttacker = true;
		}

		if (validAttackerExists)
			m_gameMode.SetDialogueReadout("Your units will attack if able, but you may order them to hold off.");
	}

	override public void UpdateState()
	{

	}

	public override void EndState()
	{
		foreach (Button button in m_selectorButtons)
		{
			GameObject.Destroy(button.gameObject);
		}

		foreach (ZoneBorder border in m_selectorBorders)
		{
			GameObject.Destroy(border.gameObject);
		}
	}

	override public bool PlayerCanDrag()
	{
		return false;
	}

	override public bool UsesNextPhaseButton(out string Label)
	{
		Label = "Attack";
		return true;
	}
}
