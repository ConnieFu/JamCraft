using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour
{
    private const float LERP_VALUE = 0.8f;
    private const float HIGHLIGHT_Z_DEPTH = 0.1f;

    [Header("Movement")]
    [SerializeField] private float m_Speed = 20.0f;
    [SerializeField] private Rigidbody2D m_CharacterRigidBody;
    private Vector3 m_Input = Vector3.zero;

    [Header("Controls")]
    [SerializeField] private Transform m_HoldAnchor;
    public PlantBase m_HeldPlant = null;

    [Header("Animations")]
    [SerializeField] private Animator m_CharacterAnimator;
    [SerializeField] private SpriteRenderer m_CharacterSprite;

    private Vector3Int m_FacingDirection = Vector3Int.down;

    [Header("ResourceManagement")]
    [SerializeField] private CharacterPickUp m_CharacterPickUp;

    [Header("")]
    [SerializeField] private Grid m_Grid;
    [SerializeField] private SpriteRenderer m_TileHighlight;
    private List<Tilemap> m_TileMaps = new List<Tilemap>();
    private List<TileBase> m_SelectedTiles = new List<TileBase>();
    private Vector3Int m_CurrentTilePos;
    private Vector3 m_CurrentHighlightPos;

    public void Initialize()
    {
        Tilemap[] tilemaps = m_Grid.GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tilemap in tilemaps)
        {
            m_TileMaps.Add(tilemap);
        }

        transform.position = Vector3.zero;
        transform.parent.gameObject.SetActive(true);

        m_CharacterPickUp.EnergyManager.Initialize();
    }

    public void ResetCharacter()
    {
        transform.parent.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!GameFlow.Instance.IsPaused)
        {
            HandleMovement();

            UpdateSelectedTiles();

            // Handle Highlighting Tile in front of Character
            SetHighlightedTile();

            InteractWithTile();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameFlow.Instance.HideCraftingMenu();
            }
        }
    }

    private void HandleMovement()
    {
        m_Input.x = Mathf.Lerp(0, Input.GetAxis("Horizontal") * m_Speed, LERP_VALUE);
        m_Input.y = Mathf.Lerp(0, Input.GetAxis("Vertical") * m_Speed, LERP_VALUE);

        m_CharacterAnimator.SetBool("IsMoving", m_Input.magnitude > 0.0f);

        if (m_Input.y > 0)
        {
            m_FacingDirection = Vector3Int.up;
            m_CharacterAnimator.SetInteger("Direction", 1);
        }
        else if (m_Input.y < 0)
        {
            m_FacingDirection = Vector3Int.down;
            m_CharacterAnimator.SetInteger("Direction", 0);
        }
        else if (m_Input.x > 0)
        {
            m_FacingDirection = Vector3Int.right;
            m_CharacterAnimator.SetInteger("Direction", 2);
            m_CharacterSprite.flipX = false;
        }
        else if (m_Input.x < 0)
        {
            m_FacingDirection = Vector3Int.left;
            m_CharacterAnimator.SetInteger("Direction", 2);
            m_CharacterSprite.flipX = true;
        }

        m_CharacterRigidBody.velocity = m_Input;
    }

    private void UpdateSelectedTiles()
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

    // gets list of tiles from tilemaps at the position of the character
    private void SetHighlightedTile()
    {
        if (m_SelectedTiles.Count > 0)
        {
            m_TileHighlight.enabled = true;
            m_CurrentHighlightPos = m_Grid.GetCellCenterWorld(m_CurrentTilePos);
            m_CurrentHighlightPos.z = HIGHLIGHT_Z_DEPTH;
            m_TileHighlight.transform.position = m_CurrentHighlightPos;
        }
        else
        {
            m_TileHighlight.enabled = false;
        }
    }

    private void InteractWithTile()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_HeldPlant == null) // can't do anything while holding a plant
            {
                GameFlow.Instance.InteractableTilemap.CharacterInteraction(m_CurrentTilePos, this);
            }
            else // put plant down
            {
                PutPlantDown();
            }
        }
    }

    public void PickUpPlant(PlantBase plant)
    {
        if (plant.CellXY.HasValue)
        {
            GameFlow.Instance.InteractableTilemap.RemoveInteractableObject(plant);
        }
        m_HeldPlant = plant;
        plant.transform.parent = m_HoldAnchor;
        plant.transform.localPosition = Vector3.zero;
    }

    private void PutPlantDown()
    {
        if (!GameFlow.Instance.InteractableTilemap.IsTileOccupied(m_CurrentTilePos))
        {
            m_HeldPlant.PlacePlant(m_CurrentTilePos);
            m_HeldPlant = null;
        }
    }
}
