using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private CharacterController m_Target;
    private float m_Damage;
    private GameConstants.eEnergyType m_DamageType;

    private bool m_IsInitialized = false;

    public void InitializeProjectile(CharacterController target, float damage, GameConstants.eEnergyType damageType)
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
                ProjectilePool.Instance.ReturnProjectile(this);
            }
            else
            {
                transform.position += direction.normalized * GameConstants.PROJECTILE_SPEED;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterController>() != null && collision.GetComponent<CharacterController>() == m_Target)
        {
            // deal damage to target

            
        }
    }
}
