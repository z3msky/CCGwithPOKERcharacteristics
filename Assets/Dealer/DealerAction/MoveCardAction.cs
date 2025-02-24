using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveCardAction : DealerAction
{
	private Card m_card;
	private Zone m_src;
	private Zone m_dest;

	public MoveCardAction(Card card, Zone dest, float time = 0.3f)
		: base(time)
	{
		m_card = card;
		m_src = card.CurrentZone;
		m_dest = dest;
	}

	public MoveCardAction(Card card, Zone src, Zone dest, float time = 0.3f)
		: base(time)
	{
		m_card = card;
		m_src = src;
		m_dest = dest;
	}


	override protected void SetupAction()
	{
		Debug.Assert(m_src != null);
		Debug.Assert(m_dest != null);
		Debug.Assert(m_card != null);
		Debug.Assert(m_src.Cards.Contains(m_card), "MoveCard: Card no longer in source Zone");

		m_dealer.MoveCardToZone(m_card, m_dest);
	}

	override protected void ProcessAction()
	{
	}
}
