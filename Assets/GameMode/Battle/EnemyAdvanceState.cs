using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyAdvanceState : GameModeState
{
	private BattleGameMode m_battle;

	override protected void SetupState()
	{
		
	}

	override public void UpdateState()
	{
		Debug.Assert(m_gameMode is BattleGameMode);
		m_battle = (BattleGameMode)m_gameMode;

		foreach (Zone z in m_battle.EnemyIntentRow.Subzones)
		{
			Debug.Assert(z is FieldSlot);
			FieldSlot intentSlot = (FieldSlot)z;

			Card card = intentSlot.CardInSlot;
			if (card == null)
				continue;

			if (!card.IsCardType(CardType.UNIT))
				continue;
			Debug.Assert(card.GetComponent<UnitTypeComponent>() != null);

			UnitTypeComponent unit = card.GetComponent<UnitTypeComponent>();
			Debug.Assert(unit != null);

			if (unit.CanAdvanceOrRetreat(AdvRetType.ADVANCE))
			{
				m_gameMode.dealer.Queue(new TryAdvanceOrRetreatAction(AdvRetType.ADVANCE, unit));
			}
		}

		m_battle.SwapState(new EnemyAttackState());
	}

	public override void EndState()
	{
	}
}
