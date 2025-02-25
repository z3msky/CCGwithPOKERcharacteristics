using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DealerAction
{
	public UnityEvent OnStartAction;
	public AudioClip StartActionSound = null;

	public float HoldTime {  get; private set; }
	public bool Started { get; private set; }
	public bool Complete { get; protected set; }
	public bool HoldTimeComplete
	{
		get
		{
			return m_timer >= HoldTime;
		}
	}
	virtual public bool UseTimer
	{
		get { return true; }
	}

	virtual public bool UpdatesOnFirstFrame
	{
		get { return true; }
	}

	private float m_timer;
	protected Dealer m_dealer;

	public DealerAction(float holdTime = 1)
	{
		HoldTime = holdTime;
		Started = false;
		Complete = false;
	}

	public void Setup(Dealer dealer)
	{
		m_dealer = dealer;
		m_timer = 0;
		SetupAction();
		if (StartActionSound != null)
		{
			m_dealer.SFXManager.PlayPitched(StartActionSound);
		}
		Started = true;
	}

	public void Process()
	{
		m_timer += Time.deltaTime;

		ProcessAction();

		if (UseTimer)
			Complete = HoldTimeComplete;
	}

	virtual protected void SetupAction()
	{
		
	}

	virtual protected void ProcessAction()
	{
		Complete = true;
	}

	
}
