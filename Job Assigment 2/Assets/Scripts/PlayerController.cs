using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D theRB;

    public float jumpForce;
    public bool onGround;
    public float jumpCount;
    private Rigidbody2D rb;
    IEnumerator dashCoroutine;
    bool isDashing;
    bool canDash = true;
    float direction = 1;
    float horizontal;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = 2;
    }

    
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (horizontal !=0)
        {
            direction = horizontal;
        }
        theRB.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), theRB.velocity.y);

        Vector3 charScale = transform.localScale;
        if(Input.GetAxis("Horizontal") < 0)
        {
            charScale.x = -1;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            charScale.x = 1;
        }
        transform.localScale = charScale;


        if (Input.GetKeyDown(KeyCode.Space) && (onGround == true || jumpCount > 0))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash == true)
        {
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            dashCoroutine = dash(.1f, 0.5f);
            StartCoroutine(dashCoroutine);
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.AddForce(new Vector2(0, -7.5f), ForceMode2D.Impulse);
        }
    }
    void Jump()
    {
        theRB.AddForce(Vector2.up * jumpForce * 100);
        onGround = false;
        jumpCount -= 1; 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            onGround = true;
            jumpCount = 2;
        }
    }
    IEnumerator dash(float dashDuration, float dashCooldown)
    {
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }
}
