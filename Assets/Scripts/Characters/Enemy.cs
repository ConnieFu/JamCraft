using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    private const float ENEMY_IN_CENTER_OF_CELL_THRESHOLD = 0.25f;
    private const float START_ATTACK_TIME = 0.5f;

    [Header("")]
    [SerializeField] private GameConstants.eEnergyType m_EnemyEnergyType;
    [SerializeField] private float m_MaxHealth = 15.0f;

    [SerializeField] private float m_AttackTime = 3.0f;

    private List<Vector3Int> m_Path = new List<Vector3Int>();
    private Vector3 m_Velocity = Vector3.zero;
    private Vector3Int m_HomeBaseTarget;

    private float m_CurrentAttackTime = START_ATTACK_TIME;
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

    public delegate void OnEnemyDeath(Enemy enemy);
    public OnEnemyDeath m_OnDeath = null;

    public override void Reset()
    {
        StopMoving();
        m_OnDeath = null;
        m_IsAlive = false;
        transform.localPosition = Vector3.zero;
    }

    public void OnSpawned(Vector3Int startCell, Grid grid)
    {
        m_IsAlive = true;
        m_CurrentHealth = m_MaxHealth;
        m_CurrentTilePos = startCell;

        transform.position = GameFlow.Instance.GridManager.GetCellWorldPos(startCell);

        m_FacingDirection = Vector3Int.up;

        m_HomeBaseTarget = GameFlow.Instance.GridManager.InteractableTilemap.HomeBaseCell;

        m_Path = GameFlow.Instance.GridManager.FindPath(m_CurrentTilePos, m_HomeBaseTarget);
    }

    protected override void FixedUpdate()
    {
        StopMoving();

        if (!GameFlow.Instance.IsPaused && m_IsAlive)
        {
            base.FixedUpdate();

            UpdateEnemyMovement();
        }
    }

    protected override void UpdateSelectedTiles()
    {
        m_CurrentTilePos = GameFlow.Instance.GridManager.WorldPosToCell(transform.position);
    }

    private void UpdateEnemyMovement()
    {
        if (!m_IsAttacking)
        {
            m_Path = GameFlow.Instance.GridManager.FindPath(m_CurrentTilePos, m_HomeBaseTarget);
        }
        if ((GameFlow.Instance.GridManager.GetCellWorldPos(m_CurrentTilePos) - transform.position).magnitude <= ENEMY_IN_CENTER_OF_CELL_THRESHOLD)
        {
            m_FacingDirection = m_Path[0] - m_CurrentTilePos;

            if (m_FacingDirection == Vector3Int.up)
            {
                m_Animator.SetInteger("Direction", 1);
            }
            else if (m_FacingDirection == Vector3Int.down)
            {
                m_Animator.SetInteger("Direction", 0);
            }
            else if (m_FacingDirection == Vector3Int.right)
            {
                m_Animator.SetInteger("Direction", 2);
                m_SpriteRenderer.flipX = false;
            }
            else if (m_FacingDirection == Vector3Int.left)
            {
                m_Animator.SetInteger("Direction", 2);
                m_SpriteRenderer.flipX = true;
            }

            // check if tile is empty in front of enemy. if tile has object, attack it
            // don't move the character anymore
            if (!GameFlow.Instance.GridManager.IsCellEmpty(m_Path[0]))
            {
                m_IsAttacking = true;

                if (!m_IsAttacking)
                {
                    m_CurrentAttackTime = START_ATTACK_TIME;
                }
            }
            else
            {
                m_IsAttacking = false;
            }
        }

        if (m_IsAttacking)
        {
            StopMoving();
            m_Velocity = Vector3.zero;

            m_CurrentAttackTime -= Time.deltaTime;
            if (m_CurrentAttackTime <= 0.0f)
            {
                m_CurrentAttackTime = m_AttackTime;
                GameFlow.Instance.GridManager.InteractableTilemap.EnemyInteraction(m_Path[0]);
                // if it's the home base, get rid of the enemy after one attack
                if (m_Path[0] == m_HomeBaseTarget)
                {
                    OnEnemyTargetReached();
                    return;
                }
                m_Path = GameFlow.Instance.GridManager.FindPath(m_CurrentTilePos, m_HomeBaseTarget);
            }
        }
        else
        {
            m_Velocity = (Vector3)m_FacingDirection * m_Speed;
            m_Animator.SetBool("IsMoving", true);
        }
        m_CharacterRigidBody.velocity = m_Velocity;

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
        if (m_OnDeath != null)
        {
            m_OnDeath(this);
        }
        Reset();
    }

    private void OnEnemyTargetReached()
    {
        Reset();
    }

    private void StopMoving()
    {
        m_CharacterRigidBody.velocity = Vector3.zero;
        m_Animator.SetBool("IsMoving", false);
    }
}
