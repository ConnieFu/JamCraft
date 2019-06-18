using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public struct sPlantData
    {
        public bool canAttack;
        public bool isAOE;
        public float damageAmt;
        public float attackRange;
        public float attackSpeed;
        public int health;
        public Transform attackPosition;
        public GameConstants.eEnergyType damageType; // if enemy is weak to damage type, double damage done to enemy
    }

    private PlantBase m_PlantBase;
    private sPlantData m_PlantInfo;

    private CharacterController m_EnteringEnemy = null;
    private CharacterController m_ExitingEnemy = null;
    private List<CharacterController> m_NearbyEnemies = new List<CharacterController>(); // CHANGE THIS TO ENEMIES LATER
    private CharacterController m_ClosestEnemy = null;

    private float m_AttackTimer = 0.0f;

    public void SetPlantInfo(sPlantData info, PlantBase plantBase)
    {
        m_PlantInfo = info;

        m_AttackTimer = m_PlantInfo.attackSpeed;
        GetComponent<CircleCollider2D>().radius = m_PlantInfo.attackRange;

        m_PlantBase = plantBase;
    }

    private void Update()
    {
        if (GameFlow.Instance.IsPaused || m_PlantBase == null || !m_PlantInfo.canAttack)
        {
            return;
        }

        if (!m_PlantBase.IsBeingHeld)
        {
            m_AttackTimer -= Time.deltaTime;
            if (m_AttackTimer <= 0.0f)
            {
                if (m_PlantInfo.isAOE)
                {
                    AttackNearbyEnemies();
                }
                else
                {
                    AttackNearestEnemy();
                }
                
                m_AttackTimer = m_PlantInfo.attackSpeed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_EnteringEnemy = collision.GetComponent<CharacterController>();
        if (m_EnteringEnemy != null)
        {
            m_NearbyEnemies.Add(m_EnteringEnemy);
            m_EnteringEnemy = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_ExitingEnemy = collision.GetComponent<CharacterController>();
        if (m_ExitingEnemy != null && m_NearbyEnemies.Contains(m_ExitingEnemy))
        {
            m_NearbyEnemies.Remove(m_ExitingEnemy);
            m_ExitingEnemy = null;
        }
    }

    private void AttackNearestEnemy()
    {
        float currentDistance;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < m_NearbyEnemies.Count; i++)
        {
            currentDistance = (m_NearbyEnemies[i].transform.position - transform.position).magnitude;
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                m_ClosestEnemy = m_NearbyEnemies[i];
            }
        }

        if (m_ClosestEnemy != null)
        {
            // shoot projectile or whatever at target (follows target)
            // target handles collision of projectile
            Projectile projectile = ProjectilePool.Instance.GetAvailableProjectile();
            projectile.transform.position = m_PlantInfo.attackPosition.position;
            projectile.InitializeProjectile(m_ClosestEnemy, m_PlantInfo.damageAmt, m_PlantInfo.damageType);

            m_ClosestEnemy = null;
        }
    }

    private void AttackNearbyEnemies()
    {
        // AOE DAMAGE HELL YEAH
    }
}
