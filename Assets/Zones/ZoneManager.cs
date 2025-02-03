using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ZoneManager : MonoBehaviour
{
	public TextMeshProUGUI debugtext;

	public Zone ZoneHoveredOver()
	{
		GraphicRaycaster rc = GetComponent<GraphicRaycaster>();
		EventSystem eventSystem = EventSystem.current;
		LayerMask oldMask = rc.blockingMask;
		LayerMask zoneOnlyMask = LayerMask.GetMask("Zones");
		PointerEventData eventData = new PointerEventData(eventSystem);
		eventData.position = Input.mousePosition;

		rc.blockingMask = zoneOnlyMask;
		List<RaycastResult> results = new List<RaycastResult>();
		rc.Raycast(eventData, results);
		rc.blockingMask = oldMask;

		foreach (var r in results)
		{
			if (r.gameObject.GetComponent<Zone>() != null)
			{
				if (debugtext != null)
				{
					debugtext.text = r.gameObject.GetComponent<Zone>().ZoneName;
				}

				return r.gameObject.GetComponent<Zone>();
			}
		}

		if (debugtext != null)
		{
			debugtext.text = "None";
		}

		return null;
	}
}
