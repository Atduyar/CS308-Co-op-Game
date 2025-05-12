using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public float range;
    public Transform target;
    bool detected = false;
    public GameObject bullet;
    public float firerate;
    float nextTimeToFire = 0;
    public Transform shootpoint;
    public float force;
    
    
    void Update()
    {
        Vector2 targetPos = target.position;
        Vector2 direction = targetPos - (Vector2)transform.position;
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, direction, range);
        if (rayInfo)
        {
            if(rayInfo.collider.gameObject.tag == "Player")
            {
                detected = true;
            }
            else
            {
                detected = false;
            }
        }

        if (detected)
        {
            if(Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1 / firerate;
                shoot();
            }
        }
    }
    void shoot()
    {
        GameObject BulletIns = Instantiate(bullet, shootpoint.position, Quaternion.identity);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}