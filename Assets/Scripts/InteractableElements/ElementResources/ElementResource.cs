using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementResource : PlayerInteractableBase
{
    [SerializeField] protected int m_SpawnAmount = 3;

    public override void OnInteracted(CharacterController controller)
    {
        if (string.Compare(controller.tag, GameConstants.PLAYER_TAG) == 0)
        {
            m_CurrentHits++;

            if (m_CurrentHits >= m_NumberHits)
            {
                // spawn resources to be picked up
                SpawnResources();
                DestroySelf();
            }
        }
    }

    protected virtual void SpawnResources()
    {
        for (int i = 0; i < m_SpawnAmount; i++)
        {
            Instantiate(Resources.Load(GameConstants.DEFAULT_RESOURCE_PREFAB_PATH), transform.position, Quaternion.identity, GameFlow.Instance.FloatingResourcesParent);
        }
    }
}
