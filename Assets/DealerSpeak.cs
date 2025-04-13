using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DealerSpeak : MonoBehaviour
{
    public GameObject MyPanel;
    public TextMeshProUGUI MyText;

    public static DealerSpeak SceneInstance
    {
        get
        {
            DealerSpeak[] insts = FindObjectsByType<DealerSpeak>(FindObjectsSortMode.None);

            Debug.Assert(insts.Length > 0, "No DealerSpeak in scene!");
            Debug.Assert(insts.Length == 1, "More than 1 DealerSpeak in scene! " + insts.Length + " instances found!");

            return insts[0];
        }
    }

	void Start()
	{
		ClearDialogue();
	}

	public void SetDialogue(string msg, bool BlocksInput = false)
    {
        MyPanel.SetActive(true);
        MyText.text = msg;
    }

    public void ClearDialogue()
    {
        MyPanel.SetActive(false);
    }
}
