using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy m_Target;
    private float m_Damage;
    private GameConstants.eEnergyType m_DamageType;

    private bool m_IsInitialized = false;

    public void InitializeProjectile(Enemy target, float damage, GameConstants.eEnergyType damageType)
    {
        m_Damage = damage;
        m_DamageType = damageType;
        m_Target = target;

        m_IsInitialized = true;
    }

    private void Update()
    {
        if (!GameFlow.Instance.IsPaused && m_IsInitialized)
        {
            // update projectile position towards target
            Vector3 direction = (m_Target.transform.position - transform.position);
            if (direction.magnitude <= GameConstants.PROJECTILE_HIT_DISTANCE)
            {
                m_IsInitialized = false;
                m_Target.TakeDamage(m_Damage, m_DamageType);
                ProjectilePool.Instance.ReturnProjectile(this);
            }
            else
            {
                transform.position += direction.normalized * GameConstants.PROJECTILE_SPEED;
            }
        }
    }
}
