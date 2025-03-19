using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCardMotionStrategy : ICardMotionStrategy
{
	public void UpdateCardPosition(Card card)
	{
		card.transform.position = Input.mousePosition;

		Vector3 FloatingTarget = 
			(Vector3)card.FloatingDirection 
			* card.FloatingDegree
			+ card.transform.position;

		card.GetComponent<CardVisual>().FloatingCard.transform.position = Vector3.Lerp(
			card.GetComponent<CardVisual>().FloatingCard.transform.position, 
			FloatingTarget, 
			Time.deltaTime * 10);
	}
}
