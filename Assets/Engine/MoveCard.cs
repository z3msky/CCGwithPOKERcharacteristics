using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : DealerAction
{
	private Card m_card;
	private Zone m_src;
	private Zone m_dest;

	public MoveCard(Card card, Zone dest)
	{
		m_card = card;
		m_src = card.CurrentZone;
		m_dest = dest;
	}

	public MoveCard(Card card, Zone src, Zone dest)
	{
		m_card = card;
		m_src = src;
		m_dest = dest;
	}


	override public void Setup()
	{
		Debug.Log("MoveCard");
	}

	public override void Process()
	{
	}
}
