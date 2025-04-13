using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(GameMode))]
[RequireComponent(typeof(ZoneManager))]
[RequireComponent(typeof(SFXManager))]
public class Dealer : MonoBehaviour
{
    public KeywordLibrary KeywordLibrary;
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
            if (m_queue.Count > 0)
            {
                return m_queue[0];
            }

            return null;
        }
    }

    // GO Refs
    public GameMode GameMode { get; private set; }
    public BattleGameMode Battle
    {
        get
        {
            Debug.Assert(GameMode is BattleGameMode, "Current game mode is not a battle");
            return GameMode as BattleGameMode;
        }
    }
    public ZoneManager ZoneManager { get; private set; }
    public SFXManager SFXManager { get; private set; }

    public bool DealerIsActive
    {
        get
        {
            return m_queue.Count > 0;
        }
    }

	private List<DealerAction> m_queue;

    void Start()
    {
        ZoneManager = GetComponent<ZoneManager>();
        GameMode = GetComponent<GameMode>();
        SFXManager = GetComponent<SFXManager>();

        m_queue = new List<DealerAction>();

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

		// If we completed a queued action
		if (CurrentAction.Complete)
        {
            m_queue.Remove(CurrentAction);
            if (m_queue.Count == 0)
            {
                OnQueueCompleted();
            }
        }
    }

    public void ClearAll()
    {
        if (m_queue.Count < 2)
        {
            return;
        }

        for (int i = 1; i < m_queue.Count; i++)
        {
            m_queue.RemoveAt(i);
        }
    }
    
    // Queueing actions blocks input until queue is done
    public void Queue(DealerAction action)
    {
        m_queue.Add(action);
    }

    // Immediate actions will resolve immediately after the current action, LIFO
    public void CutToNextInQueue(DealerAction action)
    {
        if (CurrentAction == null)
        {
            Queue(action);
        }
        else if (!CurrentAction.Started)
        {
            m_queue.Insert(0, action);
        }
        else
        {
            m_queue.Insert(1, action);
        }

    }

	// Dealer gets to update until queue is done
	// here we hand authority back to the game mode state machine
	void OnQueueCompleted()
	{
	}

	public void LerpMoveCardToZone(Card card, Zone dest)
    {
        card.transform.SetAsLastSibling();
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
            //Debug.Log("[" + card.CardDataAsset.CardName + "] stayed in zone [" + src.name + "]");
            return;
        }

        //Debug.Log("Move [" + card.CardDataAsset.CardName + "] from [" + src.ZoneName + "] -> [" + dest.ZoneName + "]");
        dest.AddCard(card);
    }

	public void InstantMoveCardToZone(Card card, Zone dest)
	{
        LerpMoveCardToZone(card, dest);
        card.ShouldTeleport = true;
	}

	public Card GenerateCard(CardData card, Zone dest, PlayerEnemyCharacter controller = null)
    {
		Card result = GameObject.Instantiate(EmptyCardObject, CardCanvas.transform).GetComponent<Card>();
        result.transform.position = dest.transform.position;
        result.DragToSetCardData = card;
        result.CardDataAsset = card;
		dest.AddCard(result);

        return result;
	}

    public void GenerateDeck(CardList cards, Pile dest, PlayerEnemyCharacter controller = null)
    {
        foreach (CardData card in cards.Cards)
        {
            GenerateCard(card, dest, controller);
        }
    }

	public void GenerateDefaultDeck(Pile dest, Suit suit, PlayerEnemyCharacter controller = null)
	{
        for (int rank = 1; rank <= 8; rank++)
        {
			CardData card = ScriptableObject.Instantiate(EmptyCard);

            card.CardName =  rank.ToString() + " of " + suit.ToString().ToLower();
            card.Rank = rank;
            card.Suit = suit;
            card.CardTypes = new CardType[1];
            card.CardTypes[0] = CardType.UNIT;
            card.Power = (rank/2)+2;
            card.Toughness = (rank / 2) + 1;

            List<KeywordAbilityData> keywords = new List<KeywordAbilityData>();
            if (Random.Range(0,5) == 1)
            {
                keywords.Add(KeywordLibrary.Find(Keyword.Ranged));
                card.Toughness /= 3;
            }
            
            if (Random.Range(0,6) == 1)
			{
				keywords.Add(KeywordLibrary.Find(Keyword.Treacherous));
                card.Power = 1;
                card.Toughness /= 2;
			}

            card.Toughness = Mathf.Clamp(card.Toughness, 1, 10);
            card.Power = Mathf.Clamp(card.Power, 1, 10);

			card.KeywordAbilities = keywords.ToArray();
            keywords = new List<KeywordAbilityData>();

			GenerateCard(card, dest);
		}
	}

	public RuntimeEnemyPlan GenerateDefaultPlan(Suit suit)
	{
        int ranknum = 10;
        EnemyMove[] moves = new EnemyMove[ranknum];

		for (int rank = 1; rank <= ranknum; rank++)
		{
			CardData card = ScriptableObject.Instantiate(EmptyCard);

			card.CardName = suit.ToString().ToLower() + " unit";
			card.Rank = rank;
			card.Suit = suit;
			card.CardTypes = new CardType[1];
			card.CardTypes[0] = CardType.UNIT;
			card.Power = (rank / 2) + 2;
			card.Toughness = (rank / 2) + 1;

            int turn;
			if (rank % 2 == 1)
            {
                turn = rank / 2;
            }
            else
            {
                turn = Random.Range(rank, 7);
                //turn = Random.Range(1, 1);
            }
            moves[rank - 1] = new EnemyMove(card, turn);
		}

        return new RuntimeEnemyPlan(moves);
	}

}
