using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInteractableBase : MonoBehaviour
{
    protected Vector3Int? m_CellXY;

    [SerializeField] protected int m_NumberHits = 1;
    protected int m_CurrentHits = 0;

    protected Animator m_Animator;

    public Vector3Int? CellXY
    {
        get
        {
            return m_CellXY;
        }
    }

    public virtual void Initialize()
    {
        m_CurrentHits = 0;
        m_CellXY = GameFlow.Instance.GridManager.WorldPosToCell(transform.position);
        m_Animator = GetComponentInChildren<Animator>();
    }

    public virtual void Reset()
    {
        m_CurrentHits = 0;
    }

    protected virtual void DestroySelf()
    {
        GameFlow.Instance.GridManager.InteractableTilemap.RemoveInteractableObject(this);
        Destroy(gameObject);
    }

    public abstract void OnInteracted(CharacterBase controller);
}
