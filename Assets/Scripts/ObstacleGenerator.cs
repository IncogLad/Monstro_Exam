using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public List<GameObject> ObstacleStackSeparator = new List<GameObject>();
    public GameObject Meat;
    public GameObject Stick;

    public GameObject PlayerPosition;

    public float ObstacleStartXPos = 10f;
    public float ObstacleVerticalSpacing = 8f;

    public float GroundStartYPos = -3.25f;
    public float CeilStartYPos = 5.55f;

    public float NoMeatOffset = 0.35f;
    public float SeparatorOffset = 0.55f;
    public float ObstacleWidthOffset = 0.65f;

    public float UniformObstacleScale = 0.6f;

    public float NextDestinationtoReach = 10f;

    public int ObstacleLineSpawned = 0;

    private int topMeatCount;
    private const int MAX_MEAT_PERLINE = 5;

    public Queue<GameObject> PreviousSpawnedObstacles = new Queue<GameObject>();
    public List<ObstacleBoxes> ObstacleBoxesList = new List<ObstacleBoxes>();

    public struct ObstacleBoxes
    {
        public BoundingBox topBox;
        public BoundingBox bottomBox;
    }

    public struct BoundingBox
    {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }


    void Start()
    {
        topMeatCount = Random.Range(0, 6);
        Vector3 startVector = new Vector3(ObstacleStartXPos, 0, 0);
        ObstacleLineSpawned = 0;
        NextDestinationtoReach = 10f;
        SpawnObstacleAt(startVector);
    }

    void Update()
    {

        if (PlayerPosition.transform.position.x > ObstacleVerticalSpacing * ObstacleLineSpawned)
        {
            topMeatCount = Random.Range(0, 6);
            Vector3 ObstacleVector = new Vector3(ObstacleStartXPos + ObstacleVerticalSpacing * ObstacleLineSpawned, 0, 0);
            SpawnObstacleAt(ObstacleVector);

            GameManager.instance.score++;
        }

    }

    void SpawnObstacleAt(Vector3 pos)
    {
        ObstacleBoxes tempBox = new ObstacleBoxes();

        Vector3 topPos = pos + new Vector3(0, CeilStartYPos, 0); 
        //spawn for top
        if (topMeatCount > 0)
        {
            for (int i = 0; i < topMeatCount; i++)
            {
                if (i != 0) topPos -= new Vector3(0, SeparatorOffset, 0);
                PreviousSpawnedObstacles.Enqueue(Instantiate(Meat, topPos, Quaternion.identity));

                topPos -= new Vector3(0, SeparatorOffset, 0);
                GameObject objSeparator = ObstacleStackSeparator[Random.Range(0, 3)];
                PreviousSpawnedObstacles.Enqueue(Instantiate(objSeparator, topPos, Quaternion.identity));
            }
        }
        else
        {
            topPos += new Vector3(0, NoMeatOffset, 0);
            GameObject objSeparator = ObstacleStackSeparator[Random.Range(0, 3)];
            PreviousSpawnedObstacles.Enqueue(Instantiate(objSeparator, topPos, Quaternion.identity));
        }

        topPos -= new Vector3(0, SeparatorOffset, 0);
        GameObject objSpike = Instantiate(Stick, topPos , Quaternion.identity);
        objSpike.GetComponent<SpriteRenderer>().flipY = true;
        PreviousSpawnedObstacles.Enqueue(objSpike);

        float totalTopBoxTopEdgeY = CeilStartYPos;
        float totalTopBoxBottomEdgeY = topPos.y ;
        float totalTopBoxLeftEdgeX = pos.x - ObstacleWidthOffset;
        float totalTopBoxRightEdgeX = pos.x + ObstacleWidthOffset;

        tempBox.topBox.topLeft = new Vector2(totalTopBoxLeftEdgeX, totalTopBoxTopEdgeY);
        tempBox.topBox.topRight = new Vector2(totalTopBoxRightEdgeX, totalTopBoxTopEdgeY);
        tempBox.topBox.bottomLeft = new Vector2(totalTopBoxLeftEdgeX, totalTopBoxBottomEdgeY);
        tempBox.topBox.bottomRight = new Vector2(totalTopBoxRightEdgeX, totalTopBoxBottomEdgeY);

        //spawn for bottom
        Vector3 bottomPos = pos + new Vector3(0, GroundStartYPos, 0); ;
        int bottomMeatCount = MAX_MEAT_PERLINE - topMeatCount;
        if (bottomMeatCount > 0)
        {
            for (int i = 0; i < bottomMeatCount; i++)
            {
                if (i != 0) bottomPos += new Vector3(0, SeparatorOffset, 0);
                PreviousSpawnedObstacles.Enqueue(Instantiate(Meat, bottomPos, Quaternion.identity));

                bottomPos += new Vector3(0, SeparatorOffset, 0);
                GameObject objSeparator = ObstacleStackSeparator[Random.Range(0, 3)];
                PreviousSpawnedObstacles.Enqueue(Instantiate(objSeparator, bottomPos, Quaternion.identity));
            }
        }
        else
        {
            bottomPos -= new Vector3(0, NoMeatOffset, 0);
            GameObject objSeparator = ObstacleStackSeparator[Random.Range(0, 3)];
            PreviousSpawnedObstacles.Enqueue(Instantiate(objSeparator, bottomPos, Quaternion.identity));
        }

        bottomPos += new Vector3(0, SeparatorOffset, 0);
        PreviousSpawnedObstacles.Enqueue(Instantiate(Stick, bottomPos, Quaternion.identity));

        float totalBottomBoxBottomEdgeY = GroundStartYPos;
        float totalBottomBoxTopEdgeY = bottomPos.y ;
        float totalBottomBoxLeftEdgeX = pos.x - ObstacleWidthOffset;
        float totalBottomBoxRightEdgeX = pos.x + ObstacleWidthOffset;

        tempBox.bottomBox.topLeft = new Vector2(totalBottomBoxLeftEdgeX, totalBottomBoxTopEdgeY);
        tempBox.bottomBox.topRight = new Vector2(totalBottomBoxRightEdgeX, totalBottomBoxTopEdgeY);
        tempBox.bottomBox.bottomLeft = new Vector2(totalBottomBoxLeftEdgeX, totalBottomBoxBottomEdgeY);
        tempBox.bottomBox.bottomRight = new Vector2(totalBottomBoxRightEdgeX, totalBottomBoxBottomEdgeY);

        ObstacleBoxesList.Add(tempBox);

        ObstacleLineSpawned++;
    }

    void DestroyAndDeQueue()
    {
        for (int i = 0; i < 12; i++)
        {
            Destroy(PreviousSpawnedObstacles.Dequeue());
        }

    }

    void DestroyAndDeQueueAll()
    {
        //Debug.Log(PreviousSpawnedObstacles.Count);

        int total = PreviousSpawnedObstacles.Count;

        for (int i = 0; i < total; i++)
        {
            Destroy(PreviousSpawnedObstacles.Dequeue());
            //Debug.Log(i);
        }

    }

    public void ObstacleReset()
    {
        DestroyAndDeQueueAll();
        topMeatCount = Random.Range(0, 6);
        Vector3 startVector = new Vector3(ObstacleStartXPos, 0, 0);
        ObstacleLineSpawned = 0;
        NextDestinationtoReach = 10f;
        SpawnObstacleAt(startVector);
    }

}


