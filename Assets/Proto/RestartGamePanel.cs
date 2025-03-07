using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RestartGamePanel : MonoBehaviour
{
    public TextMeshProUGUI StatusText;

	public void SetText(string text)
	{
		StatusText.text = text;
	}

	public void Reset()
	{
		string name = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(name);
	}
}
