using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerAction
{

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
		Started = true;
	}

	public void Process()
	{
		m_timer += Time.deltaTime;

		ProcessAction();

		if (HoldTimeComplete)
			Complete = true;
	}

	virtual protected void SetupAction()
	{
		
	}

	virtual protected void ProcessAction()
	{
		Complete = true;
	}
}
