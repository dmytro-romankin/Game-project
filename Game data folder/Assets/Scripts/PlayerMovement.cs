using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private float HorizontalMove = 0f;
    private bool FacingRight = true;

    [Header("Player Movement Settings")]
    [Range(0, 10f)] public float speed = 1f;
    [Range(0, 30f)] public float jumpForce = 8f;

    [Header("Player Animator Settings")]
    public Animator animator;

    [Space]
    [Header("Ground Checker Settings")]
    public bool isGrounded = false;
    [Range(-5f, 5f)] public float checkGroundOffsetY = 0f;
    [Range(0, 5f)] public float checkGroundRadius = 0.3f;

    public Transform GroundCheck;
    public LayerMask Ground;
    public VectorValue pos;

    private void Start()
    {
        transform.position = pos.initialValue;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        //Animator
        animator.SetFloat("SpeedAnim", Mathf.Abs(HorizontalMove));

        //Walk
        HorizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        //Fip
        if (HorizontalMove < 0 && FacingRight)
        {
            Flip();
        }
        else if (HorizontalMove > 0 && !FacingRight)
        {
            Flip();
        }

        //GroundChecking
        CheckGround();
    }
    private void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(HorizontalMove * 10f, rb.velocity.y);
        rb.velocity = targetVelocity;
    }

    private void Flip()
    {
        FacingRight = !FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, checkGroundRadius, Ground);
    }
}