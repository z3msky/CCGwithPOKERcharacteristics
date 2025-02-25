using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(GameMode))]
[RequireComponent(typeof(ZoneManager))]
[RequireComponent(typeof(SFXManager))]
public class Dealer : MonoBehaviour
{
    public GameObject EmptyCardObject;
    public Zone RootZone;
    public Canvas MainCanvas;
    public Canvas UICanvas;
    public Canvas DecorationCanvas;
    public Canvas CardCanvas;
    public CardData EmptyCard;

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

    // GO Refs
    public GameMode GameMode { get; private set; }
    public ZoneManager ZoneManager { get; private set; }
    public SFXManager SFXManager { get; private set; }

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
        SFXManager = GetComponent<SFXManager>();

        m_queue = new Queue<DealerAction>();

        GameMode.GameSetup();
    }

    void Update()
    {
        // If dealer has nothing to do the game mode handles game flow
        if (CurrentAction == null)
        {
            GameMode.UpdateGameMode();
            return;
        }

        // Setup if it's a new action
        if (!CurrentAction.Started)
        {
            CurrentAction.Setup(this);
            if (CurrentAction.UpdatesOnFirstFrame)
			    CurrentAction.Process();
		}
        // Or simply process the current action
        else
        {
            CurrentAction.Process();
        }

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
    }

	// Dealer gets to update until queue is done
	// here we hand authority back to the game mode state machine
	void OnQueueCompleted()
	{
	}

	public void InstantMoveCardToZone(Card card, Zone dest)
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

	public void GenerateDefaultDeck(Pile dest, Suit suit)
	{
        for (int rank = 1; rank <= 13; rank++)
        {
			CardData card = ScriptableObject.Instantiate(EmptyCard);

               card.CardName = suit.ToString().ToLower() + " unit";
               card.Rank = rank;
               card.Suit = suit;
               card.CardTypes = new CardType[1];
               card.CardTypes[0] = CardType.UNIT;
               card.Power = rank;
               card.Toughness = rank;

			GenerateCard(card, dest);
		}
	}

}
