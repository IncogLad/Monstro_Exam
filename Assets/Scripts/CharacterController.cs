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

    [SerializeField] private Transform obstacleTransform;
    [SerializeField] private Transform groundBounds;

    private float  verticalSpeed;

    private Vector3 totalVerticalVector;

    void Start()
    {
        playerBounds = GetComponent<SpriteRenderer>().bounds;

    }

    
    void Update()
    {
        UpdatePlayerPhysics();
        CheckCollision();
    }
    

    void UpdatePlayerPhysics()
    {
        verticalSpeed = Mathf.Lerp(verticalSpeed, grav, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            verticalSpeed = jumpForce;
            if (!GameManager.instance.GameOverUI.activeSelf && GameManager.instance.GameStartUI.activeSelf)
            {
                GameManager.instance.GameStartUI.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }

        totalVerticalVector = new Vector3(0, verticalSpeed, 0);
            
        transform.Translate(totalVerticalVector * movementSpeed * Time.deltaTime);
        
    }
    

    void CheckCollision()
    {
        if (playerBounds.extents.y + transform.position.y < groundBounds.position.y)
        {
            Debug.Log("hit");
            GameManager.instance.GameOver();
        }



        //RaycastHit hit;
        //if (Physics.SphereCast(pos, playerBounds.extents.x, vel, out hit))
        //{
        //    if (hit.collider == null)
        //    {
        //        Debug.Log("hit");
        //    }
            
        //}
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, playerBounds.extents.x);
    }


    public void PlayerReset()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
        transform.position = new Vector3(0, 3, 0);
    }
}
