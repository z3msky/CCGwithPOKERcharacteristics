using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zone : MonoBehaviour
{
	public string ZoneName;

	private bool m_hover;
	public bool IsHovered
	{
		get
		{
			return m_hover;
		}
		set
		{
			m_hover = value;
			if (m_hover)
			{
				Debug.Log("+hover over " + ZoneName);
			}
			else
			{
				Debug.Log("-hover over " + ZoneName);
			}
		}
	}

	public List<Card> Cards
	{
		get
		{
			List<Card> result = new List<Card>();
			foreach (Transform child in transform)
			{
				Card card = child.GetComponent<Card>();
				if (card != null)
				{
					result.Add(card);
				}
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

	void Update()
	{
		ArrangeCards();
	}

	virtual protected void ArrangeCards()
	{
	}

	virtual public bool AddCard(Card card)
	{
		card.transform.SetParent(transform);
		return true;
	}

	public void ListSubzones()
	{
		foreach (Zone zone in Subzones)
		{
			Debug.Log(zone.ZoneName);
		}
	}
}
