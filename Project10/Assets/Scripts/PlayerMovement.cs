using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 80f;
    [SerializeField] float sprintMultiplier = 1.5f;
    [SerializeField] float jumpForce = 40f;
    [SerializeField] float maxJumpHoldTime = 0.5f; // Adjusted to 0.5 seconds for a higher jump
    [SerializeField] Transform groundCheckTransform; // Transform to check if the player is grounded
    [SerializeField] LayerMask groundMask; // Mask to define what is considered ground
    [SerializeField] float mouseSensitivity = 100f; // Sensitivity for mouse movement
    [SerializeField] Transform playerCamera; // Reference to the player's camera

    private Rigidbody rb;
    private bool isGrounded;
    private float jumpHoldTime = 0f;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen

        // Freeze rotation to keep the player upright
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        MovePlayer();
        GroundCheck();
        JumpProcess();
        MouseLook();
    }

    private void MovePlayer()
    {
        float currentSpeed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * xInput + transform.forward * zInput).normalized;
        Vector3 targetPosition = transform.position + move * currentSpeed * Time.deltaTime;

        rb.MovePosition(targetPosition);
    }

    private void GroundCheck()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, 0.1f, groundMask);
        Debug.Log("Is Grounded: " + isGrounded); // Debug log
    }

    private void JumpProcess()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpHoldTime = 0f; // Reset jump hold time when jump starts
            Debug.Log("Jump started");
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            jumpHoldTime += Time.deltaTime;

            if (jumpHoldTime <= maxJumpHoldTime)
            {
                // Calculate adjusted jump force
                float adjustedJumpForce = jumpForce * (1 + (jumpHoldTime / maxJumpHoldTime));
                Debug.Log("Applying jump force: " + adjustedJumpForce);
                rb.AddForce(Vector3.up * adjustedJumpForce, ForceMode.Impulse);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpHoldTime = 0f;
        }
    }

    private void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically and clamp the rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
