using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dealer))]
public class PlayCardManager : MonoBehaviour
{
    public Dealer DealerRef;

    // Start is called before the first frame update
    void Start()
    {
        DealerRef = GetComponent<Dealer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestPlayabilityWhileHovered(Card card)
    {

    }
}
