using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] private List<EnergyUISlot> m_EnergyUISlots;
    [SerializeField] private CraftingManager m_CraftingManager; // this is so gross...but I don't wanna fix it yet...running out of time

    private Dictionary<GameConstants.eEnergyType, int> m_Energy = new Dictionary<GameConstants.eEnergyType, int>();

    public void Initialize()
    {
        GameFlow.Instance.CraftingMenuClosed = OnCraftingUIClosed;

        for (int i = 0; i < m_EnergyUISlots.Count; i++)
        {
            m_EnergyUISlots[i].Initialize(m_CraftingManager.CheckIfOnSlot);
        }
    }

    public void AddEnergy(GameConstants.eEnergyType energy)
    {
        if (!m_Energy.ContainsKey(energy))
        {
            m_Energy.Add(energy, 0);
        }

        m_Energy[energy]++;

        UpdateEnergyUIText(energy);
    }

    public void RemoveEnergy(GameConstants.eEnergyType energy)
    {
        if (m_Energy.ContainsKey(energy))
        {
            m_Energy[energy]--;
        }

        UpdateEnergyUIText(energy);
    }

    public int GetNumberEnergyOfType(GameConstants.eEnergyType energyType)
    {
        if (m_Energy.ContainsKey(energyType))
        {
            return m_Energy[energyType];
        }

        return 0;
    }

    private void UpdateEnergyUIText(GameConstants.eEnergyType energy)
    {
        for (int i = 0; i < m_EnergyUISlots.Count; i++)
        {
            if (m_EnergyUISlots[i].EnergyType == energy)
            {
                m_EnergyUISlots[i].UpdateText(m_Energy[energy].ToString());
            }
        }
    }

    public void OnCraftingUIClosed()
    {
        for (int i = 0; i < m_EnergyUISlots.Count; i++)
        {
            m_EnergyUISlots[i].OnEndDrag(null);
        }
    }
}
