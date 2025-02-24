using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ZoneManager : MonoBehaviour
{
	public TextMeshProUGUI debugtext;
	public GraphicRaycaster Raycaster;

	public Zone ZoneHoveredOver()
	{
		Debug.Assert(Raycaster != null);

		EventSystem evSys = EventSystem.current;
		LayerMask oldMask = Raycaster.blockingMask;
		LayerMask zoneOnlyMask = LayerMask.GetMask("Zones");
		PointerEventData eventData = new PointerEventData(evSys);
		eventData.position = Input.mousePosition;

		Raycaster.blockingMask = zoneOnlyMask;
		List<RaycastResult> results = new List<RaycastResult>();
		Raycaster.Raycast(eventData, results);
		Raycaster.blockingMask = oldMask;

		foreach (var r in results)
		{
			if (r.gameObject.GetComponent<Zone>() != null)
			{
				ZoneBorder border = r.gameObject.GetComponentInChildren<ZoneBorder>();
				return r.gameObject.GetComponent<Zone>();
			}
		}

		return null;
	}

}
