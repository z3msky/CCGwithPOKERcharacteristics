using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ZoneManager : MonoBehaviour
{
	public TextMeshProUGUI debugtext;
	public GraphicRaycaster Raycaster;

	private float m_lastUpdateTime = -1;
	private List<Zone> m_lastUpdateZones;

	public Zone CurrentZoneHoveredOver()
	{
		List<Zone> list = CurrentZonesHoveredOver();

		if (list.Count == 0)
			return null;

		if (debugtext != null)
			debugtext.text = list[0].ZoneName;

		return list[0];
	}

	public bool HoveredOverZone(Zone zone)
	{
		return CurrentZonesHoveredOver().Contains(zone);
	}


	public List<Zone> CurrentZonesHoveredOver()
	{
		Debug.Assert(Raycaster != null);

		if (Time.time == m_lastUpdateTime)
			return m_lastUpdateZones;

		m_lastUpdateTime = Time.time;

		EventSystem evSys = EventSystem.current;
		LayerMask oldMask = Raycaster.blockingMask;
		LayerMask zoneOnlyMask = LayerMask.GetMask("Zones");
		PointerEventData eventData = new PointerEventData(evSys);
		eventData.position = Input.mousePosition;

		Raycaster.blockingMask = zoneOnlyMask;
		List<RaycastResult> results = new List<RaycastResult>();
		Raycaster.Raycast(eventData, results);
		Raycaster.blockingMask = oldMask;

		m_lastUpdateZones = new List<Zone>();

		foreach (var r in results)
		{
			if (r.gameObject.GetComponent<Zone>() != null)
			{
				ZoneBorder border = r.gameObject.GetComponentInChildren<ZoneBorder>();
				m_lastUpdateZones.Add(r.gameObject.GetComponent<Zone>());
			}
		}

		return m_lastUpdateZones;
	}
}
