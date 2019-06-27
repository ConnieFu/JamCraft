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

    private Enemy m_EnteringEnemy = null;
    private List<Enemy> m_NearbyEnemies = new List<Enemy>();
    private Enemy m_ClosestEnemy = null;

    private float m_StartAttackTime;
    private float m_AttackTimer = 0.0f;

    public void SetPlantInfo(sPlantData info, PlantBase plantBase)
    {
        m_PlantInfo = info;

        m_StartAttackTime = 5f / m_PlantInfo.attackSpeed;
        m_AttackTimer = m_StartAttackTime;
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
            m_PlantBase.FlowerComponent.transform.localScale = Vector3.one * (m_StartAttackTime - m_AttackTimer) / m_StartAttackTime;
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
                
                m_AttackTimer = m_StartAttackTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_EnteringEnemy = collision.GetComponent<Enemy>();
        if (m_EnteringEnemy != null)
        {
            m_NearbyEnemies.Add(m_EnteringEnemy);
            m_EnteringEnemy.m_OnDeath = RemoveEnemy;
            m_EnteringEnemy = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveEnemy(collision.GetComponent<Enemy>());
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

        ShootProjectileAtEnemy(m_ClosestEnemy);
        m_ClosestEnemy = null;
    }

    private void AttackNearbyEnemies()
    {
        // AOE DAMAGE HELL YEAH
        foreach(Enemy enemy in m_NearbyEnemies)
        {
            if (enemy.IsAlive)
            {
                // deal damage to the enemy
                ShootProjectileAtEnemy(enemy);
            }  
        }
    }

    private void ShootProjectileAtEnemy(Enemy enemy)
    {
        if (enemy != null && enemy.IsAlive)
        {
            // for now, used iTween to handle shoot animation


            // shoot projectile or whatever at target (follows target)
            // target handles collision of projectile
            Projectile projectile = ProjectilePool.Instance.GetAvailableProjectile();
            projectile.transform.position = m_PlantInfo.attackPosition.position;
            projectile.InitializeProjectile(enemy, m_PlantInfo.damageAmt, m_PlantInfo.damageType, ProjectilePool.Instance.OnProjectileHitTarget);
        }
    }

    private void RemoveEnemy(Enemy enemy)
    {
        if (enemy != null && m_NearbyEnemies.Contains(enemy))
        {
            m_NearbyEnemies.Remove(enemy);
        }
    }
}
