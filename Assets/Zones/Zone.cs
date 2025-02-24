using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Zone : MonoBehaviour
{
	public string ZoneName;
	public float DefaultCardHeight = 150;
	public bool PlayerCanDragCards = false;
	public bool CardsEnterHidden = false;
	public UnityEvent OnCardEnter;

	private List<Card> m_cards = new List<Card>();
	public Card[] Cards
	{
		get
		{
			int count = m_cards.Count;
			Card[] result = new Card[count];

			for (int i = 0; i < count; i++)
			{
				result[i] = m_cards[i];
			}

			return result;
		}
	}

	public List<Zone> Subzones 
    { 
        get
        {
			List<Zone> result = new List<Zone>();
            foreach (Transform child in transform)
            {
                Zone zone = child.GetComponent<Zone>();
                if (zone != null)
                {
                    result.Add(zone);
                }
            }
            return result;
        } 
    }

	protected Canvas CardCanvasRef;
	protected Dealer DealerRef;
	protected GameMode GameModeRef;

	private void Start()
	{
		Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
		foreach(Canvas c in canvases)
		{
			if (c.name == "CardCanvas")
				CardCanvasRef = c;
		}
		Debug.Assert(CardCanvasRef != null);
		
		DealerRef = FindAnyObjectByType<Dealer>();
		Debug.Assert(DealerRef != null);
		
		GameModeRef = DealerRef.GetComponent<GameMode>();
		Debug.Assert(GameModeRef != null);

		gameObject.name = ZoneName;

		ZoneTypeStart();
	}
	private void Update()
	{
		ArrangeCards();
		ZoneTypeUpdate();
	}

	virtual protected void ZoneTypeStart()
	{

	}

	virtual protected void ZoneTypeUpdate()
	{

	}

	virtual public bool AddCard(Card card)
	{
		Debug.Assert(!m_cards.Contains(card));
		card.CurrentZone.RemoveCard(card);

		if (CardsEnterHidden)
		{
			card.Revealed = false;
		}
		else
		{
			card.Revealed = true;
		}

		card.DisplayHeightTarget = DefaultCardHeight;

		m_cards.Add(card);
		card.CurrentZone = this;

		return true;
	}

	protected void RemoveCard(Card card)
	{
		if (m_cards.Contains(card))
			m_cards.Remove(card);

		card.CurrentZone = null;
	}

	virtual protected void ArrangeCards()
	{
	}

	public void ListSubzones()
	{
		foreach (Zone zone in Subzones)
		{
			Debug.Log(zone.ZoneName);
		}
	}

	virtual public void TryPlayCardToZone(Card card)
	{
		// Play the card as a trace
		if (CanAcceptAsTrace(card) && !CanAcceptAsCard(card))
		{
			TraceSlot slot = this as TraceSlot;
			Debug.Assert(slot != null, "Attempted to play trace to invalid zone!");

			slot.PlayCardAsTrace(card);
		}

		if (CanAcceptAsCard(card))
		{
			this.AddCard(card);
		}
	}

	virtual public bool CanAcceptAsTrace(Card card)
	{
		return false;
	}

	virtual public bool CanAcceptAsCard(Card card)
	{
		return false;
	}

	virtual public bool CanAcceptAsSummon(Card card)
	{
		return false;
	}
}
