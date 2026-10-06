using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller; // Handles playermovement and collision
    private Vector3 velocity;               // Vertical movement (up and down)

    public float gravityStrength = 19.62f;  // Strength of gravity that affects player
    public float jumpStrength = 8f;         // Strength jumping applies

    private AudioSource audioSource; // Player jump sound effect

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        velocity = Vector3.zero; // Resets player movement state
    }

    private void Update()
    {
        if (controller.isGrounded) // If grounded allows the player to jump
        {
            velocity.y = -1f;

            if (Input.GetButton("Jump"))
            {
                velocity.y = jumpStrength;
                audioSource.Play(); // Plays jump one-shot sound
            }
        }
        else
        {
            velocity.y -= gravityStrength * Time.deltaTime; // Applies gravity
        }

        controller.Move(velocity * Time.deltaTime); // Applies movement
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ends the game if a player collides with an "obstacle" tag
        if (other.CompareTag("Obstacle"))
            GameManager.Instance.GameOver();
    }
}
