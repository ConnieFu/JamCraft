using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private const float LERP_VALUE = 0.8f;

    private Vector3 m_Input = Vector3.zero;

    [SerializeField] private float m_Speed = 500.0f;
    [SerializeField] private Rigidbody2D m_CharacterRigidBody;

    void FixedUpdate()
    {
        m_Input.x = Mathf.Lerp(0, Input.GetAxis("Horizontal") * m_Speed, LERP_VALUE);
        m_Input.y = Mathf.Lerp(0, Input.GetAxis("Vertical") * m_Speed, LERP_VALUE);

        m_CharacterRigidBody.velocity = m_Input;
    }
}
