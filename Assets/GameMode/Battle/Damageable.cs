using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
	private int m_max;
	public int MaxLife
	{
		get
		{
			return m_max;
		}
		set
		{
			m_max = value;
			Life = m_max;
		}
	}

	public int Life {  get; private set; }

	public void Damage(int dmg)
	{
		Life -= dmg;
		if (Life < 0)
			DieToDamage();
	}

	public void ResetToMax()
	{
		Life = MaxLife;
	}

	virtual public void DieToDamage()
	{

	}
}
