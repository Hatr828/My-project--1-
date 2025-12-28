using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft_3 : MonoBehaviour
{
    private float speed = 30.0f;
    private PlayerController_3 playerControllerScript;
    private float leftBound = -15;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController_3>();
    }

    // Update is called once per frame
    void Update()
    {
        // obstacle continues moving left if not game over
        if (playerControllerScript.gameOver == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        
        // destroy obstacle out of bound
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
