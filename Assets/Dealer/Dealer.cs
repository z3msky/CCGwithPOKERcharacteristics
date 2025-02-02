using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public GameObject EmptyCardObject;
    public Zone TestCardZone;
    public Canvas MainCanvas;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void MoveCardToZone(Card card, Zone dest)
    {
        Zone src = card.CurrentZone;
    }

    public void GenerateTestCard()
    {
        Debug.Log("Make test card");
        Debug.Assert(TestCardZone != null && MainCanvas != null);
        Card card = GameObject.Instantiate(EmptyCardObject, MainCanvas.transform).GetComponent<Card>();
        
        bool success = TestCardZone.AddCard(card);

        if (!success)
        {
            GameObject.Destroy(card.gameObject);
        }
    }
}
