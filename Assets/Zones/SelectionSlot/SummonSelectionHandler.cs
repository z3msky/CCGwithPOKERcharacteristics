using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSelectionHandler : SelectionHandler
{
	override public void SelectCard(Card card)
	{
		Dealer dealer = FindAnyObjectByType<Dealer>();
		Debug.Assert(dealer != null);

		dealer.GameMode.SwapState(new AttemptSummonState(card));
	}
}
