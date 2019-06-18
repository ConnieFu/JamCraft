using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantComponentBase : MonoBehaviour
{
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
