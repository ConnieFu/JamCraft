﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementResource : PlayerInteractableBase
{
    [SerializeField] protected int m_SpawnAmount = 3;
    [SerializeField] protected GameConstants.eEnergyType m_ResourceType = GameConstants.eEnergyType.DEFAULT;

    public override void OnTouchTapped()
    {
        if (m_CurrentHits < m_NumberHits)
        {
            // for now lets try using itween shake
            Hashtable args = new Hashtable();

            args.Add("time", 0.5f);
            args.Add("amount", new Vector3(0.1f, 0.1f, 0f));

            m_CurrentHits++;

            if (m_CurrentHits >= m_NumberHits)
            {
                // play death animation and call destroyself when it's done
                // make sure the resource can't be interacted with anymore
                args.Add("oncomplete", "GenerateEnergy");
                args.Add("oncompletetarget", this.gameObject);
            }

            iTween.ShakePosition(gameObject, args);
        }
    }

    public void ClearElementResource()
    {
        DestroySelf();
    }

    protected virtual void GenerateEnergy()
    {
        for (int i = 0; i < m_SpawnAmount; i++)
        {
            Instantiate(Resources.Load(string.Format(GameConstants.ENERGY_PREFAB_PATH, m_ResourceType.ToString())), transform.position, Quaternion.identity, GameFlow.Instance.FloatingResourcesParent);
        }

        DestroySelf();
    }
}
