using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    private InteractableTilemap m_InteractableTilemap;

    public void Initialize()
    {
        m_InteractableTilemap = GameFlow.Instance.GridManager.InteractableTilemap;
    }

    public void Reset()
    {
        // clear the board of resources
        m_InteractableTilemap.ClearResources();

        StartCoroutine(SpawnResources(GameConstants.eEnergyType.DEFAULT, 5));
    }

    public IEnumerator SpawnResources(GameConstants.eEnergyType type, int num)
    {
        for (int i = 0; i < num; i++)
        {
            SpawnResource(type);
            yield return new WaitForSeconds(Random.Range(0.0f, 0.8f));
        }
        
        yield return null;
    }

    private void SpawnResource(GameConstants.eEnergyType type)
    {
        Vector3Int? cell = m_InteractableTilemap.GetRandomAvailableCellForResource();
        if (cell != null)
        {
            ElementResource resource = Instantiate<GameObject>((GameObject)Resources.Load(string.Format(GameConstants.ENERGY_RESOURCE_PATH, type.ToString())), m_InteractableTilemap.EnergyResourceAnchor).GetComponent<ElementResource>();
            resource.transform.position = GameFlow.Instance.GridManager.GetCellWorldPos(cell.Value);
            resource.Initialize();

            m_InteractableTilemap.AddInteractableObject(resource);
        }
    }
}
