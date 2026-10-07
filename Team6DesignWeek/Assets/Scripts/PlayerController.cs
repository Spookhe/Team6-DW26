using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller; // Handles playermovement and collision
    private Vector3 velocity;               // Vertical movement (up and down)

    public float gravityStrength = 19.62f;  // Strength of gravity that affects player
    public float jumpStrength = 8f;         // Strength jumping applies

    public AudioSource jumpAudio; // Player jump sound effect
    public AudioSource damageAudio; // Player hit/damaged sound effect
    public AudioSource gunAudio; // gunshot sound
    public AudioSource BgAudio; // background sound
    public AudioSource reloadAudio; // reload sound

    //Ammo
    //public float reloadTime = .5f;
    //public float reloadTimer = 0f;
    public float reloadTimer = 1f;
    public float fastReloadTimer = .5f;
    public bool hasAmmo;
    [SerializeField] private TextMeshProUGUI ammoText;
    //for finding bullet
    public GameObject bullet;

    //for flipping sprite
    SpriteRenderer sr;


    private int lives = 3;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        sr = GetComponent<SpriteRenderer>();
        updateAmmo();
        BgAudio.Play(); // Plays background audio
    }

    private void OnEnable()
    {
        velocity = Vector3.zero; // Resets player movement state
    }

    private void Update()
    {

        if (controller.isGrounded && hasAmmo == true) // If grounded allows the player to jump
        {
            velocity.y = -1f;
            sr.flipX = false; //flip right

            if (Input.GetMouseButtonDown(0))
            {
                hasAmmo = false;
                StartCoroutine(regularTimer(reloadTimer));
                velocity.y = jumpStrength;
                jumpAudio.Play(); // Plays jump one-shot sound
                gunAudio.Play(); // Plays shot audio
                Debug.Log(lives);
            }

            if(Input.GetMouseButtonDown(1))
            {
                hasAmmo = false;
                StartCoroutine(regularTimer(reloadTimer));
                BulletController.Instance.bulletCreator();
                gunAudio.Play(); // Plays shot audio

            }
        }
        if (!controller.isGrounded)
        {
            velocity.y -= gravityStrength * Time.deltaTime; // Applies gravity
            sr.flipX = true; //flip left
        }

        controller.Move(velocity * Time.deltaTime); // Applies movement
        Health();
        GameUIManager.Instance.UpdateLives(lives);
        updateAmmo();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Obstacle"))
        {
            damageAudio.Play(); // Plays damage sound
            lives--;
            //velocity = Vector3.zero; // Resets player movement state //lower player/game speed/score?
            //GameManager.Instance.initialSpeed -= 1f;
        }

    }

    public void Health()
    {
        //GameUIManager.Instance.UpdateLives(lives);

        if (lives <= 0)
        {
            GameManager.Instance.GameOver();
        }

    }

    public void updateAmmo()
    {
        int ammoNumber;

        if (hasAmmo == true)
        {
            ammoNumber = 1;
        }
        else
        {
            ammoNumber = 0;
        }
        
        ammoText.text = ammoNumber + "/1";
    }

    IEnumerator regularTimer(float reload)
    {
        Debug.Log("Routine started!");
        yield return new WaitForSeconds(reload); // Pauses execution
        hasAmmo = true;
        reloadAudio.Play(); // Plays damage sound
        Debug.Log("Ammo reloaded after" + reload + " seconds!");
    }

    IEnumerator acceleratedTimer(float reload)
    {
        Debug.Log("Routine started!");
        yield return new WaitForSeconds(reload); // Pauses execution
        hasAmmo = true;
        Debug.Log("Ammo reloaded after" + reload + " seconds!");
    }

}
