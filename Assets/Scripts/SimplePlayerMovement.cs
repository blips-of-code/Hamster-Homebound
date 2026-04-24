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
    public bool doubleJumpKillReady;

    private Animator anim;
    private SpriteRenderer theSR;

    public float knockBackLength, knockBackForce;
    private float knockBackCounter;

    public bool stopInput;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);

        if (isGrounded)
        {
            canDoubleJump = true;
            doubleJumpKillReady = false;
        }

        if (stopInput)
        {
            theRB.linearVelocity = new Vector2(0f, theRB.linearVelocity.y);
            anim.SetFloat("moveSpeed", Mathf.Abs(theRB.linearVelocity.x));
            anim.SetBool("isGrounded", isGrounded);
            return;
        }

        if (knockBackCounter <= 0)
        {
            theRB.linearVelocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), theRB.linearVelocity.y);

            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
                    AudioManager.instance.PlaySFX(10);
                }
                else if (canDoubleJump)
                {
                    theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
                    canDoubleJump = false;
                    doubleJumpKillReady = true;
                    AudioManager.instance.PlaySFX(10);

                    if (Level1TutorialManager.instance != null)
                    {
                        Level1TutorialManager.instance.RegisterDoubleJump();
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

    public bool IsInDoubleJumpKillState()
    {
        return doubleJumpKillReady;
    }

    public void KnockBack()
    {
        knockBackCounter = knockBackLength;
        /*theRB.linearVelocity = new Vector2(0f, knockBackForce);

        anim.SetTrigger("hurt");*/
    }
}