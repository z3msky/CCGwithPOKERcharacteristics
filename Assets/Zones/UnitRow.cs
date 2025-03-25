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

    public List<UnitTypeComponent> Units
    {
        get
        {
            List<UnitTypeComponent> result = new List<UnitTypeComponent>();
            foreach (Zone z in Subzones)
            {

				if (!(z is FieldSlot))
					continue;

				FieldSlot slot = z as FieldSlot;

				if (!slot.HasCardInSlot)
					continue;

				if (!slot.CardInSlot.IsCardType(CardType.UNIT))
					continue;

				UnitTypeComponent unit = slot.CardInSlot.GetComponent<UnitTypeComponent>();
				Debug.Assert(unit != null);

				result.Add(unit);
			}

            Debug.Log(result.Count + "Units in row " + ZoneName);
            return result;
        }
    }

}
