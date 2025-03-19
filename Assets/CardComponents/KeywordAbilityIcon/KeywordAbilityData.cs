using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Keyword
{
	NONE,
	Ranged,
	Evasive,
	Treacherous
}

[CreateAssetMenu(fileName = "New Keyword", menuName = "Keyword", order = 5)]
public class KeywordAbilityData : ScriptableObject
{
	public Keyword Keyword = Keyword.NONE;
	public string ExplainerText = "This is the default explainer text for a keyword";
	public Sprite IconSprite;
}
