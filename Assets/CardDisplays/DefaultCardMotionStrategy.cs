using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCardMotionStrategy : ICardMotionStrategy
{
	public void UpdateCardPosition(Card card)
	{
		card.LerpTowardTarget();
	}
}
