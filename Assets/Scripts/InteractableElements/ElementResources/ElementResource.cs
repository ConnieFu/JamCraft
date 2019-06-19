using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementResource : PlayerInteractableBase
{
    [SerializeField] protected int m_SpawnAmount = 3;
    [SerializeField] protected GameConstants.eEnergyType m_ResourceType = GameConstants.eEnergyType.DEFAULT;

    public override void OnInteracted(CharacterBase controller)
    {
        m_CurrentHits++;

        if (m_CurrentHits >= m_NumberHits)
        {
            // spawn resources to be picked up
            SpawnResources();
            DestroySelf();
        }    
    }

    protected virtual void SpawnResources()
    {
        for (int i = 0; i < m_SpawnAmount; i++)
        {
            Instantiate(Resources.Load(string.Format(GameConstants.ENERGY_PREFAB_PATH, m_ResourceType.ToString())), transform.position, Quaternion.identity, GameFlow.Instance.FloatingResourcesParent);
        }
    }
}
