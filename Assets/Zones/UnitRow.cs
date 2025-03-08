using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRow : Zone
{
	override protected void ZoneTypeStart()
	{
        for (int i = 0; i < Subzones.Count; i++)
        {
            Zone z = Subzones[i];
            z.ZoneName = ZoneName + "-" + i.ToString();
        }
	}

	override public bool CanAcceptAsTrace(Card card)
	{
        bool result = false;

        foreach (Zone z in Subzones)
        {
            FieldSlot us = z as FieldSlot;
            if (us != null)
            {
                result = result || us.CanAcceptAsTrace(card);
            }
        }

        return result;
	}

}
