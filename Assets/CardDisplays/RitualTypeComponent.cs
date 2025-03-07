using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualTypeComponent : CardTypeComponent
{
	public int Power;
	public int Toughness;
	public bool DeclaredAsAttacker = false;
	public Vector3 PreviewAdvanceDir = new Vector3(0, 40, 0);
	public Vector3 AdvanceRetreatPreviewOffset {  get; private set; }
	public float LerpSpeed = 10;

	private Dealer m_dealer;

	void Start()
    {
		m_dealer = FindAnyObjectByType<Dealer>();
		Debug.Assert(m_dealer != null);

        CardTypeOfComponent = CardType.RITUAL;
		AdvanceRetreatPreviewOffset = Vector3.zero;
        AnnounceAttachToCard();
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

		card.PowerToughnessText.text = card.CardDataAsset.RitualCostDesc;
	}
}
