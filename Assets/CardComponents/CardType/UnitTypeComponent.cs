using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTypeComponent : CardTypeComponent, IDamageable, IStateBasedEvent, IDamageSource, ITurnResettable
{
	public int Power;
	public int Toughness;
	public int DamageOnUnit;
	public bool MarkedForDeath;
	public bool HasAdvRetThisTurn;
	public bool DeclaredAsAttacker = false;
	public Vector3 PreviewAdvanceDir = new Vector3(0, 40, 0);
	public Vector3 AdvanceRetreatPreviewOffset { get; private set; }
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
			float sign = Card.HasKeywordAbility(Keyword.Ranged) ? -0.7f : 1;
			AdvanceRetreatPreviewOffset = Vector3.Lerp(AdvanceRetreatPreviewOffset, sign * PreviewAdvanceDir, LerpSpeed * Time.deltaTime);
		}
		else
		{
			AdvanceRetreatPreviewOffset = Vector3.Lerp(AdvanceRetreatPreviewOffset, Vector3.zero, LerpSpeed * Time.deltaTime);
		}

		UpdatePT();
	}

	private void UpdatePT()
	{
		Card.GetComponent<CardVisual>().PowerToughnessText.text = Power + "/" + (Toughness - DamageOnUnit);
		if (DamageOnUnit > 0)
		{
			Card.GetComponent<CardVisual>().PowerToughnessText.color = Color.red;
		}
		else
		{
			Card.GetComponent<CardVisual>().PowerToughnessText.color = Color.black;
		}
	}

	public override void ActivateDesignElements(CardVisual cv)
	{
		cv.RankImage.gameObject.SetActive(true);
		cv.SuitImage.gameObject.SetActive(true);
		cv.PowerToughnessText.gameObject.SetActive(true);
		cv.CardNameText.gameObject.SetActive(true);

		UpdateAdvRetButtons(cv);
	}

	public void UpdateAdvRetButtons(CardVisual cv)
	{
		m_dealer = FindAnyObjectByType<Dealer>();
		m_battle = m_dealer.GameMode as BattleGameMode;
		Debug.Assert(m_dealer != null);
		Debug.Assert(m_battle != null);

		if (Card.IsInPlay
			&& m_battle.CurrentState != null
			&& m_battle.CurrentState is PlayerNeutralState
			&& !HasAdvRetThisTurn
			&& !Card.EnteredThisTurn
			&& Card.Controller == CharacterType.Player)
		{
			cv.AdvRetButtonPanel.gameObject.SetActive(true);

			AdvRetButton[] buttons = cv.AdvRetButtonPanel.GetComponentsInChildren<AdvRetButton>();
			foreach (AdvRetButton button in buttons)
			{
				button.UpdateInteractability(this);
			}
		}
		else
		{
			cv.AdvRetButtonPanel.gameObject.SetActive(false);
		}
	}

	public void QueueTryAdvanceOrRetreat(AdvRetType moveType = AdvRetType.ADVANCE)
	{
		Debug.Assert(m_dealer != null);
		Debug.Assert(m_battle != null);

		Zone zone = Card.CurrentZone;
		if (m_battle.PlayerBackRow.Subzones.Contains(zone))
		{
			int i = zone.transform.GetSiblingIndex();
			m_dealer.Queue(new TryAdvanceOrRetreatAction(moveType, this, 0.5f));
		}

		if (m_battle.PlayerFrontRow.Subzones.Contains(zone))
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
		m_battle.dealer.Queue(new CheckDeathsAction());
	}

	public bool CanAttack()
	{

		if (CurrentRow == m_dealer.Battle.PlayerFrontRow
			|| (CurrentRow == m_dealer.Battle.PlayerBackRow && Card.HasKeywordAbility(Keyword.Ranged)))
		{
			return true;
		}

		return false;
	}

	public bool CanAttackZone(Zone target)
	{
		Debug.Assert(m_dealer != null);
		Debug.Assert(m_battle != null);

		//Debug.Log("I am in " + Card.CurrentZone.ZoneName + " attacking " + target.ZoneName);
		Zone attackerZone = Card.CurrentZone;

		//// Summoning sickness
		//if (Card.EnteredThisTurn)
		//{
		//	return false;
		//}

		// I am a player unit
		if (m_battle.PlayerFrontRow.Subzones.Contains(attackerZone)
			|| (m_battle.PlayerBackRow.Subzones.Contains(attackerZone) && Card.HasKeywordAbility(Keyword.Ranged)))
		{
			return m_battle.EnemyRow.Subzones.Contains(target) ;
		}

		// Enemy can hit Player Front
		if (m_battle.EnemyRow.Subzones.Contains(attackerZone))
			return m_battle.PlayerFrontRow.Subzones.Contains(target);

		return false;
	}

	public bool CanAdvanceOrRetreat(AdvRetType moveType)
	{
		Zone z;
		return CanAdvanceOrRetreat(moveType, out z);
	}
	public bool CanAdvanceOrRetreat(AdvRetType moveType, out Zone targetZone)
	{
		Debug.Assert(m_dealer != null);
		Debug.Assert(m_battle != null);

		if (Card.EnteredThisTurn || HasAdvRetThisTurn)
		{
			targetZone = null;
			return false;
		}

		Zone currZone = Card.CurrentZone;
		int i = currZone.transform.GetSiblingIndex();
		Zone advZone = m_battle.PlayerFrontRow.Subzones[i];
		Zone retZone = m_battle.PlayerBackRow.Subzones[i];
		Zone enemyZone = m_battle.EnemyRow.Subzones[i];

		// Unit can advance: front to back
		if (moveType == AdvRetType.ADVANCE
			&& m_battle.PlayerBackRow.Subzones.Contains(currZone)
			&& advZone.Cards.Length == 0)
		{
			targetZone = advZone;
			return true;
		}


		// Unit can retreat: back to front
		if (moveType == AdvRetType.RETREAT
			&& m_battle.PlayerFrontRow.Subzones.Contains(currZone)
			&& retZone.Cards.Length == 0)
		{
			targetZone = retZone;
			return true;
		}

		// Unit can manifest: enemy intent to enemy row
		if (moveType == AdvRetType.ADVANCE
			&& m_battle.EnemyIntentRow.Subzones.Contains(currZone)
			&& enemyZone.Cards.Length == 0)
		{
			targetZone = enemyZone;
			return true;
		}

		targetZone = null;
		return false;
	}

	public int Damage(int dmg, IDamageSource src = null)
	{
		DamageOnUnit += dmg;
		Card.UpdateCardDisplay();
		Card.PlayAnimation("Shake");
		UpdatePT();

		if (src is UnitTypeComponent
			&& (src as UnitTypeComponent).Card.HasKeywordAbility(Keyword.Treacherous))
		{
			MarkedForDeath = true;
		}

		return DamageOnUnit - Toughness;
	}

	public bool ShouldDie()
	{
		return DamageOnUnit >= Toughness || MarkedForDeath;
	}

	public void CheckStateBasedEvents()
	{
	}

	public void ResetForTurn()
	{
		HasAdvRetThisTurn = false;
		Card.UpdateCardDisplay();
	}
}
