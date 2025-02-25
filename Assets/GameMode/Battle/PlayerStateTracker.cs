using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateTracker : Damageable
{
    [Header("Refs")]
    public TextMeshProUGUI LifeCountText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        LifeCountText.text = Life.ToString();
    }
}
