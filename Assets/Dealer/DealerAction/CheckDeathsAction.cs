using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckDeathsAction : DealerAction
{

	List<UnitTypeComponent> m_units;

	override public bool UseTimer
	{
		get { return true; }
	}
	override public bool UpdatesOnFirstFrame
	{
		get { return false; }
	}

	override protected void SetupAction()
	{
		List<UnitTypeComponent> units = new List<UnitTypeComponent>();

		TestRow(m_dealer.Battle.PlayerBackRow, ref units);
		TestRow(m_dealer.Battle.PlayerFrontRow, ref units);
		TestRow(m_dealer.Battle.EnemyRow, ref units);

		if (units.Count < 1)
		{
			Complete = true;
		}
		Debug.Log(units.Count + " units in total should die");
	}

	void TestRow(UnitRow row, ref List<UnitTypeComponent> units)
	{
		foreach (UnitTypeComponent unit in row.Units)
		{
			if (unit.ShouldDie())
			{
				units.Add(unit);
				Debug.Log(unit.Card.Rank + " of " + unit.Card.Suit.ToString() + " should die");
				unit.DamageOnUnit = 0;
				unit.MarkedForDeath = false;
				m_dealer.CutToNextInQueue(new MoveCardAction(unit.Card, m_dealer.Battle.DiscardZone, 0.3f, true));
			}
		}
	}

	override protected void ProcessAction()
	{
	}
}
