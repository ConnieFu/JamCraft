using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTilemap : MonoBehaviour
{
    [SerializeField] private Transform m_PlantAnchor;
    public Transform PlantAnchor
    {
        get
        {
            return m_PlantAnchor;
        }
    }

    [SerializeField] private Transform m_EnergyResourceAnchor;
    public Transform EnergyResourceAnchor
    {
        get
        {
            return m_EnergyResourceAnchor;
        }
    }

    [SerializeField] private RectInt m_ResourceRectangle;
    [SerializeField] private RectInt m_PlantRectangle;
    private Dictionary<Vector3Int, PlayerInteractableBase> m_InteractableObjects = new Dictionary<Vector3Int, PlayerInteractableBase>();

    private Vector3Int m_HomeBaseCell;
    public Vector3Int HomeBaseCell
    {
        get
        {
            return m_HomeBaseCell;
        }
    }

    public void Initialize()
    {
        RefreshInteractableObjects();
    }

    public void Reset()
    {
        m_InteractableObjects.Clear();
        foreach (PlayerInteractableBase interactableObject in transform.GetComponentsInChildren<PlayerInteractableBase>())
        {
            interactableObject.Reset();
            m_InteractableObjects.Add(interactableObject.CellXY.Value, interactableObject);
        }
    }

    public void ClearResources()
    {
        List<ElementResource> resourcesToDelete = new List<ElementResource>();
        foreach(PlayerInteractableBase interactable in m_InteractableObjects.Values)
        {
            ElementResource resource = interactable.GetComponent<ElementResource>();
            if (resource != null)
            {
                resourcesToDelete.Add(resource);
            }
        }

        foreach(ElementResource resource in resourcesToDelete)
        {
            resource.ClearElementResource();
        }
    }

    public bool CanPlacePlant(Vector3Int cell)
    {
        if (!IsTileOccupied(cell) &&
            cell.x <= m_PlantRectangle.width && cell.x >= m_PlantRectangle.x &&
            cell.y <= m_PlantRectangle.height && cell.y >= m_PlantRectangle.y)
        {
            return true;
        }
        return false;
    }

    public bool CanPlaceResource(Vector3Int cell)
    {
        if (!IsTileOccupied(cell) &&
            cell.x <= m_ResourceRectangle.width && cell.x >= m_ResourceRectangle.x &&
            cell.y <= m_ResourceRectangle.height && cell.y >= m_ResourceRectangle.y)
        {
            return true;
        }
        return false;
    }

    public bool IsTileOccupied(Vector3Int cell)
    {
        if (m_InteractableObjects.ContainsKey(cell))
        {
            return true;
        }
        return false;
    }

    public void EnemyInteraction(Vector3Int cell)
    {
        if (m_InteractableObjects.ContainsKey(cell))
        {
            m_InteractableObjects[cell].OnEnemyHit();
        }
    }

    public Vector3Int? GetRandomAvailableCellForResource()
    {
        // check if grid full
        if ((m_ResourceRectangle.width - m_ResourceRectangle.x) * (m_ResourceRectangle.height - m_ResourceRectangle.y) == m_InteractableObjects.Count)
        {
            return null;
        }

        Vector3Int cell = new Vector3Int();
        cell.x = Random.Range(m_ResourceRectangle.x, m_ResourceRectangle.width + 1);
        cell.y = Random.Range(m_ResourceRectangle.y, m_ResourceRectangle.height + 1);

        if (CanPlaceResource(cell))
        {
            return cell;
        }
        else
        {
            return GetRandomAvailableCellForResource();
        }
    }

    public bool AddInteractableObject(PlayerInteractableBase obj)
    {
        // check if object can be added, if so add to dictionary and return true else return false
        if (!m_InteractableObjects.ContainsKey(obj.CellXY.Value))
        {
            m_InteractableObjects.Add(obj.CellXY.Value, obj);
            GameFlow.Instance.GridManager.UpdateNodeAtCell(obj.CellXY.Value);
            return true;
        }

        return false;
    }

    public void RemoveInteractableObject(Vector3Int cell)
    {
        if (m_InteractableObjects.ContainsKey(cell))
        {
            m_InteractableObjects.Remove(cell);
            GameFlow.Instance.GridManager.UpdateNodeAtCell(cell);
        }
    }

    public void RemoveInteractableObject(PlayerInteractableBase obj)
    {
        RemoveInteractableObject(obj.CellXY.Value);
    }

    private void RefreshInteractableObjects()
    {
        m_InteractableObjects.Clear();
        foreach (PlayerInteractableBase interactableObject in transform.GetComponentsInChildren<PlayerInteractableBase>())
        {
            interactableObject.Initialize();
            m_InteractableObjects.Add(interactableObject.CellXY.Value, interactableObject);

            if (interactableObject.GetComponent<HomeBase>() != null)
            {
                m_HomeBaseCell = interactableObject.CellXY.Value;
            }
        }
    }
}
