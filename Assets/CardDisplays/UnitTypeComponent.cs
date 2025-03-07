using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTypeComponent : CardTypeComponent, IDamageable
{
	public int Power;
	public int Toughness;
	public int DamageOnUnit;
	public bool DeclaredAsAttacker = false;
	public Vector3 PreviewAdvanceDir = new Vector3(0, 40, 0);
	public Vector3 AdvanceRetreatPreviewOffset {  get; private set; }
	public float LerpSpeed = 10;

	public UnitRow CurrentRow
	{
		get
		{
			if (m_dealer.Battle.EnemyRow.Subzones.Contains(Card.CurrentZone))
				return m_dealer.Battle.EnemyRow;

			if (m_dealer.Battle.PlayerBackRow.Subzones.Contains(Card.CurrentZone))
				return m_dealer.Battle.PlayerBackRow;

			if (m_dealer.Battle.PlayerFrontRow.Subzones.Contains(Card.CurrentZone))
				return m_dealer.Battle.PlayerFrontRow;

			return null;
		}
	}

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
		DamageOnUnit = 0;
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

		Card.PowerToughnessText.text = Power + "/" + (Toughness - DamageOnUnit);
		if (DamageOnUnit > 0)
		{
			Card.PowerToughnessText.color = Color.red;
		}
		else
		{
			Card.PowerToughnessText.color = Color.black;
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

	public void QueueTryAttack(UnitRow row, string animName = "Attack")
	{
		Debug.Assert(m_dealer != null);
		Debug.Assert(m_battle != null);

		Zone attackerZone = Card.CurrentZone;
		int i = attackerZone.transform.GetSiblingIndex();

		m_battle.dealer.Queue(new AttackZoneAction(this, row.Subzones[i], animName));
	}

	public bool CanAttack()
	{
		if (Card.EnteredThisTurn)
			return false;

		Zone trash;
		if (CanAdvanceOrRetreat(MoveType.ADVANCE, out trash)
			|| CurrentRow == m_dealer.Battle.PlayerFrontRow
			|| CurrentRow == m_dealer.Battle.PlayerFrontRow)
		{
			return true;
		}

		return false;
	}

	public bool CanAttackZone(Zone target)
	{
		Debug.Assert(m_dealer != null);
		Debug.Assert(m_battle != null);

		Debug.Log("I am in " + Card.CurrentZone.ZoneName + " attacking " + target.ZoneName);
		Zone attackerZone = Card.CurrentZone;

		// Summoning sickness
		if (Card.EnteredThisTurn)
		{
			return false;
		}

		// Player Front can hit Enemy
		if (m_battle.PlayerFrontRow.Subzones.Contains(attackerZone))
		{
			return m_battle.EnemyRow.Subzones.Contains(target);
		}

		// Enemy can hit Player Front
		if (m_battle.EnemyRow.Subzones.Contains(attackerZone))
			return m_battle.PlayerFrontRow.Subzones.Contains(target);

		return false;
	}

	static private Zone trash;
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

	public int Damage(int dmg)
	{
		DamageOnUnit += dmg;
		if (DamageOnUnit >= Toughness)
		{
			Die();
		}
		return DamageOnUnit - Toughness;
	}

	public void Die()
	{
		Card.FloatingCard.gameObject.SetActive(false);
		m_dealer.PushImmediateAction(new MoveCardAction(Card, m_battle.DiscardZone));
	}
}
