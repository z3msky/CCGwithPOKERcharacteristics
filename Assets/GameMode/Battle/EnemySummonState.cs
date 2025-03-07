using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemySummonState : GameModeState
{
	private BattleGameMode m_battle;
	private RuntimeEnemyPlan m_plan;

	override protected void SetupState()
	{

	}

	override public void UpdateState()
	{

		m_battle = m_gameMode as BattleGameMode;
		Debug.Assert(m_battle != null, "Cannot attempt summon in non-battle game mode");
		Debug.Assert(m_battle.RuntimeEnemyPlan != null, "Enemy has no plan");

		List<EnemyMove> movesThisTurn = new List<EnemyMove>();
		foreach (EnemyMove m in m_battle.RuntimeEnemyPlan.Moves)
		{
			if (m.Turn <= m_battle.TurnNumber && !m.Complete)
			{
				movesThisTurn.Add(m);
			}
		}

		List<FieldSlot> validSlots = new List<FieldSlot>();
		foreach (Zone z in m_battle.EnemyRow.Subzones)
		{
			Debug.Assert(z is FieldSlot);
			if (z.Cards.Length == 0)
				validSlots.Add(z as FieldSlot);
		}

		foreach (EnemyMove move in movesThisTurn)
		{
			if (move.CardDataAsset.CardTypes.Contains(CardType.UNIT) && validSlots.Count > 0)
			{
				int slotIndex = Random.Range(0, validSlots.Count - 1);


				FieldSlot target = validSlots[slotIndex] as FieldSlot;
				Card card = m_battle.dealer.GenerateCard(move.CardDataAsset, m_battle.EnemyGenerateSlot);
				target.PlayCardAsSummon(card, true);
				move.Complete = true;
				validSlots.Remove(target);
			}
		}

		m_battle.SwapState(new EnemyAttackState());
	}

	public override void EndState()
	{
	}
}
