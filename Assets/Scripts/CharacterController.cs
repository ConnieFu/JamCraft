using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private const float LERP_VALUE = 0.8f;

    private Vector3 m_Input = Vector3.zero;

    [SerializeField] private float m_Speed = 20.0f;
    [SerializeField] private Rigidbody2D m_CharacterRigidBody;
    [SerializeField] private Animator m_CharacterAnimator;
    [SerializeField] private SpriteRenderer m_CharacterSprite;

    void FixedUpdate()
    {
        m_Input.x = Mathf.Lerp(0, Input.GetAxis("Horizontal") * m_Speed, LERP_VALUE);
        m_Input.y = Mathf.Lerp(0, Input.GetAxis("Vertical") * m_Speed, LERP_VALUE);

        m_CharacterAnimator.SetBool("IsMoving", m_Input.magnitude > 0.0f);

        if (m_Input.y > 0)
        {
            m_CharacterAnimator.SetInteger("Direction", 1);
        }
        else if (m_Input.y < 0)
        {
            m_CharacterAnimator.SetInteger("Direction", 0);
        }
        else if (m_Input.x > 0)
        {
            m_CharacterAnimator.SetInteger("Direction", 2);
            m_CharacterSprite.flipX = false;
        }
        else if (m_Input.x < 0)
        {
            m_CharacterAnimator.SetInteger("Direction", 2);
            m_CharacterSprite.flipX = true;
        }

        m_CharacterRigidBody.velocity = m_Input;
    }

    // Handle "X" Button clicked
    // uses tile info
    private void InteractWithTile()
    {

    }
}
