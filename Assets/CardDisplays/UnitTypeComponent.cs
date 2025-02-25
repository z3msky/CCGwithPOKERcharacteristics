using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTypeComponent : CardTypeComponent
{
	public int Power;
	public int Toughness;
	public bool DeclaredAsAttacker = false;
	public Vector3 PreviewAdvanceDir = new Vector3(0, 40, 0);
	public Vector3 AdvanceRetreatPreviewOffset {  get; private set; }
	public float LerpSpeed = 10;

	private Dealer m_dealer;
	private BattleGameMode m_battle;

	void Start()
    {
		m_dealer = FindAnyObjectByType<Dealer>();
		m_battle = m_dealer.GameMode as BattleGameMode;
		Debug.Assert(m_dealer != null);
		Debug.Assert(m_battle != null);

		Power = Card.CardDataAsset.Power;
		Toughness = Card.CardDataAsset.Toughness;
        CardTypeOfComponent = CardType.UNIT;
		AdvanceRetreatPreviewOffset = Vector3.zero;
        //AnnounceAttachToCard();
	}

    void Update()
    {
		if (DeclaredAsAttacker)
		{
			AdvanceRetreatPreviewOffset = Vector3.Lerp(AdvanceRetreatPreviewOffset, PreviewAdvanceDir, LerpSpeed * Time.deltaTime);
		}
		else
		{
			AdvanceRetreatPreviewOffset = Vector3.Lerp(AdvanceRetreatPreviewOffset, Vector3.zero, LerpSpeed * Time.deltaTime);
		}
    }

	public override void ActivateDesignElements(Card card)
	{
		card.RankImage.enabled = true;
		card.SuitImage.enabled = true;
		card.PowerToughnessText.enabled = true;
		card.CardNameText.enabled = true;
	}

	public void QueueTryAdvanceOrRetreat(MoveType moveType = MoveType.ADVANCE)
	{
		Debug.Assert(m_dealer != null);
		Debug.Assert(m_battle != null);

		Zone zone = Card.CurrentZone;
		if (m_battle.PlayerBackRow.Subzones.Contains(zone))
		{
			int i = zone.transform.GetSiblingIndex();
			m_dealer.Queue(new TryAdvanceOrRetreatAction(moveType, this, 0.5f));
		}
	}

	public void QueueTryAttack()
	{
		Debug.Assert(m_dealer != null);
		Debug.Assert(m_battle != null);

		Zone attackerZone = Card.CurrentZone;
		int i = attackerZone.transform.GetSiblingIndex();
		m_battle.m_dealer.Queue(new AttackZoneAction(this, m_battle.EnemyRow.Subzones[i]));
	}

	public bool CanAttackZone(Zone target)
	{
		Debug.Assert(m_dealer != null);
		Debug.Assert(m_battle != null);

		Zone attackerZone = Card.CurrentZone;

		return m_battle.PlayerFrontRow.Subzones.Contains(attackerZone) && m_battle.EnemyRow.Subzones.Contains(target);
	}

	public bool CanAdvanceOrRetreat(MoveType moveType, out Zone targetZone)
	{
		Debug.Assert(m_dealer != null);
		Debug.Assert(m_battle != null);

		Zone currZone = Card.CurrentZone;
		int i = currZone.transform.GetSiblingIndex();
		Zone advZone = m_battle.PlayerFrontRow.Subzones[i];
		Zone retZone = m_battle.PlayerBackRow.Subzones[i];

		if (moveType == MoveType.ADVANCE
			&& m_battle.PlayerBackRow.Subzones.Contains(currZone)
			&& advZone.Cards.Length == 0)
		{
			targetZone = advZone;
			return true;
		}

		if (moveType == MoveType.RETREAT
			&& m_battle.PlayerFrontRow.Subzones.Contains(currZone)
			&& retZone.Cards.Length == 0)
		{
			targetZone = advZone;
			return true;
		}

		targetZone = null;
		return false;
	}

}
