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

    void Start()
    {
    }

    void Update()
    {
        
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

    public void GenerateTestCard()
    {
        Debug.Log("Make test card");
        Debug.Assert(TestCardZone != null && MainCanvas != null);
        Card card = GameObject.Instantiate(EmptyCardObject, CardCanvas.transform).GetComponent<Card>();
        
        bool success = TestCardZone.AddCard(card);

        if (!success)
        {
            GameObject.Destroy(card.gameObject);
        }
    }
}
