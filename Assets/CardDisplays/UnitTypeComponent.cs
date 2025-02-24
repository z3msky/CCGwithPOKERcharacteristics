using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTypeComponent : CardTypeComponent
{
    void Start()
    {
        CardTypeOfComponent = CardType.UNIT;
        //AnnounceAttachToCard();
	}

    void Update()
    {
        
    }

	public override void ActivateDesignElements(Card card)
	{

		card.RankImage.enabled = true;
		card.SuitImage.enabled = true;
		card.PowerToughnessText.enabled = true;
		card.CardNameText.enabled = true;
	}
}
