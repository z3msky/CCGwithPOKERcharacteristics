using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public SoundLibrary Library;

    private AudioSource m_source;

	private void Start()
	{
		m_source = GetComponent<AudioSource>();
		Debug.Assert(m_source != null);
	}

	public void PlayPitched(AudioClip clip)
    {
		Debug.Assert(clip != null);
		Debug.Assert(m_source != null);

		Random.InitState((int)(Time.time * 1000f));
		float pitch = Random.Range(0.95f, 1.05f);
		m_source.pitch = pitch;
		m_source.PlayOneShot(clip);
	}
}
