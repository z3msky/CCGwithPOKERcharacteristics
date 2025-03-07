using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMove
{
	public EnemyMove(EnemyMove move)
	{
		Turn = move.Turn;
		CardDataAsset = move.CardDataAsset;
		Complete = false;
	}

	public EnemyMove(CardData cardData, int turn)
	{
		Turn = turn;
		CardDataAsset = cardData;
		Complete = false;
	}

	public int Turn;
	[SerializeReference]
	public CardData CardDataAsset;
	public bool Complete = false;
}

[CreateAssetMenu(fileName = "New Enemy Play Data", menuName = "Create Enemy Play Data", order = 1)]
public class EnemyPlanData : ScriptableObject
{
	public EnemyMove[] Moves;
}

public class RuntimeEnemyPlan
{
	public EnemyMove[] Moves;

	public RuntimeEnemyPlan(EnemyPlanData data)
	{
		Moves = new EnemyMove[data.Moves.Length];
		for (int i = 0; i < Moves.Length; i++)
		{
			Moves[i] = new EnemyMove(data.Moves[i]);
			Moves[i].Complete = false;
		}
	}

	public RuntimeEnemyPlan(EnemyMove[] moves)
	{
		Moves = moves;
	}
}
