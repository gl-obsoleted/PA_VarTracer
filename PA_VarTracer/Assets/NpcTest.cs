using UnityEngine;
using System.Collections;

public class NpcTest : MonoBehaviour {

    Vector3 m_velocity = new Vector3(0, 0, 0);
    int timer = 0;
    public Vector3 GetVelocity()
    {
        return m_velocity;
    }

	// Use this for initialization
	void Start () {
        Move();
	}
	
	// Update is called once per frame
	void Update () {
        timer++;
        if (timer ==180)
        {
            Move();
            timer = 0;
        }
        transform.Translate(m_velocity * Time.deltaTime);
	}

    void Move()
    {
        float speedx = UnityEngine.Random.value * 5f;
        if (UnityEngine.Random.value > 0.5f)
            m_velocity.x = speedx;
        else
            m_velocity.x = 0;

        float speedy = UnityEngine.Random.value * 5f;
        if (UnityEngine.Random.value > 0.5f)
            m_velocity.y = speedy;
        else
            m_velocity.y = 0;

        float speedz = UnityEngine.Random.value * 5f;
        if (UnityEngine.Random.value > 0.5f)
            m_velocity.z = speedz;
        else
            m_velocity.z = 0;
        VarTracerTool.SendEvent("NPC_MOVE");
    }
}
