using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : PlantComponentBase
{
    [SerializeField] private Transform m_StemAnchor;

    public Transform StemAnchor
    {
        get
        {
            return m_StemAnchor;
        }
    }
}
