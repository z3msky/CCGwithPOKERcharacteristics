using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	// returns excess damage
	public int Damage(int dmg, IDamageSource src = null);
}
