using UnityEngine;
 
public class RespawnController : MonoBehaviour
{
    public GameObject chekpoint;
    private Transform tran;
    private Rigidbody2D rb;
    public AudioClip deadSound;
    
    private void Start()
    {
        tran = this.GetComponent<Transform>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chekpoint"))
        {
            TakeChekpoint(collision.gameObject);
        }
        else if (collision.CompareTag("DeadlyByTouch"))
        {
            Respawn();
        }
    }

    public void TakeChekpoint(GameObject cp)
    {
        chekpoint = cp;
    }

    public void Respawn()
    {
        AudioSource.PlayClipAtPoint(deadSound, transform.position);
        Vector3 dbtPos = chekpoint.transform.position;
        transform.position = new Vector3(dbtPos.x, dbtPos.y, transform.position.z);
        rb.linearVelocity = Vector2.zero;
    }
}