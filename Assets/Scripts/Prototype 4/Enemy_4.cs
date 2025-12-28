using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_4 : MonoBehaviour
{
    public float speed = 3.0f;
    private GameObject player;
    private Rigidbody enemyRb;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemyRb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // vector subtraction to get direction towards player
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        // normalize distance so far distance does not give large number
        enemyRb.AddForce(lookDirection * speed);

        // destroy enemy falls below platform
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
