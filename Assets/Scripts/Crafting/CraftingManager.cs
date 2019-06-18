using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private List<CraftingSlot> m_CratingSlots;

    private CraftingSlot m_SelectedSlot = null;

    public void Initialize()
    {
        for (int i = 0; i < m_CratingSlots.Count; i++)
        {
            m_CratingSlots[i].Initialize(this);
        }
    }

    public void SetSelectedSlot(CraftingSlot slot)
    {
        m_SelectedSlot = slot;
    }
}
