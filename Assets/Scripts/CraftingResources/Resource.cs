using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    protected const float COLLECTION_SPEED = 0.1f;
    protected const float COLLECTION_DISTANCE = 0.25f;

    [SerializeField] protected GameConstants.eResourceType m_ResourceType;

    [Header("Spawning")]
    [SerializeField] protected Vector2 m_SpawnXRange;
    [SerializeField] protected Vector2 m_SpawnYRange;

    protected Vector3 m_DistanceFromPlayer;

    protected bool m_IsSpawning = false;

    protected virtual void Spawn()
    {
        // TEMP: set position to random position around origin
        transform.position = new Vector3(   Random.Range(transform.position.x + m_SpawnXRange.x, transform.position.x + m_SpawnXRange.y),
                                            Random.Range(transform.position.y + m_SpawnYRange.x, transform.position.y + m_SpawnYRange.y),
                                            transform.position.z); 
    }

    void Start()
    {
        Spawn();
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (string.Compare(collision.tag, GameConstants.PLAYER_PICKUP_COLLIDER_TAG) == 0)
        {
            m_DistanceFromPlayer = (collision.transform.position - transform.position);

            if (m_DistanceFromPlayer.magnitude < COLLECTION_DISTANCE)
            {
                collision.GetComponent<CharacterPickUp>().AddResource(m_ResourceType);
                DestroySelf();
            }
            else
            {
                transform.position += m_DistanceFromPlayer * COLLECTION_SPEED;
            }
        }
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }
}


