using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   [SerializeField] private GameConstants.eSlot m_SlotType;
    private CraftingManager m_CraftingManager;

    private Transform m_EnergyTransform = null;
    private GameConstants.eEnergyType? m_EnergyType = null;

    private RectTransform m_CraftingSlotRectTrans;

    public RectTransform CraftingSlotRectTrans
    {
        get
        {
            return m_CraftingSlotRectTrans;
        }
    }

    public GameConstants.eSlot SlotType
    {
        get
        {
            return m_SlotType;
        }
    }

    public GameConstants.eEnergyType? EnergyType
    {
        get
        {
            return m_EnergyType;
        }
    }

    public void Initialize(CraftingManager manager)
    {
        m_CraftingManager = manager;
        m_CraftingSlotRectTrans = GetComponent<RectTransform>();
    }

    public void Reset()
    {
        ClearSlot();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_CraftingManager.SetSelectedSlot(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_CraftingManager.SetSelectedSlot(null);
    }

    public void AddEnergyToSlot(GameObject energyObj, GameConstants.eEnergyType energyType)
    {
        m_EnergyType = energyType;

        m_EnergyTransform = energyObj.transform;
        m_EnergyTransform.SetParent(transform);
        m_EnergyTransform.localPosition = Vector3.zero;
    }

    public void ClearSlot()
    {
        m_EnergyType = null;
        if (m_EnergyTransform != null)
        {
            Destroy(m_EnergyTransform.gameObject);
        }
    }
}
