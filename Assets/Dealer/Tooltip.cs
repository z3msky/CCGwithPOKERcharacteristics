using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI TextDisplay;
    bool m_show = false;
    public bool ShowToolTip
    {
        get
        {
			return m_show;
        }

        set
        {
            m_show = value;
            gameObject.SetActive(m_show);
		}
    }

    public string Text
    {
        set
        {
            Debug.Assert(TextDisplay  != null);
            TextDisplay.text = value;
        }
    }
}
