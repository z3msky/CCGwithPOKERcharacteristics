using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Slot : Zone
{
	override protected void ArrangeCards()
	{
		foreach (var card in Cards)
		{
			card.TargetPosition = this.transform.position;
		}
	}
}
