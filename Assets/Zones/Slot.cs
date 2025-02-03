using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : Zone
{
	override protected void ArrangeCards()
	{
		if (Cards.Length == 0)
			return;

		Cards[0].TargetPosition = this.transform.position;
	}

	public override bool AddCard(Card card)
	{
		if (Cards.Length > 0)
		{
			GameObject.Destroy(Cards[0].gameObject);
		}

		return base.AddCard(card);
	}
}
