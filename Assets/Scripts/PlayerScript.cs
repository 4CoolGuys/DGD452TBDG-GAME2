using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float dashSpeed = 10f;
    public float dashCooldown = 6f;
    public float coyoteTime = 0.75f; // Time after leaving ground that the player can still jump
    
    // World state
    public bool inFirstWorld = true; // Start in "worldTypeFirst"
    public bool canDash = true; // Dash cooldown flag
    private float lastGroundedTime; // For coyote time jumping
    private float dashTimeStamp; // Timestamp to track cooldown
    private int dashDirection = 0; // -1 for left, 1 for right

    // Movement and ground checking
    public bool isGrounded;
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
        // Horizontal movement
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

        // Jump input
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && CanJump())
        {
            Jump();
        }

        // Double-tap to dash
        HandleDash();

        // World switch input
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SwitchWorld();
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
        inFirstWorld = !inFirstWorld; // Toggle world state
        string activeWorldTag = inFirstWorld ? "firstWorld" : "secondWorld";

        // Change physics properties to match the new world (simplified example)
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