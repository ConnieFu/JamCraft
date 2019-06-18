using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerComponent : PlantComponentBase
{
    [SerializeField] private GameConstants.eEnergyType m_DamageType = GameConstants.eEnergyType.DEFAULT;
    public GameConstants.eEnergyType DamageType
    {
        get
        {
            return m_DamageType;
        }
    }

    [SerializeField] private float m_AttackSpeed = 3.0f;
    public float AttackSpeed
    {
        get
        {
            return m_AttackSpeed;
        }
    }

    [Header("")]
    [SerializeField] private Transform m_AttackStartAnchor;
    public Transform AttackStartAnchor
    {
        get
        {
            return m_AttackStartAnchor;
        }
    }
}
