using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Card : MonoBehaviour
{
    [Header("CardData Asset")]
    public CardData DragToSetCardData = null;
    
    [Header("Motion")]
	public Vector2 TargetPosition;
    public float LerpRatio;
    public Vector2 FloatingDirection;
    public float FloatingDegree;
    public bool Draggable;

	[Header("Card Design Elements")]
    public Sprite[] RankSprites;
    public Sprite SpadeSprite;
    public Sprite HeartSprite;
    public Sprite ClubSprite;
    public Sprite DiamondSprite;
    public Sprite PairSprite;
    public Sprite ThreeofSprite;
    public Sprite CardBGSprite;
    public Sprite CardBackSprite;
    public float RandomOffsetAmount;

    [Header("Card Design References")]
    public Image FloatingCard;
    public Image RankImage;
    public Image SuitImage;
    public Image CardArtImage;
    public Image CardBGImage;
    public TextMeshProUGUI PowerToughnessText;
    public TextMeshProUGUI CardNameText;
    public TextMeshProUGUI RulesText;

    private Zone m_currZone;
	public Zone CurrentZone {
        get
        {
            if (m_currZone == null)
            {
                m_currZone = FindAnyObjectByType<Dealer>().RootZone as Zone;
            }
            return m_currZone;
        }
        set
        {
            m_currZone = value;
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
            gameObject.name = m_cardData.CardName;
			SetupCardTypeComponents();
		}
    }

    private bool m_revealed;
    public bool Revealed
    {
        get
        {
            return m_revealed;
        }

        set
        {
            m_revealed = value;
            UpdateCardDisplay();
        }
    }

    public bool CanBePlayed()
    {
        return true;
    }

	private ICardMotionStrategy m_currPosStrat;
	private ICardMotionStrategy m_defaultPosStrat;
	private ICardMotionStrategy m_dragPosStrat;


	// Start is called before the first frame update
	void Start()
    {
        TargetPosition = transform.position;
        m_defaultPosStrat = new DefaultCardMotionStrategy();
        m_dragPosStrat = new DragCardMotionStrategy();
        m_currPosStrat = m_defaultPosStrat;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Assert(CurrentZone != null);

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
        UpdateCardRotation();
    }

    void UpdateCardDisplay()
	{
        if (!Revealed)
        {
            CardBGImage.sprite = CardBackSprite;

			CardArtImage.enabled = false;
			RankImage.enabled = false;
			SuitImage.enabled = false;
			CardNameText.enabled = false;
		    PowerToughnessText.enabled = false;
            RulesText.enabled = false;

			return;
		}

        CardBGImage.sprite = CardBGSprite;

		CardArtImage.enabled = true;
		RankImage.enabled = true;
		SuitImage.enabled = true;
		CardNameText.enabled = true;
		PowerToughnessText.enabled = false;
        RulesText.enabled = true;

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
		//CardNameText.text = CardDataAsset.CardName.ToUpper();
		CardNameText.text = CardDataAsset.ShortName.ToUpper();
        RulesText.text = CardDataAsset.RulesText;
		gameObject.name = CardDataAsset.CardName;
	}

    public void UpdateCardRotation()
    {
    }

    public void LerpToward(Vector2 target)
    {
        Vector2 currPos = transform.position;
        transform.position = Vector2.Lerp(currPos, target, LerpRatio * Time.deltaTime);
    }

	public void Drag()
	{
        if (Draggable)
        {
			GetComponent<Image>().enabled = true;
			transform.SetAsLastSibling();
			Zone hoveredZone = FindAnyObjectByType<ZoneManager>().ZoneHoveredOver();
			m_currPosStrat = m_dragPosStrat;
        }
	}

	public void Drop()
	{

        if (Draggable)
        {
            TargetPosition = transform.position;
            m_currPosStrat = m_defaultPosStrat;

            Zone zone = FindAnyObjectByType<ZoneManager>().ZoneHoveredOver();

            FindAnyObjectByType<Dealer>().MoveCardToZone(this, zone);

            FloatingCard.transform.position = transform.position;
            GetComponent<Image>().enabled = false;
        }
	}
}
