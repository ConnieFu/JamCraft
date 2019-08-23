using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouching : MonoBehaviour, ITouchable
{
    [SerializeField] private SpriteRenderer m_Sprite;
    [SerializeField] private Color m_HoldColor;
    [SerializeField] private float m_TouchSize;
    [SerializeField] private Color m_StartColor;

    private void Start()
    {
        m_StartColor = m_Sprite.color;
    }

#if !UNITY_IOS && !UNITY_ANDROID
    // only do these if we're not on mobile
    private void OnMouseDown()
    {
        OnTouchBegin();
    }

    private void OnMouseDrag()
    {
        OnTouchHold(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnMouseUp()
    {
        OnTouchEnd();
    }
#endif

    public void OnTouchBegin()
    {

    }

    public void OnTouchTapped()
    {
        Hashtable hash = new Hashtable();
        hash.Add("amount", Vector3.one * m_TouchSize);
        hash.Add("time", 0.1f);
        iTween.PunchScale(m_Sprite.gameObject, hash);
        Debug.LogError("I TAPPED THE FIRE");
    }

    public void OnTouchHold(Vector2 position)
    {
        transform.position = position;
        m_Sprite.color = m_HoldColor;
        Debug.LogError("I'M HOLDING THE FIRE");
    }

    public void OnTouchEnd()
    {
        m_Sprite.color = m_StartColor;
        Debug.LogError("LEGGO OF ME");
    }
}
