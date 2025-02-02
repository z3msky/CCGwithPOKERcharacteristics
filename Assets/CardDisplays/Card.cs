using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    [Header("CardData Asset")]
    public CardData DragToSetCardData = null;
    
    [Header("Motion")]
    public float LerpRatio;
	public Vector2 TargetPosition;

	[Header("Card Design Elements")]
    public Sprite[] RankSprites;
    public Sprite SpadeSprite;
    public Sprite HeartSprite;
    public Sprite ClubSprite;
    public Sprite DiamondSprite;

    [Header("Card Design References")]
    public Image RankImage;
    public Image SuitImage;
    public Image CardArtImage;
    public TextMeshProUGUI PowerToughnessText;
    public TextMeshProUGUI CardNameText;

	public Zone CurrentZone
	{
		get
		{
			Debug.Assert(transform.parent.GetComponent<Zone>() != null, "Card is not in a Zone!");
			return transform.parent.GetComponent<Zone>();
		}
	}

    private CardData m_cardData;
    public CardData CardDataAsset
    {
        get 
        {
            return m_cardData;
        }

        set
        {
            m_cardData = value;
			SetupCardTypeComponents();
		}
    }

	private ICardMotionStrategy m_currPosStrat;
	private ICardMotionStrategy m_defaultPosStrat;
	private ICardMotionStrategy m_dragPosStrat;

	// Start is called before the first frame update
	void Start()
    {
        Debug.Log("Start card");

        TargetPosition = transform.position;
        m_defaultPosStrat = new DefaultCardMotionStrategy();
        m_dragPosStrat = new DragCardMotionStrategy();
        m_currPosStrat = m_defaultPosStrat;
    }

    // Update is called once per frame
    void Update()
    {
        m_currPosStrat.UpdateCardPosition(this);

        if (DragToSetCardData != null )
        {
            CardDataAsset = DragToSetCardData;
            DragToSetCardData = null;
        }
    }

    void SetupCardTypeComponents()
    {
        foreach (CardTypeComponent old in GetComponents<CardTypeComponent>())
        {
            Destroy(old);
        }

        foreach (CardType cardType in m_cardData.CardTypes)
        {
            switch (cardType)
            {
                case CardType.UNIT:
                    gameObject.AddComponent<UnitTypeComponent>();
                    break;
            }
        }

        UpdateCardDisplay();
    }

    void UpdateCardDisplay()
	{
		CardArtImage.enabled = true;
		RankImage.enabled = false;
		SuitImage.enabled = false;
		PowerToughnessText.enabled = false;
		CardNameText.enabled = false;

		foreach (CardTypeComponent typeComponent in GetComponents<CardTypeComponent>())
		{
            typeComponent.ActivateDesignElements(this);
		}

        CardArtImage.sprite = CardDataAsset.CardArt;
        RankImage.sprite = RankSprites[CardDataAsset.Rank];
        switch (CardDataAsset.Suit)
        {
			case Suit.SPADES:
                SuitImage.sprite = SpadeSprite;
                break;
            case Suit.HEARTS:
                SuitImage.sprite = HeartSprite;
				break;
			case Suit.CLUBS:
                SuitImage.sprite = ClubSprite;
				break;
			case Suit.DIAMONDS:
                SuitImage.sprite = DiamondSprite;
				break;
		}
        PowerToughnessText.text = CardDataAsset.Power + "/" + CardDataAsset.Toughness;
        CardNameText.text = CardDataAsset.CardName;
	}

    public void LerpTowardTarget()
    {
        Vector2 currPos = transform.position;
        transform.position = Vector2.Lerp(currPos, TargetPosition, LerpRatio * Time.deltaTime);
    }

	public void Drag()
	{
        m_currPosStrat = m_dragPosStrat;
	}

	public void Drop()
	{
        TargetPosition = transform.position;

        foreach (Zone zone in GameObject.FindObjectsOfType<Zone>())
        {
            if (zone.IsHovered)
            {
                GameObject.FindAnyObjectByType<Dealer>().MoveCardToZone(this, zone);
            }
        }

		m_currPosStrat = m_defaultPosStrat;
	}
}
