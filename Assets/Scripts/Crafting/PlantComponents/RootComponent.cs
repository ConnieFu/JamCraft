using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootComponent : PlantComponentBase
{
    [SerializeField] private float m_DamageModifier = 1;
    public float DamageModifier
    {
        get
        {
            return m_DamageModifier;
        }
    }

    [SerializeField] private int m_HealthModifier = 1;
    public int HealthModifier
    {
        get
        {
            return m_HealthModifier;
        }
    }

    [SerializeField] private float m_AttackSpeedModifier = 1.0f;
    public float AttackSpeedModifier
    {
        get
        {
            return m_AttackSpeedModifier;
        }
    }

    [Header("")]
    [SerializeField] private Transform m_StemAnchor;

    public Transform StemAnchor
    {
        get
        {
            return m_StemAnchor;
        }
    }
}
