using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTest : MonoBehaviour {

    Vector3 m_velocity = new Vector3(0, 0, 0);
    float m_speed;
    bool m_isUp=true;
    public Vector3 GetVelocity()
    {
        return m_velocity;
    }

    // Use this for initialization
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        transform.Translate(m_velocity * Time.deltaTime);
    }

    void Move()
    {
        if(m_isUp)
            m_speed+=0.1f;
        else
            m_speed-=0.1f;

        if(m_speed >=8)
        {
            m_isUp = false;
            VarTracerTool.SendEvent("SPEED_DOWN");
        }
        if (m_speed <= 0)
        {
            m_isUp = true;
            VarTracerTool.SendEvent("SPEED_UP");
        }

        m_velocity.x = m_speed;

        m_velocity.y = m_speed;
    }
}
