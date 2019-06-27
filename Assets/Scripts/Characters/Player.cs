using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : CharacterBase
{
    private const float LERP_VALUE = 0.8f;
    private const float HIGHLIGHT_Z_DEPTH = 0.1f;

    private Vector3 m_Input = Vector3.zero;

    [SerializeField] private Transform m_StartAnchor;

    [Header("Controls")]
    [SerializeField] private Transform m_HoldAnchor;
    public PlantBase m_HeldPlant = null;

    [Header("ResourceManagement")]
    [SerializeField] private CharacterPickUp m_CharacterPickUp;

    [Header("")]
    [SerializeField] private SpriteRenderer m_TileHighlight;
    private Vector3 m_CurrentHighlightPos;

    public override void Initialize()
    {
        base.Initialize();

        m_CharacterPickUp.Initialize();
        transform.parent.gameObject.SetActive(false);
    }

    public override void Reset()
    {
        base.Reset();

        transform.position = m_StartAnchor.position;
        transform.parent.gameObject.SetActive(true);
    }

    protected override void FixedUpdate()
    {
        if (!GameFlow.Instance.IsPaused)
        {
            HandleMovement();

            UpdateSelectedTiles();

            // Handle Highlighting Tile in front of Character
            SetHighlightedTile();

            InteractWithTile();
        }
        else if(GameFlow.Instance.IsCrafting)
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

        m_Animator.SetBool("IsMoving", m_Input.magnitude > 0.0f);

        if (m_Input.y > 0)
        {
            m_FacingDirection = Vector3Int.up;
            m_Animator.SetInteger("Direction", 1);
        }
        else if (m_Input.y < 0)
        {
            m_FacingDirection = Vector3Int.down;
            m_Animator.SetInteger("Direction", 0);
        }
        else if (m_Input.x > 0)
        {
            m_FacingDirection = Vector3Int.right;
            m_Animator.SetInteger("Direction", 2);
            m_SpriteRenderer.flipX = false;
        }
        else if (m_Input.x < 0)
        {
            m_FacingDirection = Vector3Int.left;
            m_Animator.SetInteger("Direction", 2);
            m_SpriteRenderer.flipX = true;
        }

        m_CharacterRigidBody.velocity = m_Input;
    }

    // gets list of tiles from tilemaps at the position of the character
    private void SetHighlightedTile()
    {
        if (!GameFlow.Instance.GridManager.IsCellEmpty(m_CurrentTilePos, false))
        {
            m_TileHighlight.enabled = true;
            m_CurrentHighlightPos = GameFlow.Instance.GridManager.GetCellWorldPos(m_CurrentTilePos);
            m_CurrentHighlightPos.z = HIGHLIGHT_Z_DEPTH;
            m_TileHighlight.transform.position = m_CurrentHighlightPos;
        }
        else
        {
            m_TileHighlight.enabled = false;
        }
    }

    protected override void InteractWithTile()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_HeldPlant == null) // can't do anything while holding a plant
            {
                base.InteractWithTile();
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
            GameFlow.Instance.GridManager.InteractableTilemap.RemoveInteractableObject(plant);
        }
        m_HeldPlant = plant;
        plant.transform.parent = m_HoldAnchor;
        plant.transform.localPosition = Vector3.zero;
    }

    private void PutPlantDown()
    {
        if (GameFlow.Instance.GridManager.InteractableTilemap.CanPlacePlant(m_CurrentTilePos))
        {
            m_HeldPlant.PlacePlant(m_CurrentTilePos);
            m_HeldPlant = null;
        }
    }
}
