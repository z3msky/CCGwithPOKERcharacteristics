using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile : Zone
{
    public Vector3 Spread = new Vector3(0.3f, 0.8f, 0);
    public Vector3 FirstCardOffset = Vector3.zero;

    void Start()
    {
        
    }

    override protected void ArrangeCards()
    {
        Vector3 currPos = transform.position + FirstCardOffset;

        foreach (Card card in Cards)
        {
            card.TargetPosition = currPos;
            currPos += Spread;
        }
    }
}
