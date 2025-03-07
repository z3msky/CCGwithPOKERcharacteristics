using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pile : Zone
{
    public Vector3 Spread = new Vector3(0.3f, 0.8f, 0);
    public Vector3 Margin = Vector3.zero;

    override protected void ZoneTypeUpdate()
    {
        ZoneLabeller label = GetComponentInChildren<ZoneLabeller>();
        if (label != null)
        {
            label.SetLabel(Cards.Length.ToString());
        }
    }

    override protected void ArrangeCards()
    {

        Vector3 currPos = transform.position + (Margin * CardCanvasRef.scaleFactor);

		foreach (Card card in Cards)
		{
			card.TargetPosition = currPos;
			currPos += Spread * CardCanvasRef.scaleFactor;
		}

	}

    public Card CardFromTop(int n = 0)
    {
        Debug.Assert(n < Cards.Length);
        Debug.Assert(n >= 0);
        int i = Cards.Length - 1 - n;

        return Cards[i];
    }
}
