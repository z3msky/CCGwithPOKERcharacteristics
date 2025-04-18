using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackZoneAction : DealerAction
{
	private UnitTypeComponent m_unit;
	private Zone m_tgt;
	private string m_animname;

	override public bool UseTimer
	{
		get { return false; }
	}
	override public bool UpdatesOnFirstFrame
	{
		get { return false; }
	}

	public AttackZoneAction(UnitTypeComponent unit, Zone target, string animName = "Attack")
		: base(0.0f)
	{
		m_unit = unit;
		m_tgt = target;
		m_animname = animName;
	}


	override protected void SetupAction()
	{
		// End if attack is invalid
		if (!m_unit.CanAttackZone(m_tgt))
		{
			//Debug.Log("Cannot attack with " + m_unit.Card.Rank.ToString() + " of " + m_unit.Card.Suit.ToString());
			m_unit.DeclaredAsAttacker = false;
			Complete = true;
			return;
		}

		m_tgt.ResolveDamage(m_unit.Power, m_unit);
		m_unit.transform.SetAsLastSibling();
		m_unit.Card.PlayAnimation(m_animname);


		DealerSpeak dealerSpeak = DealerSpeak.SceneInstance;
		if (m_tgt.Cards.Length > 0 && m_tgt.Cards[0].IsCardType(CardType.UNIT))
		{
			dealerSpeak.SetDialogue(
				m_unit.Card.CardName + " attacks " 
				+ m_tgt.Cards[0].CardName
				+ " for " + m_unit.Power + " damage.");
		}
		else
		{
			dealerSpeak.SetDialogue(
				m_unit.Card.CardName 
				+ " attacks directly for " 
				+ m_unit.Power + " damage.");
		}

	}

	override protected void ProcessAction()
	{
		Complete = m_unit.Card.AnimationComplete(m_animname);
		if (Complete)
		{
			//m_tgt.ResolveDamage(m_unit.Power, m_unit);
			IStateBasedEvent.TestAll();
			m_unit.DeclaredAsAttacker = false;
		}
	}
}
