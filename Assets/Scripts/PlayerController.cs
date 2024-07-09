using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{

    public Rigidbody2D theRB;
    public float moveSpeed;

    public Animator myAnim;

    public static PlayerController instance;

    public string areaTransitionName;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    public bool canMove = true;

    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    public Sprite sprite {

        get => m_spriteRenderer.sprite;
    }

    public RuntimeAnimatorController animationController
    {

        get => m_animator.runtimeAnimatorController;
        set { m_animator.runtimeAnimatorController = value; }
    }

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }


        //NOTE:The designer set the instance somewhere so the only safe place to do initialization of component references is here.
        //If you put them at singleton test time they will be null because instnace is already set.
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;

        }
        else
        {
            theRB.velocity = Vector2.zero;
        }


        myAnim.SetFloat("moveX", theRB.velocity.x);
        myAnim.SetFloat("moveY", theRB.velocity.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            if (canMove)
            {
                myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);

        if (Boat.isPlayerOnBoat)
        {
            m_spriteRenderer.enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            m_spriteRenderer.enabled = true;
            GetComponent<Collider2D>().enabled = true;
        }
    }

    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
        topRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    }
}
