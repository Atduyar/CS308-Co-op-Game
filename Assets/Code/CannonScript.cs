using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public float range;
    public GameObject bullet;
    public float firerate;
    float nextTimeToFire = 0;
    public Transform shootpoint;
    public float force;
    public AudioClip shootSound;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        bool playerDetected = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerDetected = true;
                break;
            }
        }

        if (playerDetected && Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1 / firerate;
            shoot();
        }
    }

    void shoot()
    {
        AudioSource.PlayClipAtPoint(shootSound, transform.position);
        GameObject BulletIns = Instantiate(bullet, shootpoint.position, Quaternion.identity);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force);

        if (anim != null)
        {
            anim.SetTrigger("Fire");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
