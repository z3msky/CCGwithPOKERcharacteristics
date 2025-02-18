using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SplashImage : MonoBehaviour
{

    public float FadeTime = 1;
    public float MaxOpacity = 1;

    private float m_timer;

    // Start is called before the first frame update
    void Start()
    {
        m_timer = FadeTime;
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        float tNorm = 0;

        if (m_timer < FadeTime)
        {
            tNorm = 1 - m_timer / FadeTime;
        }

        Color c = GetComponent<Image>().color;
        c.a = tNorm * MaxOpacity;
        GetComponent<Image>().color = c;
    }

    public void ShowImage(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
        m_timer = 0;
    }
}
