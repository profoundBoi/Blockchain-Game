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

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        rb.freezeRotation = true;

        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        moveInput = new Vector3(
            input.x,
            0f,
            input.y
        );
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Vector3 movement = moveInput.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
    }

    private void RotatePlayer()
    {
        Vector3 inputDirection = new Vector3(
            moveInput.x,
            0f,
            moveInput.z
        );

        if (inputDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(inputDirection);

            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    15f * Time.fixedDeltaTime
                )
            );
        }
    }
}

