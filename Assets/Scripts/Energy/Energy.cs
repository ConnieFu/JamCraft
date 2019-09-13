using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour, ITouchable
{
    [SerializeField] protected GameConstants.eEnergyType m_EnergyType;

    [Header("Spawning")]
    [SerializeField] protected Vector2 m_SpawnXRange;
    [SerializeField] protected Vector2 m_SpawnYRange;

    [Header("Animations")]
    [SerializeField] protected Animator m_EnergyAnimation;
    [SerializeField] protected string m_EnergyAnimName;
    [SerializeField] protected Animator m_ShadowAnimation;
    [SerializeField] protected string m_ShadowAnimName;

    protected bool m_IsSpawning = false;

    protected virtual void Spawn()
    {
        // TEMP: set position to random position around origin
        transform.position = new Vector3(   Random.Range(transform.position.x + m_SpawnXRange.x, transform.position.x + m_SpawnXRange.y),
                                            Random.Range(transform.position.y + m_SpawnYRange.x, transform.position.y + m_SpawnYRange.y),
                                            transform.position.z);

        PlayIdleAnimation();
    }

    void Start()
    {
        Spawn();
    }

    protected virtual void PlayIdleAnimation()
    {
        m_EnergyAnimation.Play(m_EnergyAnimName);
        m_ShadowAnimation.Play(m_ShadowAnimName);
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnTouchBegin()
    {
        GameFlow.Instance.EnergyManager.AddEnergy(m_EnergyType);
        DestroySelf();
    }

    public void OnTouchTapped()
    {
    }

    public void OnTouchHold(Vector2 position)
    {
    }

    public void OnTouchEnd(Vector2 position, bool wasTapped)
    {
    }
}


