using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector2 m_CameraYBounds;
    [SerializeField] private Player m_Character;

    private Vector3 m_CurrentPos = new Vector3();
    private float m_CharacterYPos = 0.0f;

    void FixedUpdate()
    {
        // move the camera up and down based on character position
        m_CurrentPos = transform.position;
        m_CharacterYPos = m_Character.transform.position.y;
        if (m_CharacterYPos > m_CameraYBounds.x)
        {
            m_CharacterYPos = m_CameraYBounds.x;
        }
        else if (m_CharacterYPos < m_CameraYBounds.y)
        {
            m_CharacterYPos = m_CameraYBounds.y;
        }
        m_CurrentPos.y = m_CharacterYPos;
        transform.position = m_CurrentPos;
    }
}
