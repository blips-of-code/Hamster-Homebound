using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public static SimplePlayerMovement instance;

    public float moveSpeed;
    public Rigidbody2D theRB;
    public float jumpForce;

    private bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    private bool canDoubleJump;

    private Animator anim;
    private SpriteRenderer theSR;

    public float knockBackLength, knockBackForce;
    private float knockBackCounter;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockBackCounter <= 0)
        {
            theRB.linearVelocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), theRB.linearVelocity.y);

            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);

            if (isGrounded)
            {
                canDoubleJump = true;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
                }
                else
                {
                    if (canDoubleJump)
                    {
                        theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
                        canDoubleJump = false;
                    }
                }
            }

            if (theRB.linearVelocity.x < 0)
            {
                theSR.flipX = true;
            }
            else if (theRB.linearVelocity.x > 0)
            {
                theSR.flipX = false;
            }

        }
        else
        {
            knockBackCounter -= Time.deltaTime;
            if (!theSR.flipX)
            {
                theRB.linearVelocity = new Vector2(-knockBackForce, theRB.linearVelocity.y);
            }
            else
            {
                theRB.linearVelocity = new Vector2(knockBackForce, theRB.linearVelocity.y);
            }
        }

        anim.SetFloat("moveSpeed", Mathf.Abs(theRB.linearVelocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    public void KnockBack()
    {
        knockBackCounter = knockBackLength;
        /*theRB.linearVelocity = new Vector2(0f, knockBackForce);

        anim.SetTrigger("hurt");*/
    }

}
