using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float grav = -10f;
    [SerializeField] private float jumpForce = 2f;

    private Bounds playerBounds;
    private float boundsEdgeWidth = 0.01f;

    private Transform obstacleTransform;

    private float  verticalSpeed;

    private Vector3 totalVerticalVector;


    void Start()
    {
        playerBounds = GetComponent<SpriteRenderer>().bounds;

    }

    
    void Update()
    {
        UpdateBounds();
        UpdatePlayerPhysics();
        CheckCollision(totalVerticalVector, transform.position);
    }

    void UpdateBounds()
    {
        playerBounds.Expand(-3 * boundsEdgeWidth);
    }

    void UpdatePlayerPhysics()
    {
        verticalSpeed = Mathf.Lerp(verticalSpeed, grav, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            verticalSpeed = jumpForce;
        }

        totalVerticalVector = new Vector3(0, verticalSpeed, 0);
            
        transform.Translate(totalVerticalVector * movementSpeed * Time.deltaTime);
        
    }
    

    void CheckCollision(Vector3 vel, Vector3 pos)
    {
        float dist = vel.magnitude + boundsEdgeWidth;

        RaycastHit hit;
        if (Physics.SphereCast(pos, playerBounds.extents.x, vel.normalized, out hit, dist,
                LayerMask.GetMask("Obstacle")))
        {
            Vector3 snapSurface = vel.normalized * (hit.distance - boundsEdgeWidth);

            if (snapSurface.magnitude <= boundsEdgeWidth)
            {
                snapSurface = Vector3.zero;
            }

            Debug.Log("hit");
        }
    }


}
