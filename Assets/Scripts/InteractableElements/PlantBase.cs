using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBase : PlayerInteractableBase
{
    [SerializeField] protected Renderer m_PlantRenderer;

    protected bool m_IsBeingHeld = false;

    public override void OnInteracted(CharacterController controller)
    {
        if (string.Compare(controller.tag, GameConstants.PLAYER_TAG) == 0)
        {
            if (!m_IsBeingHeld) // player picks up plant
            {
                controller.PickUpPlant(this);
                m_CellXY = null;
                m_IsBeingHeld = true;

                m_PlantRenderer.sortingLayerName = GameConstants.PLAYER_LAYER_NAME;
            }
        }
        else
        {
            m_CurrentHits++;

            if (m_CurrentHits >= m_NumberHits)
            {
                // play death animation
                DestroySelf();
            }
        }
    }

    public void PlacePlant(Vector3Int cell)
    {
        m_IsBeingHeld = false;

        m_PlantRenderer.sortingLayerName = GameConstants.INTERACTABLE_LAYER_NAME;

        m_CellXY = cell;

        GameFlow.Instance.InteractableTilemap.AddInteractableObject(this);
        transform.parent = GameFlow.Instance.InteractableTilemap.PlantAnchor;
        transform.position = GameFlow.Instance.Grid.GetCellCenterWorld(cell);
    }
}
