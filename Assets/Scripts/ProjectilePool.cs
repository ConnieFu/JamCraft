using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
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
    }

    public void GetAvailableProjectile()
    {

    }
}
