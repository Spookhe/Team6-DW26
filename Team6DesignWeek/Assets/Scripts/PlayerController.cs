using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public float superReloadTimer = .25f;
    public bool hasAmmo;
    [SerializeField] private TextMeshProUGUI ammoText;
    //for finding bullet
    public GameObject bullet;

    //for flipping sprite
    SpriteRenderer sr;

    //check if any enemies are in Player radius
    public float detectionRadius = 1f;
    bool isClose = false;
    //use to only detect objects to the right of player
    new Vector3 delay = new Vector3(3,0,0);

    private int lives = 3;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        sr = GetComponent<SpriteRenderer>();
        UpdateAmmo();
        BgAudio.Play(); // Plays background audio
    }

    private void OnEnable()
    {
        velocity = Vector3.zero; // Resets player movement state
    }

    private void Update()
    {
        
        //create collider array to check if any colliders are within range of player transform
         Collider[] hitColliders = Physics.OverlapSphere(transform.position + delay, detectionRadius);
        //create loop checking for any colliders within range of the player
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Obstacle"))
            {
                // Insert your obstacle handling logic here (e.g., stop moving, turn around)
                isClose = true;
                Debug.Log($"is close" + isClose);
            }
            else 
            {
                // Insert your obstacle handling logic here (e.g., stop moving, turn around)
                isClose = false;
                Debug.Log("isClose" + isClose);
            }
        }
        

        if (controller.isGrounded && hasAmmo == true) // If grounded allows the player to jump
        {
            velocity.y = -1f;
            sr.flipX = false; //flip right

            //player is very close to an obstacle
            if(isClose == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    hasAmmo = false;
                    velocity.y = jumpStrength;
                    jumpAudio.Play(); // Plays jump one-shot sound
                    gunAudio.Play(); // Plays shot audio
                    Debug.Log(lives);
                    StartCoroutine(SuperTimer(superReloadTimer));
                }
                if (Input.GetMouseButtonDown(1))
                {
                    BulletController.Instance.BulletCreator();
                    hasAmmo = false;
                    gunAudio.Play(); // Plays shot audio
                    Debug.Log(lives);
                    StartCoroutine(SuperTimer(superReloadTimer));
                }
            }
            
            //player is not within 1f of an obstacle
            if (isClose == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    hasAmmo = false;
                    velocity.y = jumpStrength;
                    jumpAudio.Play(); // Plays jump one-shot sound
                    gunAudio.Play(); // Plays shot audio
                    Debug.Log(lives);
                    StartCoroutine(RegularTimer(reloadTimer));
                }
                if (Input.GetMouseButtonDown(1))
                {
                    BulletController.Instance.BulletCreator();
                    hasAmmo = false;
                    gunAudio.Play(); // Plays shot audio
                    Debug.Log(lives);
                    StartCoroutine(RegularTimer(reloadTimer));
                }
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
        UpdateAmmo();
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

    public void UpdateAmmo()
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

    IEnumerator RegularTimer(float reload)
    {
        Debug.Log("Routine started!");
        yield return new WaitForSeconds(reload); // Pauses execution
        hasAmmo = true;
        reloadAudio.Play(); // Plays damage sound
        Debug.Log("Ammo reloaded after" + reload + " seconds!");
    }

    public IEnumerator SuperTimer(float reload)
    {
        Debug.Log("Routine started!");
        yield return new WaitForSeconds(reload); // Pauses execution
        hasAmmo = true;
        Debug.Log("Ammo reloaded after" + reload + " seconds!");
    }

}
