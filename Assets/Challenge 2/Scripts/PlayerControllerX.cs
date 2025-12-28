using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    public float dogDelay = 1f;
    private float lastDog = 0f;

    // Update is called once per frame
    void Update()
    {
        // On spacebar press, send dog
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (Time.time - lastDog >= dogDelay)
            {
                GameObject dog = Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
                dog.transform.parent = transform;

                lastDog = Time.time;
            }
        }
    }
}
