using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerAction : ScriptableObject
{
	public bool Complete { get; private set; }

	[SerializeReference] public List<DealerAction> Subactions;
}
