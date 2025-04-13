using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ZoneBorder : MonoBehaviour
{
    public Color DefaultColor;
    public Image[] Images;

    private Color m_oldDefault;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_oldDefault != DefaultColor)
        {
            SetColor(DefaultColor);
            m_oldDefault = DefaultColor;
        }
    }

	public void SetColor(Color color)
    {
        foreach (var item in Images)
        {
            item.color = color;
        }
    }
}
