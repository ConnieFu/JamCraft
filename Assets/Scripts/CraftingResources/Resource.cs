using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    protected const string Y_MOVEMENT = "Y_Movement";

    [SerializeField] protected Vector2 m_SpawnBounceYRange;
    [SerializeField] protected Vector2 m_BounceTime;

    protected Vector3 m_Velocity;
    protected float m_YHeight;

    protected float m_SleepThreshold = 0.0025f;
    protected float m_Gravity = -9.8f;
    protected float m_BounceCoEf = 0.6f;

    protected Vector3 m_CurrentPos;

    void Start()
    {
        Debug.LogError("I'm ALIVEEEE");

        m_Velocity = new Vector3(Random.Range(0.5f, 1.5f), Random.Range(3.0f, 8.0f), 0);
        m_YHeight = Random.Range(m_SpawnBounceYRange.x, m_SpawnBounceYRange.y);

        //Hashtable args = new Hashtable();
        //args.Add("onupdatetarget", gameObject);
        //args.Add("onupdate", )
        //args.Add("name", X_MOVEMENT);


        //iTween.ValueTo(gameObject, args);


    }

    protected void FixedUpdate()
    {
        if (m_Velocity.magnitude > m_SleepThreshold || transform.position.y > m_YHeight)
        {
            m_Velocity.y += m_Gravity * Time.fixedDeltaTime;
        }
        transform.position += m_Velocity * Time.fixedDeltaTime;

        m_CurrentPos = transform.localPosition;
        if (m_CurrentPos.y <= m_YHeight)
        {
            m_CurrentPos.y = m_YHeight;
            m_Velocity.y = -m_Velocity.y;
            m_Velocity *= m_BounceCoEf;
        }
    }
}
