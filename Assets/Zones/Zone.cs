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
	public PlayerStateTracker ZoneOwner { get; set; }

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

	public void Shuffle()
	{
		Random.InitState((int) (System.DateTime.Now.ToBinary() * 1000));
		for (int i = 0; i < m_cards.Count - 1; i++)
		{
			int j = Random.Range(i + 1, m_cards.Count - 1);

			Card tmp = m_cards[i];
			m_cards[i] = m_cards[j];
			m_cards[j] = tmp;

			m_cards[i].transform.SetAsLastSibling();
		}
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
		card.UpdateCardDisplay();

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

	virtual public void DamageZone(int dmg)
	{
		Debug.Assert(ZoneOwner != null);

		ZoneOwner.Damage(dmg);
	}

	virtual public void PlayCardAsTrace(Card card)
	{
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

	virtual public bool CardsDisappear()
	{
		return false;
	}
}
