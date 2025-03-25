using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AudioSettings;
using static UnityEngine.GraphicsBuffer;


public enum AdvRetType
{
    ADVANCE, RETREAT
}

public class TryAdvanceOrRetreatAction : DealerAction
{
    private AdvRetType m_type;
    private UnitTypeComponent m_unit;

    public TryAdvanceOrRetreatAction(AdvRetType type, UnitTypeComponent unit, float time = 0.3f)
        : base(time)
    {
        m_type = type;
		m_unit = unit;
    }

	override protected void SetupAction()
	{
		Debug.Assert(m_unit != null);

        Zone targetZone;

        if (m_unit.CanAdvanceOrRetreat(m_type, out targetZone))
        {
            m_dealer.SFXManager.PlayPitched(m_dealer.SFXManager.Library.AdvanceSound);
            m_dealer.LerpMoveCardToZone(m_unit.Card, targetZone);
            m_unit.HasAdvRetThisTurn = true;
        }

        m_unit.Card.UpdateCardDisplay();
	}

}
