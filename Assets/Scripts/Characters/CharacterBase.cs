using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterBase : MonoBehaviour
{
    protected Vector3Int m_CurrentTilePos;

    [Header("Movement")]
    [SerializeField] protected float m_Speed = 20.0f;
    protected Rigidbody2D m_CharacterRigidBody;

    [Header("Animations")]
    [SerializeField] protected Animator m_Animator;
    [SerializeField] protected SpriteRenderer m_SpriteRenderer;
    protected Vector3Int m_FacingDirection = Vector3Int.down;

    public virtual void Initialize()
    {
        m_CharacterRigidBody = GetComponent<Rigidbody2D>();
    }

    public virtual void Reset()
    {
        transform.parent.gameObject.SetActive(false);

    }

    protected virtual void FixedUpdate()
    {
        if (!GameFlow.Instance.IsPaused)
        {
            UpdateSelectedTiles();
        }
    }

    protected virtual void InteractWithTile()
    {
        GameFlow.Instance.GridManager.InteractableTilemap.EnemyInteraction(m_CurrentTilePos);
    }

    protected virtual void UpdateSelectedTiles()
    {
        m_CurrentTilePos = GameFlow.Instance.GridManager.WorldPosToCell(transform.position) + m_FacingDirection;
    }
}
