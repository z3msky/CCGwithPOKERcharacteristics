using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zone : MonoBehaviour
{
	public string ZoneName;
	public bool CardsEnterHidden = false;
	public bool CardsEnterDraggable = false;

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

	protected Canvas CardCanvas;

	private void Start()
	{
		Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
		foreach(Canvas c in canvases)
		{
			if (c.name == "CardCanvas")
				CardCanvas = c;
		}

		Debug.Assert(CardCanvas != null);

		gameObject.name = ZoneName;
	}
	private void Update()
	{
		ArrangeCards();
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

		card.Draggable = CardsEnterDraggable;

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
}
