using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveCardAction : DealerAction
{
	private Card m_card;
	private Zone m_src;
	private Zone m_dest;
	private bool m_instant;

	public MoveCardAction(Card card, Zone dest, float time = 0.3f, bool instant = false)
		: base(time)
	{
		m_card = card;
		m_src = card.CurrentZone;
		m_dest = dest;
		m_instant = instant;
	}

	public MoveCardAction(Card card, Zone src, Zone dest, float time = 0.3f, bool instant = false)
		: base(time)
	{
		m_card = card;
		m_src = src;
		m_dest = dest;
		m_instant = instant;
	}


	override protected void SetupAction()
	{
		Debug.Assert(m_src != null);
		Debug.Assert(m_dest != null);
		Debug.Assert(m_card != null);
		Debug.Assert(m_src.Cards.Contains(m_card), "MoveCard: Card no longer in source Zone");

		if (m_instant)
		{
			m_dealer.InstantMoveCardToZone(m_card, m_dest);
		}
        else
        {
			m_dealer.LerpMoveCardToZone(m_card, m_dest);
        }
	}

	override protected void ProcessAction()
	{
	}
}
