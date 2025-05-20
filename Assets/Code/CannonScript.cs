using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public float range;
    public GameObject bullet;
    public float firerate;
    public float bulletLifeTime = 5f;
    public bool flip = false;
    float nextTimeToFire = 0;
    public Transform shootpoint;
    public float force;
    public AudioClip shootSound;

    Animator anim;
    Transform[] players;
    Transform closestPlayer;

    void Start()
    {
        anim = GetComponentsInChildren<Animator>()[0];

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        players = new Transform[playerObjects.Length];

        for (int i = 0; i < playerObjects.Length; i++)
        {
            players[i] = playerObjects[i].transform;
        }

        if (flip)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Update()
    {
        if (players == null || players.Length == 0) return;

        closestPlayer = GetClosestPlayer();
        if (closestPlayer == null) return;


        float distance = Vector2.Distance(transform.position, closestPlayer.position);
        bool playerDetected = distance <= range;

        if (playerDetected && Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1 / firerate;
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        if (anim != null)
        {
            anim.SetTrigger("Fire");
        }

        yield return new WaitForSeconds(1);

        AudioSource.PlayClipAtPoint(shootSound, transform.position);

        Vector2 direction;
        float scaleX;

        if (flip)
        {
            direction = Vector2.right;
            scaleX = -1;
        }
        else 
        {
            direction = Vector2.left;
            scaleX = 1;
        }

        GameObject BulletIns = Instantiate(bullet, shootpoint.position, Quaternion.identity);
        BulletIns.transform.localScale = new Vector3(scaleX, 1, 1);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(direction * force);
        Destroy(BulletIns, bulletLifeTime);
    }

    Transform GetClosestPlayer()
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform p in players)
        {
            float dist = Vector2.Distance(transform.position, p.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = p;
            }
        }

        return nearest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
