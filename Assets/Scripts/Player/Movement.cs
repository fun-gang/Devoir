using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header ("Movement")]
    private Gameplay controls = null;
    [HideInInspector] public Vector2 movement = Vector2.zero;
    [HideInInspector] public float jumpPressTime = float.NegativeInfinity;
    [HideInInspector] public float jumpStartTime = float.NegativeInfinity;
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
    public float jumpBufferingTime = 0.1f;
    [Tooltip ("Max time beetween input and climb")]
    public float climbBufferingTime = 0.3f;
    public float coyoteTime = 0.05f;
    public float stepHeight = 0.1f;

    [Header ("Collision")]
    public LayerMask groundLayer;
    public BoxRay groundRay;
    public BoxRay climbRay;
    public SegmentRay stepRay;

    [Header ("Effects")]
    public Transform particleOrigin;
    public GameObject groundParticle;

    private float direction = 1;

    // Components
    private Animator anim;
    private Rigidbody2D rb = null;

    public static bool control; // Variable for cutscenes => Turn off/on movement ability

    // Jump
    private bool isGrounded;
    private float lastGroundedTime = float.NegativeInfinity;
    public Sword sword;


    void Awake() {
        controls = new Gameplay();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        control = true;
        movementSpeed = PlayerStats.PlayerSpeed;
    }

    private void FixedUpdate()
    {
        if (control)
        {
            ApplyStep ();
            GroundCheck ();
            TryClimb ();
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
        if (isGrounded && (Time.time - jumpPressTime) < jumpBufferingTime && !sword.isBlock) {
            rb.velocity = Vector2.up * jumpInitialVelocity;
            jumpStartTime = Time.time;
            lastGroundedTime = float.NegativeInfinity;
        }
    }

    private void ApplyJumpSpeed () {
        float progress = 1 - (Time.time - jumpStartTime) / jumpHoldDuration;
        if (progress < 0) return;

        progress = Mathf.Pow (progress, jumpHoldEasing);
        rb.velocity += Vector2.up * (jumpHoldPower * progress * Time.fixedDeltaTime);
    }

    private void MovePlayer () {
        float velocity = rb.velocity.x;

        if (movement.x != 0 && !sword.isBlock) {
            direction = Mathf.Sign (movement.x);
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
        anim.SetBool ("IsMoving", movement.x != 0);
        rb.velocity = new Vector2(velocity, rb.velocity.y);
    }

    private void GroundCheck () {
        if (groundRay.Raycast (groundLayer)) lastGroundedTime = Time.time;
        isGrounded = (Time.time - lastGroundedTime) < coyoteTime;
    }

    void OnDrawGizmos () {
        Gizmos.color = new Color (0.5f, 0.2f, 1f);
        climbRay.Draw ();

        Gizmos.color = new Color (0.2f, 0.5f, 1f);
        groundRay.Draw ();

        Gizmos.color = new Color (0.2f, 1f, 0.2f);
        stepRay.Draw ();
    }

    private void TryClimb () {
        bool isJumpPressed = Time.time - jumpPressTime < climbBufferingTime;
        bool isAwaliable = climbRay.Raycast(groundLayer) && isJumpPressed; //замени на 2 рейкаста

        if (isAwaliable) {
            control = false;
            StartCoroutine (Climb ());
        }
    }

    private IEnumerator Climb() {
        control = false;
        rb.isKinematic = true;

        anim.Play ("PlayerClimb");
        yield return null;
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo (0);
        while (state.IsName ("PlayerClimb") && state.normalizedTime < 1) {
            yield return null;
            state = anim.GetCurrentAnimatorStateInfo (0);
        }

        rb.isKinematic = false;
        control = true;
    }

    void OnAnimatorMove () {
        if (!control) {
            anim.ApplyBuiltinRootMotion ();
        }
    }

    public void ApplyStep () {
        if (rb.velocity.y > 0 || movement.x == 0) return;

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

    void OnMovePerformed(InputAction.CallbackContext value) => movement = value.ReadValue<Vector2>();
    void OnMoveCanceled(InputAction.CallbackContext value) => movement = Vector2.zero;

    void PressJump (InputAction.CallbackContext value) => jumpPressTime = Time.time;
    void ReleaseJump (InputAction.CallbackContext value) => jumpPressTime = jumpStartTime = float.NegativeInfinity;

    void OnEnable() {
        controls.Enable();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Jump.performed += PressJump;
        controls.Player.Jump.canceled += ReleaseJump;
    }

    private void OnDisable() {
        controls.Disable();
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Jump.performed -= PressJump;
        controls.Player.Jump.canceled -= ReleaseJump;
    }
}
