using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class playerController : MonoBehaviour
{

    [Header("Horizontal Movement")]
    public float MovementSpeed = 1;
    public Rigidbody2D rb;
    public float MaxMoveSpeed = 20;

    [Header("Jumping")]
    public float JumpForce = 20;
    public float jumpDelay = 0.15f;
    public float CoyoteTimeMax = 0.3f;
    float coyoteTime = 0f;
    public int MaxJumps = 2;
    public int jumpsLeft;
    private float jumpTimer;
    public LayerMask groundLayers;
    public LayerMask spikeLayers;
    public LayerMask bouncyLayers;

    private Vector3 colliderOffset = new Vector3(0.22f, 0, 0);
    private bool grounded = false;
    private bool wasGrounded = false;

    [Header("Wall Grabbing")]
    public float wallGrabTime = 0.2f;
    float lastGrabbingTime = 0;
    float lastWallTouchTime = 0;

    [Header("Wall Jumping")]
    public float wallJumpForce = 25;
    public float wallUp = 1;
    public float wallSide = 4;
    public float wallJumpTime = 0.01f;
    float lastWallJumpTime = 0;
    public float wallSlideSpeed = 0.3f;
    public float wallDistance = 0.5f;
    bool isOnWall = false;
    bool isFacingRight = true;
    bool rightTouch = false;
    bool leftTouch = false;
    bool grabControl = false;
    RaycastHit2D wallHitL;
    RaycastHit2D wallHitR;

    [Header("Bouncy Blocks")]
    public float onBounceMultiplier = 1.5f;

    [Header("Misc. Pointers We Need")]
    public Animator animator;
    public PhysicsMaterial2D playerMat;
    public SpriteRenderer playerRenderer;

    [Header("particles")]
    public ParticleSystem footDust;
    private ParticleSystem.EmissionModule footEmission;
    public float dustAmount = 50;
    public ParticleSystem impactDust;

    [Header("Colors")]
    public Color normColor;
    public Color invincColor;
    public Color deadColor;

    bool isAlive = true;
    bool onBouncy = false;
    bool canMove = true;
    bool onMovPlat = false;
    float mx;
    bool JumpRequest;
    float gScale;
    Transform movPlatformOn;

    private void Start()
    {
        gScale = rb.gravityScale;
        footEmission = footDust.emission;
    }
    void Update()
    {
        mx = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            grabControl = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            grabControl = false;
            animator.SetBool("OnTheWall", false);
        }

        if (grounded)
        {
            rb.gravityScale = 0;
            coyoteTime = Time.time + CoyoteTimeMax;
        } else
        {
            rb.gravityScale = gScale;
        }

        checkCollisions();

        animator.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetBool("Grounded", grounded);
    }
    private void FixedUpdate()
    {
        if (canMove && !PersistentManager._instance.transitioning)
        {
            if (!((lastWallJumpTime + wallJumpTime) > Time.time))
            {
                HorizontalMovement();
            }

            VerticalMovement();
            WallGrabbing();

            if (mx != 0 && grounded) footEmission.rateOverTime = dustAmount;
            else footEmission.rateOverTime = 0;
        } else
        {
            rb.velocity = new Vector2(0, 0);
        }
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxMoveSpeed);


        if (!isAlive)
        {
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
            canMove = false;
            playerRenderer.color = deadColor;
        }


        if (grounded && !wasGrounded) impactDust.Play(); 
        wasGrounded = grounded;
    }

    void HorizontalMovement()
    {
        /*        if( mx > 0.5 || mx < -0.5){
             Vector2 Movement = new Vector2(mx * MovementSpeed, rb.velocity.y);
             rb.velocity = Movement;
         }*/
        if (Mathf.Abs(rb.velocity.y) < 0.1)
        {
            Vector2 Movement = new Vector2(mx * MovementSpeed, rb.velocity.y);
            rb.velocity = Movement;
        }
        else
        {
            Vector2 Movement = new Vector2(mx * MovementSpeed / 1.5f, rb.velocity.y);
            rb.velocity = Movement;
        }

    }
    void VerticalMovement()
    {

        if (grounded) jumpsLeft = MaxJumps;
        dotheBounce();
        if ((jumpTimer > Time.time))
        {
            if (((lastWallTouchTime + wallGrabTime) > Time.time || lastGrabbingTime > Time.time)&& PersistentManager._instance.wallJumping)
            {
                WallJumping();
            }
            if (!(lastGrabbingTime > Time.time) && jumpsLeft > 0)
            {
                if (jumpsLeft < MaxJumps) Jump(1);
                else if (coyoteTime > Time.time) Jump(1);
            }
        }
    }
    void Jump(float bMulti)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * JumpForce * (jumpsLeft + 2) * bMulti, ForceMode2D.Impulse);
        animator.SetTrigger("takeOff");
        jumpTimer = 0;
        jumpsLeft--;
        JumpRequest = false;
        
    }

    void WallGrabbing()
    {
        //if (((isOnWall && rightTouch && mx > 0.5f) || (isOnWall && leftTouch && mx < -0.5f)))
        if (((isOnWall && rightTouch && grabControl) || (isOnWall && leftTouch && grabControl)))
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            lastGrabbingTime = Time.time + wallGrabTime;

        }
        else
        {
            rb.gravityScale = gScale;
            animator.SetBool("OnTheWall", false);
        }
    }
    void WallJumping()
    {
        if (!grounded)
        {
            lastWallJumpTime = Time.time;
            rb.gravityScale = gScale;
            rb.velocity = new Vector2(0, 0);
            if (rightTouch)
            {
                rb.AddForce(((new Vector2(-wallSide, wallUp)) * wallJumpForce * (MaxJumps + 2)), ForceMode2D.Impulse);

            }
            else
            {
                rb.AddForce(((new Vector2(wallSide, wallUp)) * wallJumpForce * (MaxJumps + 2)), ForceMode2D.Impulse);
            }
            jumpTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PersistentManager._instance.invincOn)
        {
            if (collision.gameObject.CompareTag("Damage"))
            {
                isAlive = false;
            }
        }
    }

    public void checkCollisions()
    {
        checkGrounded();
        if (mx > 0.5 && !isFacingRight) isFacingRight = true;
        else if (mx < -0.5 && isFacingRight) isFacingRight = false;

        wallHitR = Physics2D.Raycast(transform.position, Vector2.right, wallDistance, groundLayers);

        wallHitL = Physics2D.Raycast(transform.position, Vector2.left, wallDistance, groundLayers);

        if (wallHitR) rightTouch = true;
        else rightTouch = false;

        if (wallHitL) leftTouch = true;
        else leftTouch = false;

        if (!grounded)
        {
            if (rightTouch || leftTouch)
            {
                isOnWall = true;
                lastWallTouchTime = Time.time;
            }
            else
            {
                isOnWall = false;
            }
        }
        else
        {
            isOnWall = false;
        }
        if(!PersistentManager._instance.invincOn) checkSpikes();
        checkBouncy();
    }
    public void checkGrounded()
    {
        //grounded = (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, 0.55f, groundLayers)) || (Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, 0.55f, groundLayers));
        grounded = (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, 0.55f, groundLayers));
        grounded = grounded || (Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, 0.55f, groundLayers));
        grounded = grounded || (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, 0.55f, bouncyLayers));
        grounded = grounded || (Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, 0.55f, bouncyLayers));
        if (PersistentManager._instance.invincOn)
        {
            grounded = grounded || (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, 0.55f, spikeLayers));
            grounded = grounded || (Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, 0.55f, spikeLayers));
        }
    }
    public void checkSpikes()
    {
        bool spikeHit = (Physics2D.Raycast(transform.position, Vector2.down, 0.55f, spikeLayers));
        if (spikeHit)
        {
            isAlive = false;
        }
    }
    public void checkBouncy()
    {
        onBouncy = Physics2D.Raycast(transform.position, Vector2.down, 0.55f, bouncyLayers);
    }
    void dotheBounce()
    {
        if (onBouncy)
        {
            Jump(onBounceMultiplier);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, (transform.position + (Vector3.down * 0.55f)) + colliderOffset);
        Gizmos.DrawLine(transform.position - colliderOffset, (transform.position + (Vector3.down * 0.55f)) - colliderOffset);
        Gizmos.DrawLine(transform.position, (transform.position + Vector3.right * wallDistance));
        Gizmos.DrawLine(transform.position, (transform.position + Vector3.left * wallDistance));
    }

    public void setinvincColor()
    {
        if(PersistentManager._instance.invincOn) playerRenderer.color = invincColor;
        else playerRenderer.color = normColor;
    }

    public bool getIsAlive()
    {
        return isAlive;
    }
}
