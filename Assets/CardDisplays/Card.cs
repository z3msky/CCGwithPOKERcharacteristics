using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Card : Zone
{
    [Header("CardData Asset")]
    public CardData DragToSetCardData = null;
    
    [Header("Motion")]
	public Vector2 TargetPosition;
    public float LerpRatio;
    public Vector2 FloatingDirection;
    public float FloatingDegree;

	[Header("Card Design Elements")]
    public Sprite[] RankSprites;
    public Sprite SpadeSprite;
    public Sprite HeartSprite;
    public Sprite ClubSprite;
    public Sprite DiamondSprite;
    public Sprite PairSprite;
    public Sprite ThreeofSprite;
    public Sprite CardBGSprite;
    public Sprite CardBackLogoSprite;

	[Header("CardDesignSettings")]
	public float RandomOffsetAmount;
    public float DisplayHeightTarget;

    [Header("Card Design References")]
    public Image FloatingCard;
    public Image RankImage;
    public Image SuitImage;
    public Image CardArtImage;
    public GameObject Cardback;
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
			Debug.Assert(m_cardData != null, gameObject.name + "Has no card data asset!");
			return m_cardData;
        }

        set
        {
            m_cardData = value;
            gameObject.name = m_cardData.CardName;
			SetupCardTypeComponents();
		}
    }

    public string CardName
    {
        get
        {
            string result = CardDataAsset.CardName;
            return result;
        }
    }
    public int Rank
    {
        get { return CardDataAsset.Rank; }
    }
    public Suit Suit
    {
        get
        {
            return CardDataAsset.Suit;
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

    public bool CanBePlayedAsCard
    {
        get
        {
            return true;
        }
    }
	public bool CanBePlayedAsTrace
	{
		get
		{
			return CardDataAsset.IsNonFace;
		}
	}
    public bool IsFace
    {
        get
        {
            return CardDataAsset.IsFace;
        }
    }
    public bool IsNumber
    {
        get
        {
            return CardDataAsset.IsNumber;
        }
    }
    public bool IsCardType(CardType type)
    {
        foreach (CardTypeComponent co in GetComponentsInChildren<CardTypeComponent>())
        {
            if (co.CardTypeOfComponent == type)
                return true;
        }

        return false;
    }

    // Positioning strategies
	private ICardMotionStrategy m_currPosStrat;
	private ICardMotionStrategy m_defaultPosStrat;
	private ICardMotionStrategy m_dragPosStrat;

    // Scene References
    private Dealer m_dealer;


	// Start is called before the first frame update
	void Start()
    {
        TargetPosition = transform.position;
        m_defaultPosStrat = new DefaultCardMotionStrategy();
        m_dragPosStrat = new DragCardMotionStrategy();
        m_currPosStrat = m_defaultPosStrat;

        m_dealer = FindAnyObjectByType<Dealer>();
        Debug.Assert(m_dealer != null);
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

		UpdateCardRotation();
		UpdateCardDisplayHeight();
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
        if (!Revealed)
        {
			CardArtImage.enabled = false;
			RankImage.enabled = false;
			SuitImage.enabled = false;
			CardNameText.enabled = false;
		    PowerToughnessText.enabled = false;
            RulesText.enabled = false;

            Cardback.gameObject.SetActive(true);

			return;
		}

		Cardback.gameObject.SetActive(false);

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
		gameObject.name = CardDataAsset.CardName + " " + gameObject.GetInstanceID();
	}

    public void UpdateCardRotation()
    {
    }

    public void UpdateCardDisplayHeight()
    {
        Rect old = GetComponent<Image>().rectTransform.rect;
        float h = Mathf.Lerp(old.height, DisplayHeightTarget, LerpRatio * Time.deltaTime * 2);

        GetComponent<Image>().rectTransform
            .SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        // kind of lazy but using opacity
        // because TypeComponent deals with enabling
        if (h >= 150)
        {
            RulesText.alpha = 1;
        }
        else
        {
            RulesText.alpha = 0;
        }
	}

    public void LerpToward(Vector2 target)
    {
        Vector2 currPos = transform.position;
        transform.position = Vector2.Lerp(currPos, target, LerpRatio * Time.deltaTime);
    }

	public void Drag()
	{
        if (!CurrentZone.PlayerCanDragCards) return;
        if (!m_dealer.GameMode.PlayerCanDragCards) return;
        if (m_dealer.DealerIsActive) return;

		GetComponent<Image>().enabled = true;
		transform.SetAsLastSibling();
		m_currPosStrat = m_dragPosStrat;
	}

	public void Drop()
	{
        // Do this stuff regardless
		TargetPosition = transform.position;
		m_currPosStrat = m_defaultPosStrat;
		GetComponent<Image>().enabled = false;
		FloatingCard.transform.position = transform.position;

		// Everything after here only if valid
		if (!CurrentZone.PlayerCanDragCards) return;
		if (!m_dealer.GameMode.PlayerCanDragCards) return;
		if (m_dealer.DealerIsActive) return;
		if (!CanBePlayedAsCard && !CanBePlayedAsTrace) return;

		Zone zone = FindAnyObjectByType<ZoneManager>().ZoneHoveredOver();
		zone.TryPlayCardToZone(this);
	}
}
