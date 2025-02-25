using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AudioSettings;
using static UnityEngine.GraphicsBuffer;


public enum MoveType
{
    ADVANCE, RETREAT
}

public class TryAdvanceOrRetreatAction : DealerAction
{
    private MoveType m_type;
    private UnitTypeComponent m_unit;

    public TryAdvanceOrRetreatAction(MoveType type, UnitTypeComponent unit, float time = 0.3f)
        : base(time)
    {
        m_type = type;
		m_unit = unit;
    }

	override protected void SetupAction()
	{
		Debug.Assert(m_unit != null);

        Zone targetZone;
        if (m_unit.CanAdvanceOrRetreat(MoveType.ADVANCE, out targetZone))
        {
            m_dealer.SFXManager.PlayPitched(m_dealer.SFXManager.Library.AdvanceSound);
            m_dealer.InstantMoveCardToZone(m_unit.Card, targetZone);
        }
	}

}
