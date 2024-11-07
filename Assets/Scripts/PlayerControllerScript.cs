using System.Collections;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float dashSpeed = 10f;
    public float dashCooldown = 1f;
    public float coyoteTime = 0.5f;
    private Vector2 lastMoveDirection = Vector2.zero;

    // World state
    public bool inFirstWorld = true;
    private bool canSwitchWorlds = true;
    public float worldSwitchCooldown = 1.5f;
    private float cooldownTimer = 0f;

    // Movement and ground checking
    public bool isGrounded;
    public bool canDash = true;
    private float lastGroundedTime;
    private float dashTimeStamp;
    private int dashDirection = 0;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float horizontalInput;

    // Double-tap dash
    private float lastTapTimeA = 0;
    private float lastTapTimeD = 0;
    private bool isDashing;

    public bool hasKey = false;

    // Store the reference to the box pushing
    private PushableBoxScript pushableBox = null;

    // Sprite flipping
    private SpriteRenderer spriteRenderer;

    // Audio
    public AudioClip jumpSound; // Jump sound clip
    private AudioSource audioSource; // Audio source component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (!isDashing)
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }

        if (horizontalInput != 0)
        {
            spriteRenderer.flipX = horizontalInput < 0;
        }

        isGrounded = IsGrounded();
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && CanJump())
        {
            Jump();
        }

        HandleDash();

        if (canSwitchWorlds && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.F)))
        {
            SwitchWorld();
        }

        if (!canSwitchWorlds)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                canSwitchWorlds = true;
            }
        }

        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movement != Vector2.zero)
        {
            lastMoveDirection = movement.normalized;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        
        // Play the jump sound
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    private bool CanJump()
    {
        return isGrounded || Time.time - lastGroundedTime <= coyoteTime;
    }

    private void HandleDash()
    {
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
        yield return new WaitForSeconds(0.2f);
        isDashing = false;

        yield return new WaitUntil(() => isGrounded);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void SwitchWorld()
    {
        inFirstWorld = !inFirstWorld;
        string activeWorldTag = inFirstWorld ? "firstWorld" : "secondWorld";
        canSwitchWorlds = false;
        cooldownTimer = worldSwitchCooldown;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null && (raycastHit.collider.CompareTag("bothWorlds") || raycastHit.collider.CompareTag(inFirstWorld ? "firstWorld" : "secondWorld") || raycastHit.collider.CompareTag("Box"));
    }

    public float GetHorizontalInput()
    {
        return horizontalInput;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            pushableBox = collision.gameObject.GetComponent<PushableBoxScript>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            pushableBox = null;
        }
    }

    public Vector2 GetLastMoveDirection()
    {
        return lastMoveDirection;
    }
}
