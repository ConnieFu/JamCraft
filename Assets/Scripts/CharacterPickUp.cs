using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPickUp : MonoBehaviour
{
    private Dictionary<GameConstants.eResourceType, int> m_Resources = new Dictionary<GameConstants.eResourceType, int>();

    public void AddResource(GameConstants.eResourceType resourceType)
    {
        if (!m_Resources.ContainsKey(resourceType))
        {
            m_Resources.Add(resourceType, 0);
        }

        m_Resources[resourceType]++;
    }

    public void RemoveResource(GameConstants.eResourceType resourceType)
    {
        if (m_Resources.ContainsKey(resourceType))
        {
            m_Resources[resourceType]--;
        }
    }
}
