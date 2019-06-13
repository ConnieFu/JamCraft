using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInteractableBase : MonoBehaviour
{
    private Vector3Int m_CellXY;

    [SerializeField] protected int m_NumberHits = 1;
    protected int m_CurrentHits = 0;

    protected InteractableTilemap m_InteractableTilemap;

    public Vector3Int CellXY
    {
        get
        {
            return m_CellXY;
        }
    }

    public virtual void Initialize(InteractableTilemap interactableTilemap)
    {
        m_CurrentHits = 0;
        m_CellXY = GameFlow.Instance.Grid.WorldToCell(transform.position);
        m_InteractableTilemap = interactableTilemap;
    }

    protected virtual void DestroySelf()
    {
        m_InteractableTilemap.RemoveInteractableObject(this);
        Destroy(gameObject);
    }

    public virtual void OnInteracted(bool isChar = true)
    {
        m_CurrentHits++;
    }
}
