using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerEnemyCharacter : MonoBehaviour, IDamageable
{
    [Header("Refs")]
    public TextMeshProUGUI LifeCountText;

    public int Life { get; set; }

    private int m_max;
    public int MaxLife
    {
        get
        {
            return m_max;
        }

        set
        {
            m_max = value;
            Life = m_max;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        LifeCountText.text = Life.ToString();
    }

    public int Damage(int dmg, IDamageSource src = null)
    {
        Life -= dmg;

        if (Life <= 0)
        {
            BattleGameMode battle = FindAnyObjectByType<BattleGameMode>();
            Debug.Assert(battle != null);

            battle.PlayerLose(this);
        }
        return 0;
    }
}
