using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Card : Slot, ITurnResettable
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
    public Color NormalCardColor;
    public Color TraceCardColor;

    [Header("Card Design References")]
    public Image FloatingCard;
    public Image RankImage;
    public Image SuitImage;
    public Image CardArtImage;
    public RawImage SummoningSickness;
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
    public bool TraceMode { get; set; }

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
			return !IsFace;
		}
	}
    public bool IsFace
    {
        get
        {
            return CardDataAsset.Rank >= 11 && CardDataAsset.Rank <= 13;
        }
    }
    public bool IsNumber
    {
        get
        {
            return CardDataAsset.Rank >= 2 && CardDataAsset.Rank <= 10;
        }
    }
    public bool IsAce
    {
        get
        {
            return CardDataAsset.Rank == 1;
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
    public bool EnteredThisTurn { get; private set; }

    // Positioning strategies
	private ICardMotionStrategy m_currPosStrat;
	private ICardMotionStrategy m_defaultPosStrat;
	private ICardMotionStrategy m_dragPosStrat;

    // Scene References
    private Dealer m_dealer;

    // Comps
    private Animator m_animator;


	// Start is called before the first frame update
	void Start()
    {
        TargetPosition = transform.position;
        m_defaultPosStrat = new DefaultCardMotionStrategy();
        m_dragPosStrat = new DragCardMotionStrategy();
        m_currPosStrat = m_defaultPosStrat;
        TraceMode = false;

        m_dealer = FindAnyObjectByType<Dealer>();
        Debug.Assert(m_dealer != null);

        m_animator = GetComponent<Animator>();
        Debug.Assert(m_animator != null);
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

        foreach (Card card in Cards)
        {
            card.transform.position = transform.position;
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
                case CardType.RITUAL:
                    gameObject.AddComponent<RitualTypeComponent>();
                    break;

            }
        }

        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
	{
        FloatingCard.gameObject.SetActive(!CurrentZone.CardsDisappear());
        SummoningSickness.gameObject.SetActive(EnteredThisTurn && IsCardType(CardType.UNIT));

        if (TraceMode)
        {
            FloatingCard.color = TraceCardColor;
        }
        else
        {
            FloatingCard.color = NormalCardColor;
        }

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

		CardArtImage.enabled = (CardDataAsset.CardArt != null);
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

		CardNameText.text = CardDataAsset.ShortName.ToUpper();
        RulesText.text = CardDataAsset.RulesText;
        if (CardDataAsset.CenteredRules)
        {
            RulesText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            RulesText.verticalAlignment = VerticalAlignmentOptions.Top;
        }
        else
        {
            RulesText.alignment = TextAlignmentOptions.TopJustified;
        }
		gameObject.name = CardDataAsset.CardName + " " + gameObject.GetInstanceID();
	}

    public void UpdateCardRotation()
    {
    }

    public void UpdateCardDisplayHeight()
    {
        Rect old = GetComponent<Image>().rectTransform.rect;

        float hRatio = old.height / DisplayHeightTarget;
		float h = Mathf.Lerp(old.height, DisplayHeightTarget, LerpRatio * Time.deltaTime * 2);

		if (hRatio >= 0.95f || hRatio <= 1.05f)
        {
			GetComponent<Image>().rectTransform
			.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, DisplayHeightTarget);
		}
        else
        {
			GetComponent<Image>().rectTransform
				.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
		}


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

    public void Teleport()
    {
        transform.position = TargetPosition;
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
		if ((!CanBePlayedAsCard) && (!CanBePlayedAsTrace)) return;

		Zone zone = FindAnyObjectByType<ZoneManager>().ZoneHoveredOver();
        Debug.Log("Drop on zone " + zone.ZoneName);

        if (zone.CanAcceptAsTrace(this))
        {
            zone.PlayCardAsTrace(this);
        }

        if (zone.CanAcceptAsCard(this))
        {
            zone.AddCard(this);
        }
	}

    override public bool CardsDisappear()
    {
        return true;
    }

    public void PlayAnimation(string name)
    {
        Debug.Assert(m_animator != null);
        m_animator.Play(name, 0, 0);
    }

    public bool AnimationComplete(string name)
    {
        Debug.Assert(m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(name));
        return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !m_animator.IsInTransition(0);
    }

	public void PlayAttackSound()
	{
		SFXManager sfx = FindAnyObjectByType<SFXManager>();
		Debug.Assert(sfx != null);
		AudioClip clip = sfx.Library.DefaultAttackSound;

		sfx.PlayPitched(clip);
	}

    public void OnEnterArena()
    {
        EnteredThisTurn = true;
    }

    public void ResetForTurn()
    {
        EnteredThisTurn = false;
        UpdateCardDisplay();
    }

    public void Discard(float animLength = 0.001f)
    {
        Zone dest = null;
        if (m_dealer.GameMode is BattleGameMode)
        {
            dest = m_dealer.Battle.DiscardZone;
        }
        CardDiscardAction action = new CardDiscardAction(this, dest, animLength);
    }
}
