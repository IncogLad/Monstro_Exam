using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera cam;
    private float startPos;
    private float length;

    [SerializeField] private float lengthOffset = 1f;
    [SerializeField] private float scrollSpeed;
    
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x * lengthOffset;
        cam = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - scrollSpeed);
        float distance = cam.transform.position.x * scrollSpeed;

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }

    }
}
