using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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

    public float Spike = 0.65f;
    public float NoMeatOffset = 0.35f;
    public float SeparatorOffset = 0.55f;

    public float UniformObstacleScale = 0.6f;

    public int ObstacleLineSpawned = 0;

    private int topMeatCount;
    private const int MAX_MEAT_PERLINE = 5;

    public Queue<GameObject> PreviousSpawnedObstacles = new Queue<GameObject>();

    void Start()
    {
        topMeatCount = Random.Range(0, 6);
        Vector3 startVector = new Vector3(ObstacleStartXPos, 0, 0);
        ObstacleLineSpawned = 0;
        SpawnObstacleAt(startVector);
    }

    void Update()
    {

        if (PlayerPosition.transform.position.x > ObstacleVerticalSpacing * ObstacleLineSpawned)
        {
            topMeatCount = Random.Range(0, 6);
            Vector3 ObstacleVector = new Vector3(ObstacleStartXPos + ObstacleVerticalSpacing * ObstacleLineSpawned, 0, 0);
            SpawnObstacleAt(ObstacleVector);
        }

        if (ObstacleLineSpawned > 1) //TODO: change 1 to score
        {
            if (PlayerPosition.transform.position.x > ObstacleVerticalSpacing * ObstacleLineSpawned)
            {
                DestroyAndDeQueue();
            }
        }
        

    }

    void SpawnObstacleAt(Vector3 pos)
    {
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

        topPos -= new Vector3(0, Spike, 0);
        GameObject objSpike = Instantiate(Stick, topPos , Quaternion.identity);
        objSpike.GetComponent<SpriteRenderer>().flipY = true;
        PreviousSpawnedObstacles.Enqueue(objSpike);

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

        bottomPos += new Vector3(0, Spike, 0);
        PreviousSpawnedObstacles.Enqueue(Instantiate(Stick, bottomPos, Quaternion.identity));


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
        foreach (var obj in PreviousSpawnedObstacles)
        {
            Destroy(PreviousSpawnedObstacles.Dequeue());
        }
    }

}


