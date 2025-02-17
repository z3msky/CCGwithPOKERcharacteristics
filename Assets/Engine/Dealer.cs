using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public GameObject EmptyCardObject;
    public Zone RootZone;
    public Zone TestCardZone;
    public Canvas MainCanvas;
    public Canvas CardCanvas;
    public CardList TestDeck;
    public Pile TestDeckPile;

    private GameMode m_gameMode;
    private Queue<DealerAction> m_queue;

    void Start()
    {
        m_gameMode = GetComponent<GameMode>();
        m_queue = new Queue<DealerAction>();

        Debug.Assert(m_gameMode != null);

        m_gameMode.GameSetup();
    }

    void Update()
    {
        
    }

    public void Queue(DealerAction action)
    {
        m_queue.Enqueue(action);
    }

    public void MoveCardToZone(Card card, Zone dest)
    {
        Zone src = card.CurrentZone;

        if (src == dest)
        {
            Debug.Log("[" + card.CardDataAsset.CardName + "] stayed in zone [" + src.name);
            return;
        }

        Debug.Log("Move [" + card.CardDataAsset.CardName + "] from [" + src.name + "] -> [" + dest.ZoneName + "]");
        src.RemoveCard(card);
        dest.AddCard(card);
    }

    public Card GenerateCard(CardData card, Zone dest)
    {
		Card result = GameObject.Instantiate(EmptyCardObject, CardCanvas.transform).GetComponent<Card>();
        result.transform.position = dest.transform.position;
        result.DragToSetCardData = card;
		dest.AddCard(result);

        return result;
	}

    public void GenerateDeck(CardList cards, Pile dest)
    {
        foreach (CardData card in cards.Cards)
        {
            Debug.Log("-----\n Card should be " + card.CardName);
            GenerateCard(card, dest);
        }
    }
}
