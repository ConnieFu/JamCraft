using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    private const string PROJECTILE_PREFAB_PATH = "Prefabs/Projectiles/Projectile";
    private const string HIT_PARTICLES_PREFAB_PATH = "Prefabs/Projectiles/HitEnemyParticles";

    private List<Projectile> m_AvailableProjectiles = new List<Projectile>();

    [SerializeField] private Transform m_ParticlesAnchor;
    private List<ParticleSystem> m_ParticlesPool = new List<ParticleSystem>();

    private static ProjectilePool m_Instance;
    public static ProjectilePool Instance
    {
        get
        {
            return m_Instance;
        }
    }

    void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
        }

        m_AvailableProjectiles.AddRange(GetComponentsInChildren<Projectile>());
        m_ParticlesPool.AddRange(m_ParticlesAnchor.GetComponentsInChildren<ParticleSystem>());
    }

    public Projectile GetAvailableProjectile()
    {
        Projectile returnProjectile;

        if (m_AvailableProjectiles.Count > 0)
        {
            returnProjectile = m_AvailableProjectiles[0];
            m_AvailableProjectiles.Remove(returnProjectile);
        }
        else
        {
            returnProjectile = Instantiate<GameObject>((GameObject)Resources.Load(PROJECTILE_PREFAB_PATH), transform).GetComponent<Projectile>();
            returnProjectile.transform.localPosition = Vector3.zero;
        }

        return returnProjectile;
    }

    public void ReturnProjectile(Projectile projectile)
    {
        if (!m_AvailableProjectiles.Contains(projectile))
        {
            m_AvailableProjectiles.Add(projectile);
            projectile.transform.localPosition = Vector3.zero;
        }
    }

    public void OnProjectileHitTarget(Transform hitPos)
    {
        ParticleSystem hitParticles = null;
        foreach(ParticleSystem particle in m_ParticlesPool)
        {
            if (!particle.IsAlive())
            {
                hitParticles = particle;
            }
        }

        if (hitParticles == null)
        {
            hitParticles = Instantiate<GameObject>((GameObject)Resources.Load(HIT_PARTICLES_PREFAB_PATH), m_ParticlesAnchor).GetComponent<ParticleSystem>();
            m_ParticlesPool.Add(hitParticles);
        }

        hitParticles.transform.position = hitPos.position;
        hitParticles.Play();
    }
}
