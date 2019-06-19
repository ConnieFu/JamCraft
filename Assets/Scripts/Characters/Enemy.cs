using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    private const float ENEMY_IN_CENTER_OF_CELL_THRESHOLD = 0.25f;

    [Header("")]
    [SerializeField] private GameConstants.eEnergyType m_EnemyEnergyType;
    [SerializeField] private float m_MaxHealth = 15.0f;

    [SerializeField] private float m_AttackTime = 3.0f;

    private List<Vector3Int> m_Path = new List<Vector3Int>();
    private Vector3Int m_TargetMoveCell;

    private float m_CurrentAttackTime = 0.0f;
    private bool m_IsAttacking = false;
 
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
        m_CurrentTilePos = startCell;

        transform.position = GameFlow.Instance.GridManager.GetCellWorldPos(startCell);

        m_FacingDirection = Vector3Int.up;

        Initialize();
    }

    protected override void FixedUpdate()
    {
        if (!GameFlow.Instance.IsPaused && m_IsAlive)
        {
            base.FixedUpdate();

            UpdateFacingDirection();
            CheckForAttacking();

            if (!m_IsAttacking)
            {
                m_CharacterRigidBody.velocity = (Vector3)m_FacingDirection * m_Speed;
            }
            else
            {
                m_CharacterRigidBody.velocity = Vector3.zero;
            }
        }
    }

    protected override void UpdateSelectedTiles()
    {
        m_CurrentTilePos = GameFlow.Instance.GridManager.WorldPosToCell(transform.position);
    }

    private void UpdateFacingDirection()
    {
        m_Path = GameFlow.Instance.GridManager.FindPath(GameFlow.Instance.GridManager.WorldPosToCell(transform.position), GameConstants.TEMP_NODE_POSITON);

        if ((GameFlow.Instance.GridManager.GetCellWorldPos(m_CurrentTilePos) - transform.position).magnitude <= ENEMY_IN_CENTER_OF_CELL_THRESHOLD)
        {
            m_FacingDirection = m_Path[0] - m_CurrentTilePos;
        }
    }

    private void CheckForAttacking()
    {
        // reached the end, attack the nexus
        if (m_Path.Count <= 0)
        {
            KillEnemy();
        }

        // check if tile is empty in front of enemy. if tile has object, attack it
        if (!GameFlow.Instance.GridManager.IsCellEmpty(m_Path[0]))
        {
            m_IsAttacking = true;

            if (m_CurrentAttackTime <= 0.0f)
            {
                m_CurrentAttackTime = m_AttackTime;
                GameFlow.Instance.GridManager.InteractableTilemap.CharacterInteraction(m_Path[0], this);
            }
            m_CurrentAttackTime -= Time.deltaTime;
        }
        else if (m_IsAttacking)
        {
            m_IsAttacking = false;
        }
    }

    public void TakeDamage(float damage, GameConstants.eEnergyType damageType)
    {
        if (m_EnemyEnergyType != GameConstants.eEnergyType.DEFAULT && GameConstants.GetEnergyWeakness(m_EnemyEnergyType) == damageType)
        {
            damage *= GameConstants.ENERGY_WEAKNESS_DAMAGE_MODIFIER;
        }

        m_CurrentHealth -= damage;
        if (m_CurrentHealth <= 0.0f)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        m_CharacterRigidBody.velocity = Vector3.zero;

        m_IsAlive = false;
    }
}
