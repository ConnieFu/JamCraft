using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] protected Grid m_Grid;
    protected List<Tilemap> m_TileMaps = new List<Tilemap>();
    protected List<TileBase> m_SelectedTiles = new List<TileBase>();
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
        Tilemap[] tilemaps = m_Grid.GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tilemap in tilemaps)
        {
            m_TileMaps.Add(tilemap);
        }

        m_CharacterRigidBody = GetComponent<Rigidbody2D>();
    }

    public virtual void ResetCharacter()
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
        GameFlow.Instance.InteractableTilemap.CharacterInteraction(m_CurrentTilePos, this);
    }

    protected virtual void UpdateSelectedTiles()
    {
        if (m_TileMaps != null && m_TileMaps.Count > 0)
        {
            m_SelectedTiles.Clear();
            m_CurrentTilePos = m_Grid.WorldToCell(transform.position) + m_FacingDirection;
            for (int i = 0; i < m_TileMaps.Count; i++)
            {
                if (m_TileMaps[i].GetTile(m_CurrentTilePos) != null)
                {
                    m_SelectedTiles.Add(m_TileMaps[i].GetTile(m_CurrentTilePos));
                }
            }
        }
    }
}
