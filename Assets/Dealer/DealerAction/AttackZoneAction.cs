using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackZoneAction : DealerAction
{
	private UnitTypeComponent m_unit;
	private Zone m_tgt;
	override public bool UseTimer
	{
		get { return false; }
	}
	override public bool UpdatesOnFirstFrame
	{
		get { return false; }
	}

	public AttackZoneAction(UnitTypeComponent unit, Zone target, float time = 0.3f)
		: base(time)
	{
		m_unit = unit;
		m_tgt = target;
	}


	override protected void SetupAction()
	{

		// End if attack is invalid
		if (!m_unit.CanAttackZone(m_tgt))
		{
			Debug.Log("Cannot attack with " + m_unit.Card.Rank.ToString() + " of " + m_unit.Card.Suit.ToString());
			Complete = true;
			return;
		}

		m_unit.Card.PlayAnimation("Attack");
		m_tgt.DamageZone(m_unit.Power);
	}

	override protected void ProcessAction()
	{
		Complete = m_unit.Card.AnimationComplete("Attack");
		if (Complete)
		{
			m_unit.DeclaredAsAttacker = false;
		}
	}
}
