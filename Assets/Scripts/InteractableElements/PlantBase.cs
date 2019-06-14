using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBase : PlayerInteractableBase
{
    protected bool m_IsBeingHeld = false;

    public override void OnInteracted(CharacterController controller)
    {
        if (string.Compare(controller.tag, GameConstants.PLAYER_TAG) == 0)
        {
            if (!m_IsBeingHeld) // player picks up plant
            {
                m_CellXY = null;
                controller.PickUpPlant(this);
                m_IsBeingHeld = true;
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
        m_CellXY = cell;
        GameFlow.Instance.InteractableTilemap.AddInteractableObject(this);
        transform.parent = GameFlow.Instance.InteractableTilemap.transform;
        transform.position = GameFlow.Instance.Grid.GetCellCenterWorld(cell);
    }
}
