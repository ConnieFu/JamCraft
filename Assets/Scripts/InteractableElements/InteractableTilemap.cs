using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTilemap : MonoBehaviour
{
    private Dictionary<Vector3Int, PlayerInteractableBase> m_InteractableObjects = new Dictionary<Vector3Int, PlayerInteractableBase>();

    public void Initialize()
    {
        RefreshInteractableObjects();
    }

    public bool IsTileOccupied(Vector3Int cell)
    {
        if (m_InteractableObjects.ContainsKey(cell))
        {
            return true;
        }
        return false;
    }

    public void CharacterInteraction(Vector3Int cell, CharacterController controller)
    {
        if (m_InteractableObjects.ContainsKey(cell))
        {
            m_InteractableObjects[cell].OnInteracted(controller);
        }
    }

    public bool AddInteractableObject(PlayerInteractableBase obj)
    {
        // check if object can be added, if so add to dictionary and return true else return false
        if (!m_InteractableObjects.ContainsKey(obj.CellXY))
        {
            m_InteractableObjects.Add(obj.CellXY, obj);
            return true;
        }
     
        return false;
    }

    public void RemoveInteractableObject(Vector3Int cell)
    {
        if (m_InteractableObjects.ContainsKey(cell))
        {
            m_InteractableObjects.Remove(cell);
        }
    }

    public void RemoveInteractableObject(PlayerInteractableBase obj)
    {
        RemoveInteractableObject(obj.CellXY);
    }

    private void RefreshInteractableObjects()
    {
        m_InteractableObjects.Clear();
        foreach (PlayerInteractableBase interactableObject in transform.GetComponentsInChildren<PlayerInteractableBase>())
        {
            interactableObject.Initialize();
            m_InteractableObjects.Add(interactableObject.CellXY, interactableObject);
        }
    }
}
