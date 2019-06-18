using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantComponentBase : MonoBehaviour
{
    [SerializeField] private float m_DamageAmount = 1;
    public float DamageAmount
    {
        get
        {
            return m_DamageAmount;
        }
    }

    [SerializeField] private int m_HealthAmount = 1;
    public int HealthAmount
    {
        get
        {
            return m_HealthAmount;
        }
    }

    private List<SpriteRenderer> m_Renderers = null;
    public void ChangeRenderingLayerInfo(string layerName, int sortingOrder)
    {
        if (m_Renderers == null)
        {
            m_Renderers = new List<SpriteRenderer>();
            m_Renderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
        }

        for (int i = 0; i < m_Renderers.Count; i++)
        {
            m_Renderers[i].sortingLayerName = layerName;
            m_Renderers[i].sortingOrder = sortingOrder;
        }
    }
}
