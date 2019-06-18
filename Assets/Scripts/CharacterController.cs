using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private const float LERP_VALUE = 0.8f;

    private Vector3 m_Input = Vector3.zero;

    [SerializeField] private float m_Speed = 20.0f;
    [SerializeField] private Rigidbody2D m_CharacterRigidBody;
    [SerializeField] private Animator m_CharacterAnimator;
    [SerializeField] private SpriteRenderer m_CharacterSprite;
<<<<<<< Updated upstream
=======
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
>>>>>>> Stashed changes

    void FixedUpdate()
    {
<<<<<<< Updated upstream
        m_Input.x = Mathf.Lerp(0, Input.GetAxis("Horizontal") * m_Speed, LERP_VALUE);
=======
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
>>>>>>> Stashed changes
        m_Input.y = Mathf.Lerp(0, Input.GetAxis("Vertical") * m_Speed, LERP_VALUE);

        m_CharacterAnimator.SetBool("IsMoving", m_Input.magnitude > 0.0f);

        if (m_Input.y > 0)
        {
            m_CharacterAnimator.SetInteger("Direction", 1);
        }
        else if (m_Input.y < 0)
        {
            m_CharacterAnimator.SetInteger("Direction", 0);
        }
        else if (m_Input.x > 0)
        {
            m_CharacterAnimator.SetInteger("Direction", 2);
            m_CharacterSprite.flipX = false;
        }
        else if (m_Input.x < 0)
        {
            m_CharacterAnimator.SetInteger("Direction", 2);
            m_CharacterSprite.flipX = true;
        }

        m_CharacterRigidBody.velocity = m_Input;
    }

    // Handle "X" Button clicked
    // uses tile info
    private void InteractWithTile()
    {

    }
}
