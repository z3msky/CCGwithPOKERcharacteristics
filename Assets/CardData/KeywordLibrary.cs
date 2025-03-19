using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Keyword", menuName = "Singletons/KeywordLib", order = 5)]
public class KeywordLibrary : ScriptableObject
{
	[SerializeReference]
	public KeywordAbilityData[] Library;

	public KeywordAbilityData Find(Keyword keyword)
	{
			//Debug.Log("Keyword find single");
		foreach (KeywordAbilityData item in Library)
		{
			//Debug.Log("Keyword " +  name);
			if (item.Keyword == keyword)
				return item;
		}

		Debug.Assert(false, "Keyword " +  keyword.ToString() + " is not in library");
		return null;
	}
}
