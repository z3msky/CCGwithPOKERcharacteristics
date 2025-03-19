using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackState: GameModeState
{
	override public GameModeState NextGameModePhase
	{
		get
		{
			return new PlayerStartTurnState();
		}
	}


	private BattleGameMode m_battle;

	override protected void SetupState()
	{
		m_battle = m_gameMode as BattleGameMode;
		Debug.Assert(m_battle != null);
		List<UnitTypeComponent> units = new List<UnitTypeComponent>();

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

		foreach (UnitTypeComponent unit in units)
		{
			if (!unit.DeclaredAsAttacker)
				continue;
			
			if (!unit.Card.HasKeywordAbility(Keyword.Ranged))
			{
				unit.QueueTryAdvanceOrRetreat();
			}
			unit.QueueTryAttack(m_battle.EnemyRow);
		}

		Debug.Log("PA setup");
	}

	override public void UpdateState()
	{
		m_gameMode.SetDialogueReadout("");
		m_gameMode.SwapState(new EnemySummonState());
	}

	public override void EndState()
	{
	}

	override public bool PlayerCanDrag()
	{
		return false;
	}

	override public bool UsesNextPhaseButton(out string Label)
	{
		Label = "Attack";
		return false;
	}
}
