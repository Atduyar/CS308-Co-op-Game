using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 1000.0f;
    public float jump = 1000.0f;
    public float limit = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX /= 1.005f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForceX(-speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForceX(speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.linearVelocityY = jump;
        }

        if (rb.linearVelocityX > limit)
        {
            rb.linearVelocityX = limit;
        }
        else if(rb.linearVelocityX < -limit)
        {
            rb.linearVelocityX = -limit;
        }
        //rb.AddForce(new Vector2(1,0));
    }
}
