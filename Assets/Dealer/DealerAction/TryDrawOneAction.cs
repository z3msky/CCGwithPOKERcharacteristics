using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryDrawOneAction : DealerAction
{
	private Pile m_pile;
	private Zone m_target;

	public TryDrawOneAction(Pile sourcePile, Zone targetZone, float time = 0.3f)
		: base(time)
	{
		m_pile = sourcePile;
		m_target = targetZone;
	}

	override protected void SetupAction()
	{
		Debug.Assert(m_pile != null);
		Debug.Assert(m_target != null);


		if (m_pile.Cards.Length > 0)
		{
			m_dealer.LerpMoveCardToZone(m_pile.CardFromTop(), m_target);
			m_dealer.SFXManager.PlayPitched(m_dealer.SFXManager.Library.DrawSound);
		}
	}

	override protected void ProcessAction()
	{
	}
}
