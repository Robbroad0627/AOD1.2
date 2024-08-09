using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

public class WorldBoat : MonoBehaviour
{
    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            m_Animator.SetBool("FacingRight", true);
            m_Animator.SetBool("FacingLeft", false);
            m_Animator.SetBool("FacingUp", false);
            m_Animator.SetBool("FacingDown", false);
        }

        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            m_Animator.SetBool("FacingRight", false);
            m_Animator.SetBool("FacingLeft", true);
            m_Animator.SetBool("FacingUp", false);
            m_Animator.SetBool("FacingDown", false);
        }

        if (Input.GetAxisRaw("Vertical") == 1)
        {
            m_Animator.SetBool("FacingRight", false);
            m_Animator.SetBool("FacingLeft", false);
            m_Animator.SetBool("FacingUp", true);
            m_Animator.SetBool("FacingDown", false);
        }

        if (Input.GetAxisRaw("Vertical") == -1)
        {
            m_Animator.SetBool("FacingRight", false);
            m_Animator.SetBool("FacingLeft", false);
            m_Animator.SetBool("FacingUp", false);
            m_Animator.SetBool("FacingDown", true);
        }
    }
}
