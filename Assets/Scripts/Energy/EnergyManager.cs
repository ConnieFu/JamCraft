using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] private List<EnergyUISlot> m_EnergyUISlots;

    private Dictionary<GameConstants.eEnergyType, int> m_Energy = new Dictionary<GameConstants.eEnergyType, int>();

    public void Initialize()
    {
        GameFlow.Instance.CraftingMenuClosed = OnCraftingUIClosed;

        for (int i = 0; i < m_EnergyUISlots.Count; i++)
        {
            m_EnergyUISlots[i].Initialize();
        }
    }

    public void AddEnergy(GameConstants.eEnergyType energy)
    {
        if (!m_Energy.ContainsKey(energy))
        {
            m_Energy.Add(energy, 0);
        }

        m_Energy[energy]++;
      
        for (int i = 0; i < m_EnergyUISlots.Count; i++)
        {
            if (m_EnergyUISlots[i].EnergyType == energy)
            {
                m_EnergyUISlots[i].UpdateText(m_Energy[energy].ToString());
            }
        }
    }

    public void RemoveEnergy(GameConstants.eEnergyType energy)
    {
        if (m_Energy.ContainsKey(energy))
        {
            m_Energy[energy]--;
        }
    }

    public int GetNumberEnergyOfType(GameConstants.eEnergyType energyType)
    {
        if (m_Energy.ContainsKey(energyType))
        {
            return m_Energy[energyType];
        }

        return 0;
    }

    public void OnCraftingUIClosed()
    {
        for (int i = 0; i < m_EnergyUISlots.Count; i++)
        {
            m_EnergyUISlots[i].OnEndDrag(null);
        }
    }
}
