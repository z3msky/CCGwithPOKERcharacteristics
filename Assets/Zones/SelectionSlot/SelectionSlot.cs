using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SelectionSlot : Slot
{
	public override bool AddCard(Card card)
	{
		bool result = base.AddCard(card);

		SelectionHandler selector = GetComponent<SelectionHandler>();
		Debug.Assert(selector != null, "Selection slot " + ZoneName + " needs a selection handler!");
		selector.SelectCard(card);

		OnCardEnter.Invoke();

		return result;
	}

	public override bool CanAcceptAsCard(Card card)
	{
		return true;
	}
}
