using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCardMotionStrategy : ICardMotionStrategy
{
	public void UpdateCardPosition(Card card)
	{
		card.transform.position = Input.mousePosition;
	}
}
