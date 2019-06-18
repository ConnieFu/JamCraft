using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   [SerializeField] private GameConstants.eSlot m_SlotType;
    private CraftingManager m_CraftingManager;

    public GameConstants.eSlot SlotType
    {
        get
        {
            return m_SlotType;
        }
    }

    public void Initialize(CraftingManager manager)
    {
        m_CraftingManager = manager;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_CraftingManager.SetSelectedSlot(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_CraftingManager.SetSelectedSlot(null);
    }
}
