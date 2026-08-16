using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 moveInput;
    private Rigidbody rb;
    private PlayerInput playerInput;

    [Header("Movement")]
    public float speed = 5f;
    public float SpeedMultiplier;

    [Header("Dash")]
    [Tooltip("How fast the player moves during a dash.")]
    public float dashSpeed = 20f;

    [Tooltip("How long the dash burst lasts, in seconds.")]
    public float dashDuration = 0.15f;

    [Tooltip("Time between dashes, in seconds.")]
    public float dashCooldown = 1.5f;
    private bool isDashing;
    private float dashTimeRemaining;
    private float dashCooldownRemaining;
    private Vector3 dashDirection;

    [Tooltip("Layer other players are on. Used to detect nearby flag carriers to steal from.")]
    public LayerMask PlayerLayer;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }
    void Start()
    {
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        playerInput = GetComponent<PlayerInput>();
    }
    // MOVEMENT
    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector3(input.x, 0f, input.y);
    }
    void Update()
    {
        if (dashCooldownRemaining > 0f)
        {
            dashCooldownRemaining -= Time.deltaTime;
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (isDashing || dashCooldownRemaining > 0f) return;
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.z);
        dashDirection = inputDir.sqrMagnitude > 0.001f ? inputDir.normalized : transform.forward;
        isDashing = true;
        dashTimeRemaining = dashDuration;
        dashCooldownRemaining = dashCooldown;
    }
    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            dashTimeRemaining -= Time.fixedDeltaTime;
            if (dashTimeRemaining <= 0f)
            {
                isDashing = false;
            }
            return;
        }
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.z);
        rb.MovePosition(rb.position + inputDir * speed * Time.fixedDeltaTime);
        if (inputDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                15f * Time.fixedDeltaTime
            );
        }
    }
}