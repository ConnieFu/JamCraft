using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    private const string PROJECTILE_PREFAB_PATH = "Prefabs/Projectiles/Projectile";
    private List<Projectile> m_AvailableProjectiles = new List<Projectile>();

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
}
