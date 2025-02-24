using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayCardManager))]
[RequireComponent(typeof(ZoneManager))]
[RequireComponent(typeof(GameMode))]
public class Dealer : MonoBehaviour
{
    public GameObject EmptyCardObject;
    public Zone RootZone;
    public Canvas MainCanvas;
    public Canvas UICanvas;
    public Canvas CardCanvas;

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

    public ZoneManager ZoneManager { get; private set; }
    public GameMode GameMode { get; private set; }
    public bool DealerIsActive
    {
        get
        {
            return m_queue.Count > 0;
        }
    }

	private Queue<DealerAction> m_queue;

    void Start()
    {
        ZoneManager = GetComponent<ZoneManager>();
        GameMode = GetComponent<GameMode>();

        m_queue = new Queue<DealerAction>();

        Debug.Assert(GameMode != null);

        GameMode.GameSetup();
    }

    void Update()
    {
        // If dealer has nothing to do the game mode handles game flow
        if (CurrentAction == null)
        {
            GameMode.UpdateStateMachine();
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
            if (m_queue.Count == 0)
            {
                OnQueueCompleted();
            }
        }
    }

    
    // Queueing actions blocks input until queue is done
    public void Queue(DealerAction action)
    {
        m_queue.Enqueue(action);
        GameMode.SetDialogueReadout("Dealer is Acting");
    }

	// Dealer gets to update until queue is done
	// here we hand authority back to the game mode state machine
	void OnQueueCompleted()
	{
	}

	public void MoveCardToZone(Card card, Zone dest)
    {
        Zone src = card.CurrentZone;
        Debug.Assert(card != null);
        Debug.Assert(dest != null);
        Debug.Assert(src != null);
        if (card.CardDataAsset == null)
        {
            Debug.Log(card.gameObject.GetInstanceID());
        }
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
        result.CardDataAsset = card;
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
