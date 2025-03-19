using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDiscardAction : DealerAction
{
	private Card m_card;
	private Zone m_dest;
	float m_movTime;

	float m_time;

	public CardDiscardAction(Card card, Zone dest = null, float animTime = 0.3f, float movTime = 0.3f)
		: base(animTime)
	{
		m_card = card;
		m_dest = dest;
		m_movTime = movTime;
	}

	override protected void SetupAction()
	{

	}

	override protected void ProcessAction()
	{

		float tNorm = Timer / HoldTime;

		if (tNorm > 1)
		{
			m_dealer.CutToNextInQueue(new MoveCardAction(m_card, m_dest, m_movTime));
			Complete = true;
		}
	}
}
