using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float dashSpeed = 10f;
    public float dashCooldown = 1f;
    public float coyoteTime = 0.5f; // Time after leaving ground that the player can still jump
    
    // World state
    public bool inFirstWorld = true; // Start in first world
    private bool canSwitchWorlds = true; // Can switch worlds?
    public float worldSwitchCooldown = 1.5f; // Cooldown time in seconds
    private float cooldownTimer = 0f; // Timer to track cooldown (I hate timers in Unity)
    
    // Movement and ground checking
    public bool isGrounded;
    public bool canDash = true; // Dash cooldown
    private float lastGroundedTime; // For coyote time jumping
    private float dashTimeStamp; // Timestamp to track cooldown
    private int dashDirection = 0; // -1 for left, 1 for right
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float horizontalInput;

    // Double-tap dash
    private float lastTapTimeA = 0;
    private float lastTapTimeD = 0;
    private bool isDashing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Horizontal left Right movement
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (!isDashing)
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }

        // Ground checking
        isGrounded = IsGrounded();
        if (isGrounded)
        {
            lastGroundedTime = Time.time; // Record the last time we were on the ground
        }

        // Jumping
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && CanJump())
        {
            Jump();
        }

        // Double-tap to dash
        HandleDash();

        // World switch
        if (canSwitchWorlds && Input.GetKeyDown(KeyCode.S) || canSwitchWorlds && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SwitchWorld();
            
        }
        
        // World switch cooldown timer
        if (!canSwitchWorlds)
        {
            cooldownTimer -= Time.deltaTime; // Count down the timer
            if (cooldownTimer <= 0)
            {
                canSwitchWorlds = true; // Re-enable world switching
            }
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private bool CanJump()
    {
        return isGrounded || Time.time - lastGroundedTime <= coyoteTime;
    }

    private void HandleDash()
    {
        // Detect double-tap for dashing
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastTapTimeA < 0.5f && canDash)
            {
                StartCoroutine(Dash(-1));
            }
            lastTapTimeA = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - lastTapTimeD < 0.5f && canDash)
            {
                StartCoroutine(Dash(1));
            }
            lastTapTimeD = Time.time;
        }
    }

    private IEnumerator Dash(int direction)
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(direction * dashSpeed, rb.velocity.y);
        yield return new WaitForSeconds(0.2f); // Dash duration
        isDashing = false;

        // Wait until the player touches the ground to reset the dash cooldown
        yield return new WaitUntil(() => isGrounded);
        yield return new WaitForSeconds(dashCooldown); // Dash cooldown
        canDash = true;
    }

    private void SwitchWorld()
    {
        inFirstWorld = !inFirstWorld; // Change  world state
        string activeWorldTag = inFirstWorld ? "firstWorld" : "secondWorld";
        canSwitchWorlds = false; // Prevent switching again
        cooldownTimer = worldSwitchCooldown; // Reset the cooldown timer
        
        // No idea. No touch
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("firstWorld"), !inFirstWorld);
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("secondWorld"), inFirstWorld);
    }

    private bool IsGrounded()
    {
        // Ground check using a small box overlap
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null && (raycastHit.collider.CompareTag("bothWorlds") || raycastHit.collider.CompareTag(inFirstWorld ? "firstWorld" : "secondWorld"));
    }
}
