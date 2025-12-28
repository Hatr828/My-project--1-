using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_3 : MonoBehaviour
{
    public GameObject obstaclePrefab;
    private Vector3 spawnPos = new Vector3(25, 0, 0);
    private float startDelay = 2.0f;
    private float repeatRate = 2.0f;
    private PlayerController_3 playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        // find using tag, get reference to PlayerController script
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController_3>();
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObstacle()
    {
        // create obstacle object is not game over
        if (playerControllerScript.gameOver == false)
        {
            Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
        }
      
    }
}
