using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPickUp : MonoBehaviour
{
    [SerializeField] private EnergyManager m_EnergyManager;

    public EnergyManager EnergyManager
    {
        get
        {
            return m_EnergyManager;
        }
    }

    public void OnResourcePickedUp(GameConstants.eEnergyType energy)
    {
		m_EnergyManager.AddEnergy(energy);
    }
}
