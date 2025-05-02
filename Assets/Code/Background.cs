using UnityEngine;

public class Background : MonoBehaviour
{
    private float startPos, lenght;
    public GameObject cam;
    public float parallaxEffect;
    public float yOffset;
    void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, cam.transform.position.y + yOffset, transform.position.z);

        if (movement > startPos + lenght)
        {
            startPos += lenght;
        }
        else if (movement < startPos - lenght)
        {
            startPos -= lenght;
        }
    }
}
