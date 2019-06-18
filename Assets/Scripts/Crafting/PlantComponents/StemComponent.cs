using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemComponent : PlantComponentBase
{
    [SerializeField] private bool m_CanAttack = true;
    public bool CanAttack
    {
        get
        {
            return m_CanAttack;
        }
    }

    [SerializeField] private bool m_IsAOE = false;
    public bool IsAOE
    {
        get
        {
            return m_IsAOE;
        }
    }

    [SerializeField] private float m_AttackRange = 5.0f;
    public float AttackRange
    {
        get
        {
            return m_AttackRange;
        }
    }

    [Header("")]
    [SerializeField] private Transform m_FlowerAnchor;

    public Transform FlowerAnchor
    {
        get
        {
            return m_FlowerAnchor;
        }
    }
}
