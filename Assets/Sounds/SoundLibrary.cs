using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : ScriptableObject
{


	[Header("General")]
	public AudioClip SelectLow;
	public AudioClip RejectSound;
	public AudioClip DrawSound;
	public AudioClip CoinSound;

	[Header("Combat")]
	public AudioClip AdvanceSound;
	public AudioClip DefaultAttackSound;
	public AudioClip SelectForSummonSound;
	public AudioClip SummonSound;
}
