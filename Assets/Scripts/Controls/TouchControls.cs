using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: If the touch ends off of the object, cancel the touch somehow?? 
public class TouchControls : MonoBehaviour
{
    RaycastHit2D m_TouchRaycastHit;
    private ITouchable m_CurrentTouchable;
    private float m_TapTimer;
    private bool m_WasTapped;

    private void Update()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            m_TouchRaycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
            if (m_TouchRaycastHit.collider != null && m_TouchRaycastHit.collider.TryGetComponent<ITouchable>(out m_CurrentTouchable))
            {
                m_CurrentTouchable = m_TouchRaycastHit.collider.GetComponent<ITouchable>();

                if (m_CurrentTouchable != null)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            m_TapTimer = 0.0f;
                            m_CurrentTouchable.OnTouchBegin();
                            break;

                        case TouchPhase.Moved:
                            if (m_TapTimer > GameConstants.TAP_TIME)
                            {
                                m_CurrentTouchable.OnTouchHold(Camera.main.ScreenToWorldPoint(touch.position));
                            }
                            break;

                        case TouchPhase.Ended:
                            m_WasTapped = false;
                            if (m_TapTimer <= GameConstants.TAP_TIME)
                            {
                                m_CurrentTouchable.OnTouchTapped();
                                m_WasTapped = true;
                            }
                            m_CurrentTouchable.OnTouchEnd(Camera.main.ScreenToWorldPoint(touch.position), m_WasTapped);
                            m_CurrentTouchable = null;
                            break;
                    }
                }

                m_TapTimer += Time.deltaTime;
            }
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            m_TouchRaycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (m_TouchRaycastHit.collider != null && m_TouchRaycastHit.collider.TryGetComponent<ITouchable>(out m_CurrentTouchable))
            {
                m_TapTimer = 0.0f;
                m_CurrentTouchable.OnTouchBegin();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (m_CurrentTouchable != null && m_TapTimer > GameConstants.TAP_TIME)
            {
                m_CurrentTouchable.OnTouchHold(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (m_CurrentTouchable != null)
            {
                m_WasTapped = false;
                if (m_TapTimer <= GameConstants.TAP_TIME)
                {
                    m_CurrentTouchable.OnTouchTapped();
                    m_WasTapped = true;
                }
                m_CurrentTouchable.OnTouchEnd(Camera.main.ScreenToWorldPoint(Input.mousePosition), m_WasTapped);
                m_CurrentTouchable = null;
            }
        }

        m_TapTimer += Time.deltaTime;
#endif
    }
}
