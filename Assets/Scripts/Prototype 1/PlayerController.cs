using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // SerializeField allows variable being displayed in Unity inspector window
    [SerializeField] private float speed;
    [SerializeField] private float horsePower = 200.0f;
    [SerializeField] private float steerPower = 30.0f;
    [SerializeField] private float rpm;
    private const float turnSpeed = 45.0f;
    private float horizontalInput; 
    private float verticalInput;
    private Rigidbody playerRb;
    [SerializeField] GameObject centerOfMass;
    [SerializeField] TextMeshProUGUI speedometerText;
    [SerializeField] TextMeshProUGUI rpmText;
    [SerializeField] List<WheelCollider> allWheels;
    [SerializeField] int wheelsOnGround;
    private bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        // set center of mass for RigidBody to prevent vehcle from flipping
        playerRb.centerOfMass = centerOfMass.transform.localPosition;
    }

    // Called before game starts, script instance is loaded, or previously inactive GameObject set to active
    //private void Awake()
    //{
    //    
    //}

    // Update is called once per frame, framerate-dependent, sense of running slow when framerate drops
    // Fixed Update can be called zero, once or more per frame (interval is determined by the physics timestep setings)
    void FixedUpdate()
    {
        if (!isGameOver && transform.position.y < -10f)
        {
            isGameOver = true;
            PrototypeHUD.ShowGameOver();
        }

        if (isGameOver)
        {
            return;
        }

        // get keyboard input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // vehicle movement control if all wheels on ground
        if (IsOnGround())
        {
            // Move the car forward and backward based on vertical input
            //transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
            //playerRb.AddRelativeForce(Vector3.forward * horsePower * verticalInput);  // apply force in local coordinates

            // Turn the vehicle based on horizontal input
            //transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

            // updated move vehicle forward and backward
            foreach (WheelCollider wheel in allWheels)
            {
                wheel.motorTorque = verticalInput * horsePower;
            }

            // updated turn vehicle left and right
            for (int i = 0; i < allWheels.Count; i++)
            {
                if (i < 2)
                {
                    allWheels[i].steerAngle = horizontalInput * steerPower;
                }
            }

            // display speed
            speed = Mathf.RoundToInt(playerRb.linearVelocity.magnitude * 3.6f);  // km/h
            speedometerText.SetText("Speed: " + speed + " km/h");

            // display RPM
            rpm = (speed % 30) * 40;
            rpmText.SetText("RPM: " + rpm);
        }
        
    }

    // check all wheels on ground
    bool IsOnGround()
    {
        wheelsOnGround = 0;
        foreach (WheelCollider wheel in allWheels)
        {
            if (wheel.isGrounded)
            {
                wheelsOnGround++;
            }
        }
        return wheelsOnGround == 4;
    }
}
