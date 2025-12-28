using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private GameManager gameManager;
    private float minSpeed = 12.0f;
    private float maxSpeed = 16.0f;
    private float maxTorque = 10.0f;
    private float xRange = 4.0f;
    private float ySpawnPos = -2.0f;

    public ParticleSystem explosionParticle;
    public int pointValue;

    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // toss the target object upward
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);

        // apply torque to rotate target object
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

        // spawn target at random position underneath the scene
        transform.position = RandomSpawnPos();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // mouse click event
    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            // destroy target if mouse cursor in box collider and is clicked
            Destroy(gameObject);

            // display explosion when target is destroyed
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);

            gameManager.UpdateScore(pointValue);
        }
        
    }

    // sensor
    private void OnTriggerEnter(Collider other)
    {
        // destroy target below sensor which has box collider applied to it
        Destroy(gameObject);

        // game over when good target drops below sensor
        if (!gameObject.CompareTag("Bad"))
        {   
            gameManager.GameOver();
        }
        
    }


    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

}
