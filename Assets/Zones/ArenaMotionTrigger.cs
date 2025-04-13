using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Zone))]
public class ArenaMotionTrigger : MonoBehaviour
{
    public Vector3 Offset;

	private bool m_hasControl = false;

	private void Update()
	{
		ZoneManager zm = FindAnyObjectByType<ZoneManager>();
		Debug.Assert(zm != null);
		Zone zone = GetComponent<Zone>();
		Debug.Assert(zone != null);

		if (zm.HoveredOverZone(zone) && !m_hasControl)
		{
			Set();
		}

		if (!zm.HoveredOverZone(zone) && m_hasControl)
		{
			Unset();
		}
	}

	private void Set()
    {
		ArenaMotion mot = FindAnyObjectByType<ArenaMotion>();
		Debug.Assert(mot != null);
		Debug.Log("Set my offset");
		mot.Set(Offset);
		m_hasControl = true;
	}

	private void Unset()
	{
		ArenaMotion mot = FindAnyObjectByType<ArenaMotion>();
		Debug.Assert(mot != null);
		Debug.Log("Remove my offset");
		mot.Unset();
		m_hasControl = false;
	}
}
