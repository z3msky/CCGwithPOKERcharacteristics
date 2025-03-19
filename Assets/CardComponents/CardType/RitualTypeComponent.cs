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

	public override void ActivateDesignElements(CardVisual cv)
	{
		cv.RankImage.gameObject.SetActive(true);
		cv.SuitImage.gameObject.SetActive(true);
		cv.PowerToughnessText.gameObject.SetActive(true);
		cv.CardNameText.gameObject.SetActive(true);

		cv.PowerToughnessText.text = cv.card.CardDataAsset.RitualCostDesc;
	}
}
