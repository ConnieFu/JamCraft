using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    public GameObject particle;

    Ray m_TouchRay = new Ray();
    RaycastHit m_TouchRaycastHit = new RaycastHit();
    private ITouchable m_CurrentTouchable = null;

    private void Update()
    {
//#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // get the ITouchable at the position of the touch
            m_TouchRay = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(m_TouchRay, out m_TouchRaycastHit))
            {
                m_CurrentTouchable = m_TouchRaycastHit.collider.GetComponent<ITouchable>();

                if (m_CurrentTouchable != null)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            m_CurrentTouchable.OnTouchBegin();
                            break;

                        case TouchPhase.Moved:
                            m_CurrentTouchable.OnTouchHold(Camera.main.ScreenToWorldPoint(touch.position));
                            break;

                        case TouchPhase.Ended:
                            m_CurrentTouchable.OnTouchEnd();
                            break;
                    }
                }
            }
        }
//#endif
    }
}
