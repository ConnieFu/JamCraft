using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private List<CraftingSlot> m_CraftingSlots;
    [SerializeField] private EnergyManager m_EnergyManager;

    private CraftingSlot m_SelectedSlot = null;

    public void Initialize()
    {
        for (int i = 0; i < m_CraftingSlots.Count; i++)
        {
            m_CraftingSlots[i].Initialize(this);
        }
    }

    public void SetSelectedSlot(CraftingSlot slot)
    {
        m_SelectedSlot = slot;
    }

    public void CheckIfOnSlot(EnergyUISlot energySlot)
    {
        if (m_EnergyManager.GetNumberEnergyOfType(energySlot.EnergyType) > 0 && m_SelectedSlot != null)
        {
            RemoveSelectedSlotEnergy();
            m_SelectedSlot.AddEnergyToSlot((GameObject)Instantiate(Resources.Load(string.Format(GameConstants.STATIC_ENERGY_PREFAB_PATH, energySlot.EnergyType.ToString()))), energySlot.EnergyType);
            m_EnergyManager.RemoveEnergy(energySlot.EnergyType);
        }
    }

    public void RemoveSelectedSlotEnergy()
    {
        if (m_SelectedSlot.EnergyType != null)
        {
            m_EnergyManager.AddEnergy(m_SelectedSlot.EnergyType.Value);
            m_SelectedSlot.ClearSlot();
        }
    }

    public void CraftPlant()
    {
        if (!AreSlotsEmpty())
        {
            Debug.LogError("CRAFT SOMETHING");
            ClearSlots();

            GameFlow.Instance.HideCraftingMenu();
            ((GameObject)Instantiate(Resources.Load(GameConstants.BASE_PLANT_PREFAB))).GetComponent<PlantBase>().OnInteracted(GameFlow.Instance.CharacterController);
        }
    }

    private bool AreSlotsEmpty()
    {
        for (int i = 0; i < m_CraftingSlots.Count; i++)
        {
            if (m_CraftingSlots[i].EnergyType == null)
            {
                return true;
            }
        }

        return false;
    }

    private void ClearSlots()
    {
        for (int i = 0; i < m_CraftingSlots.Count; i++)
        {
            m_CraftingSlots[i].ClearSlot();
        }
    }
}