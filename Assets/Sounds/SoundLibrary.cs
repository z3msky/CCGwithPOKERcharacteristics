using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : ScriptableObject
{


	[Header("Menu")]
	public AudioClip SelectLow;
	public AudioClip RejectSound;
	[Header("Combat")]
	public AudioClip AdvanceSound;
	public AudioClip DefaultAttackSound;
	public AudioClip SelectForSummonSound;
	public AudioClip SummonSound;
}
