using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public GameObject EmptyCardObject;
    public Zone RootZone;
    public Canvas MainCanvas;
    public Canvas CardCanvas;
    public CardList TestDeck;
    public Pile TestDeckPile;

    public DealerAction CurrentAction
    {
        get
        {
            DealerAction val;
            bool success = m_queue.TryPeek(out val);

            if (success)
                return val;

            return null;
        }
    }

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
        // If dealer has nothing to do the game mode handles game flow
        if (CurrentAction == null)
        {
            m_gameMode.WhileQueueEmpty();
            return;
        }

        // Setup if it's a new action
        if (!CurrentAction.Started)
        {
            CurrentAction.Setup(this);
        }

        // Dealing with current action
        CurrentAction.Process();

        // Remove it if it's done
        if (CurrentAction.Complete)
        {
            m_queue.Dequeue();
        }
    }

    public void Queue(DealerAction action)
    {
        m_queue.Enqueue(action);
        //Debug.Log("Queued dealer action");
    }

    public void MoveCardToZone(Card card, Zone dest)
    {
        Zone src = card.CurrentZone;
        Debug.Assert(card != null);
        Debug.Assert(dest != null);
        Debug.Assert(src != null);
        Debug.Assert(card.CardDataAsset != null);

        if (src == dest)
        {
            Debug.Log("[" + card.CardDataAsset.CardName + "] stayed in zone [" + src.name + "]");
            return;
        }

        if (dest.ZoneName == "PlayerHand")
        {
            SplashImage img = FindAnyObjectByType<SplashImage>();
            if (img != null && card.CardDataAsset.CardArt != null)
            {
                img.ShowImage(card.CardDataAsset.CardArt);
            }
        }

        Debug.Log("Move [" + card.CardDataAsset.CardName + "] from [" + src.ZoneName + "] -> [" + dest.ZoneName + "]");
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
            GenerateCard(card, dest);
        }
    }
}
