using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnergyUISlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameConstants.eEnergyType m_EnergyType;

    public GameConstants.eEnergyType EnergyType
    {
        get
        {
            return m_EnergyType;
        }
    }

    [Header("")]
    [SerializeField] private Text m_Text;
    [SerializeField] private GameObject m_DraggingIcon;

    private RectTransform m_DraggingPlane;
    private Vector3 m_GlobalMousePos;
    private Vector3 m_StartPosition;

    public void Initialize()
    {
        m_StartPosition = m_DraggingIcon.transform.position;
        m_DraggingPlane = GameFlow.Instance.Canvas.transform as RectTransform;
    }

    public void UpdateText(string newText)
    {
        m_Text.text = newText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameFlow.Instance.IsCrafting && GameFlow.Instance.IsPaused)
        {
            SetDraggedPosition(eventData);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (GameFlow.Instance.IsCrafting && GameFlow.Instance.IsPaused)
        {
            SetDraggedPosition(data);
        }
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        RectTransform rt = m_DraggingIcon.GetComponent<RectTransform>();
        
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out m_GlobalMousePos))
        {
            rt.position = m_GlobalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }

    // call this when menu exits
    public void OnEndDrag(PointerEventData eventData)
    {
        // snap back to original position
        m_DraggingIcon.transform.position = m_StartPosition;

        // check if on top of slot position and add one thingy to slot
    }
}
