using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneBorder : MonoBehaviour
{
    public Image[] Images;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color color)
    {
        foreach (var item in Images)
        {
            item.color = color;
        }
    }
}
