using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpingPower;

    private Attractor attractor;
    private float horizontal;
    bool magneticFieldIsActive = false;

    private void Awake()
    {
        attractor = GetComponent<Attractor>();
    }

    private void Update()
    {
        ManageMovement();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void MagneticFieldActivation(InputAction.CallbackContext context)
    {
        if (context.performed && !magneticFieldIsActive)
        {
            attractor.isAttracting = true;
            magneticFieldIsActive = true;
        }
        else if (context.performed && magneticFieldIsActive)
        {
            attractor.isAttracting = false;
            magneticFieldIsActive = false;
        }
    }

    private void ManageMovement()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
}
