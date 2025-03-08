using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IStateBasedEvent
{

	public static void TestAll()
	{
		foreach (IStateBasedEvent ev 
			in GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
			.OfType<IStateBasedEvent>())
		{
			ev.CheckStateBasedEvents();
		}
	}

	public void CheckStateBasedEvents();
}
