using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5.0f;
    public float jumpPower = 10.0f;
    public float limit = 5.0f;
    
    private Vector2 movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        //rb.linearVelocityX /= 1.005f;
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    rb.AddForceX(-speed * Time.deltaTime);
        //}
        //else if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    rb.AddForceX(speed * Time.deltaTime);
        //}

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    rb.linearVelocityY = jump;
        //}

        //if (rb.linearVelocityX > limit)
        //{
        //    rb.linearVelocityX = limit;
        //}
        //else if(rb.linearVelocityX < -limit)
        //{
        //    rb.linearVelocityX = -limit;
        //}
        //rb.AddForce(new Vector2(1,0));
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        Debug.Log("Move");
        movement.x = ctx.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Jump");
            rb.linearVelocityY = jumpPower;
        }
        else if(ctx.canceled)
        {
            Debug.Log("Jump Canceled");
            rb.linearVelocityY *= 0.5f;
        }
    }
}
