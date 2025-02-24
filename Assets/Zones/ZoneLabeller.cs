using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZoneLabeller : MonoBehaviour
{
    public TextMeshProUGUI LabelText;

    public void SetLabel(string label)
    {
        Debug.Assert(LabelText != null, gameObject.name + " has no label text ref");

        LabelText.text = label;
    }
}
