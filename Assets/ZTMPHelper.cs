using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZTMPHelper
{
    public static string Bold(string text)
    {
        return "<b>" + text + "</b>";
    }

    public static string ListItems(List<string> list)
    {

        if (list.Count == 1)
            return list[0];

        if (list.Count == 2)
            return list[0] + " and " + list[1];

        if (list.Count >= 3)
        {
            // First
            string result = list[0];

            // All but first and last
            for (int i = 1; i < list.Count - 1; i++)
            {
                result += ", " + list[i];
            }

            // Last
            result += ", and " + list[list.Count - 1];

            return result;
        }

		return "";
	}
}
