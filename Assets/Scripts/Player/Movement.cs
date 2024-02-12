using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header ("Movement")]
    private float movementSpeed = 5;
    public float acceleration = 10;
    public float deceleration = 10;

    [Tooltip ("How much player speed is reduced in the air")]
    public float airSpeedMod = 0.5f;
    [Tooltip ("How much player acceleration is reduced in the air")]
    public float airAccelerationMod = 0.5f;

    [Header ("Jump")]
    public float jumpInitialVelocity = 5;
    public float jumpHoldPower = 50;
    public float jumpHoldDuration = 0.25f;
    public float jumpHoldEasing = 2;

    [Header ("Accessibility")]
    [Tooltip ("Max time beetween input and jump")]
    public float bufferingTime = 0.1f;
    public float coyoteTime = 0.05f;
    public float stepHeight = 0.1f;

    [Header ("Collision")]
    public LayerMask groundLayer;
    public BoxRay groundRay;
    public SegmentRay stepRay;

    [Header ("Effects")]
    public Transform particleOrigin;
    public GameObject groundParticle;

    private float direction = 1;

    // Components
    private Animator anim;
    private Rigidbody2D rb = null;
    private PlayerInit plInput = null;

    public static bool control; // Variable for cutscenes => Turn off/on movement ability

    // Jump
    private bool isGrounded;
    private float lastGroundedTime = float.NegativeInfinity;
    public Sword sword;


    void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        plInput = GetComponent<PlayerInit>();
        control = true;
        movementSpeed = PlayerStats.PlayerSpeed;
    }

    private void FixedUpdate()
    {
        if (control)
        {
            ApplyStep ();
            GroundCheck ();
            TryJump ();
            ApplyJumpSpeed ();
            MovePlayer ();
        } else {
            anim.SetBool ("IsMoving", false);
            rb.velocity = Vector2.zero;
        }

        anim.SetFloat ("VerticalSpeed", rb.velocity.y);
        anim.SetBool ("IsGrounded", isGrounded);

    }    
    
    private void TryJump () {
        if (isGrounded && (Time.time - plInput.jumpPressTime) < bufferingTime && !sword.isBlock) {
            rb.velocity = Vector2.up * jumpInitialVelocity;
            plInput.jumpStartTime = Time.time;
            lastGroundedTime = float.NegativeInfinity;
        }
    }

    private void ApplyJumpSpeed () {
        float progress = 1 - (Time.time - plInput.jumpStartTime) / jumpHoldDuration;
        if (progress < 0) return;

        progress = Mathf.Pow (progress, jumpHoldEasing);
        rb.velocity += Vector2.up * (jumpHoldPower * progress * Time.fixedDeltaTime);
    }

    private void MovePlayer () {
        float velocity = rb.velocity.x;

        if (plInput.movement.x != 0 && !sword.isBlock) {
            direction = Mathf.Sign (plInput.movement.x);
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            transform.localScale = scale;
            if (velocity * direction < movementSpeed * (isGrounded ? 1 : airSpeedMod)) {
                velocity = Mathf.MoveTowards (velocity,
                                              movementSpeed * direction * (isGrounded ? 1 : airSpeedMod),
                                              acceleration * Time.fixedDeltaTime * (isGrounded ? 1 : airAccelerationMod));
            }
        } else {
            velocity = Mathf.MoveTowards (velocity, 0, deceleration * Time.fixedDeltaTime);
        }
        anim.SetBool ("IsMoving", plInput.movement.x != 0);
        rb.velocity = new Vector2(velocity, rb.velocity.y);
    }

    private void GroundCheck () {
        if (groundRay.Raycast (groundLayer)) lastGroundedTime = Time.time;
        isGrounded = (Time.time - lastGroundedTime) < coyoteTime;
    }

    void OnDrawGizmos () {
        Gizmos.color = new Color (0.2f, 0.5f, 1f);
        groundRay.Draw ();

        Gizmos.color = new Color (0.2f, 1f, 0.2f);
        stepRay.Draw ();
    }


    public void ApplyStep () {
        if (rb.velocity.y > 0 || plInput.movement.x == 0) return;

        float step = CalulateStep ();
        if (step > stepHeight || step == 0) return;
        rb.velocity = new Vector2 (rb.velocity.x, 0);
        lastGroundedTime = Time.time;
        transform.Translate (Vector3.up * step);
    }

    private float CalulateStep () {
        stepRay.Raycast (groundLayer, out RaycastHit2D hit);
        if (Physics2D.Raycast (stepRay.start.position + Vector3.down * (hit.distance - 0.01f), Vector2.left * direction, 0.1f, groundLayer).collider != null) {
            return 0;
        }
        return stepRay.distance - hit.distance;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (((groundLayer >> collision.gameObject.layer) & 1) == 1) {
            Instantiate(groundParticle, particleOrigin.transform.position, Quaternion.identity);
        }
    }
}
