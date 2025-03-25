using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AdvRetButton : MonoBehaviour
{
    public Card MyCard;
	public AdvRetType MyType;

	public void UpdateInteractability(UnitTypeComponent unit)
	{
		Zone target;
		GetComponent<Button>().interactable = unit.CanAdvanceOrRetreat(MyType, out target);
	}

    public void TryMove()
    {
		Debug.Assert(MyCard != null);
		
		Dealer dealer = FindAnyObjectByType<Dealer>();
		Debug.Assert(dealer != null);
		if (!dealer.DealerIsActive)
		{
			UnitTypeComponent unit = MyCard.GetComponent<UnitTypeComponent>();
			if (unit != null)
			{
				unit.QueueTryAdvanceOrRetreat(MyType);
			}
		}
	}
}
