using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemComponent : PlantComponentBase
{
    [SerializeField] private Transform m_FlowerAnchor;

    public Transform FlowerAnchor
    {
        get
        {
            return m_FlowerAnchor;
        }
    }
}
