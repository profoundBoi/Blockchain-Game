using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveInput;
    private Rigidbody rb;
    private PlayerInput playerInput;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 15f;

    // Mobile button states
    private bool mobileForward;
    private bool mobileBack;
    private bool mobileLeft;
    private bool mobileRight;

    public static PlayerMovement Instance;

    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    // Keyboard input from Input System
    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector3(input.x, 0f, input.y);
    }

    // Mobile button press/release methods
    public void MobileForwardPress() { mobileForward = true; }
    public void MobileForwardRelease() { mobileForward = false; }
    public void MobileBackPress() { mobileBack = true; }
    public void MobileBackRelease() { mobileBack = false; }
    public void MobileLeftPress() { mobileLeft = true; }
    public void MobileLeftRelease() { mobileLeft = false; }
    public void MobileRightPress() { mobileRight = true; }
    public void MobileRightRelease() { mobileRight = false; }

    void FixedUpdate()
    {
        // Combine keyboard and mobile input
        Vector3 combinedInput = moveInput;

        if (mobileForward) combinedInput += Vector3.forward;
        if (mobileBack) combinedInput += Vector3.back;
        if (mobileLeft) combinedInput += Vector3.left;
        if (mobileRight) combinedInput += Vector3.right;

        combinedInput = Vector3.ClampMagnitude(combinedInput, 1f);

        MovePlayer(combinedInput);
        RotatePlayer(combinedInput);
    }

    private void MovePlayer(Vector3 input)
    {
        Vector3 movement = input.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void RotatePlayer(Vector3 inputDirection)
    {
        if (inputDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            ));
        }
    }
}