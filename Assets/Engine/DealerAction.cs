using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerAction
{

	public float HoldTime = 1;

	public bool Complete { get; private set; }

	virtual public void Setup()
	{
		
	}

	virtual public void Process()
	{
		Complete = true;
	}
}
