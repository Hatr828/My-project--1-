using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_3 : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    public AudioSource playerAudio;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private float jumpForce = 700;
    private float gravityModifier = 1.5f;
    public bool isOnGround = true;
    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        // make player jump when spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && gameOver == false)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;

            // play jump animation
            playerAnim.SetTrigger("Jump_trig");

            // stop dirt particle while jumping
            dirtParticle.Stop();

            // play jump sound
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

    }

    // detect collision
    private void OnCollisionEnter(Collision collision)
    {
        // collide with ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }
        // collide with obstacle
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
            gameOver = true;

            // play death animation
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);

            // play explosion particle
            explosionParticle.Play();

            dirtParticle.Stop();

            // play crash sound
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }
}
