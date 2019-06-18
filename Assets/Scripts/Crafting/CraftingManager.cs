using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private List<CraftingSlot> m_CratingSlots;

    private CraftingSlot m_SelectedSlot = null;

    public void Initialize()
    {
    }
}
