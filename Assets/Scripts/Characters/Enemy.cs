using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    [Header("")]
    [SerializeField] private GameConstants.eEnergyType m_EnemyEnergyType;
    [SerializeField] private float m_MaxHealth = 30.0f;
 
    private bool m_IsAlive = false;
    public bool IsAlive
    {
        get
        {
            return m_IsAlive;
        }
    }

    private float m_CurrentHealth;

    public void OnSpawned(Vector3Int startCell, Grid grid)
    {
        m_IsAlive = true;
        m_CurrentHealth = m_MaxHealth;

        transform.position = GameFlow.Instance.GridManager.GetCellWorldPos(startCell);

        m_FacingDirection = Vector3Int.up;

        Initialize();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!GameFlow.Instance.IsPaused && m_IsAlive)
        {
            
            m_CharacterRigidBody.velocity = GetNextDirectionTowardsNode() * m_Speed;
        }
    }

    private Vector3 GetNextDirectionTowardsNode()
    {
        // check if tile is empty in front of enemy
        // if tile has object,
        // 
        return Vector3.zero;
    }

    public void TakeDamage(float damage, GameConstants.eEnergyType damageType)
    {

    }
}
