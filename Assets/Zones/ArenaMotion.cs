using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaMotion : MonoBehaviour
{
    Vector3 m_default;
    Vector3 m_target;

    // Start is called before the first frame update
    void Start()
    {
        m_default = transform.position;
        m_target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, m_target, Time.deltaTime * 10);
    }

    public void Set (Vector3 offset)
    {
        m_target = m_default + offset;
    }

    public void Unset()
    {
        m_target = m_default;
    }
}
