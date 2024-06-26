using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
        if (transform.position.y - playerBounds.extents.y < groundBounds.position.y)
        {
            Debug.Log("hit");
            GameManager.instance.GameOver();
        }

        int start = 0;

        if (GameManager.instance.ObstacleGenerator.ObstacleBoxesList.Count >= 3)
        {
            start = GameManager.instance.ObstacleGenerator.ObstacleBoxesList.Count - 3;
        }
        
        for (int i = start; i < GameManager.instance.ObstacleGenerator.ObstacleBoxesList.Count; i++)
        {
            if (transform.position.y - playerBounds.extents.y <= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].bottomBox.topLeft.y &&
            transform.position.y - playerBounds.extents.y <= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].bottomBox.topRight.y &&
            transform.position.x + playerBounds.extents.x >= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].bottomBox.topLeft.x &&
            transform.position.x + playerBounds.extents.x >= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].bottomBox.bottomLeft.x &&
            transform.position.x - playerBounds.extents.x <= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].bottomBox.topRight.x &&
            transform.position.x - playerBounds.extents.x <= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].bottomBox.bottomRight.x)
            {
                Debug.Log("hit");
                GameManager.instance.GameOver();
            }

            if (transform.position.y - playerBounds.extents.y <= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].topBox.topLeft.y &&
                transform.position.y - playerBounds.extents.y <= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].topBox.topRight.y &&
                transform.position.x + playerBounds.extents.x >= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].topBox.topLeft.x &&
                transform.position.x + playerBounds.extents.x >= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].topBox.bottomLeft.x &&
                transform.position.x - playerBounds.extents.x <= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].topBox.topRight.x &&
                transform.position.x - playerBounds.extents.x <= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].topBox.bottomRight.x &&
                transform.position.y + playerBounds.extents.y >= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].topBox.bottomLeft.y &&
                transform.position.y + playerBounds.extents.y >= GameManager.instance.ObstacleGenerator.ObstacleBoxesList[i].topBox.bottomRight.y)
            {
                Debug.Log("hit");
                GameManager.instance.GameOver();
            }
        }
        

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
