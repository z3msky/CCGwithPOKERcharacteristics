using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyAttackState : GameModeState
{
	private BattleGameMode m_battle;

	override protected void SetupState()
	{
		
	}

	override public void UpdateState()
	{
		Debug.Assert(m_gameMode is BattleGameMode);
		m_battle = (BattleGameMode)m_gameMode;

		foreach (Zone z in m_battle.EnemyRow.Subzones)
		{
			Debug.Assert(z is FieldSlot);
			FieldSlot slot = z as FieldSlot;

			if (slot.Cards.Length > 0 && slot.Cards[0].IsCardType(CardType.UNIT))
			{
				UnitTypeComponent unit = slot.Cards[0].GetComponent<UnitTypeComponent>();
				Debug.Assert(unit != null);

				unit.QueueTryAttack(m_battle.PlayerFrontRow, "EnemyAttack");
			}
		}

		m_battle.StartNextTurn();
	}

	public override void EndState()
	{
	}
}
