using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private float leftBoundary; // Position which destroys obstacles (Reduces lag and unwanted gameobjects)

    private void Start()
    {
        // Sets left boundary off-screen
        leftBoundary = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    // Uses gamespeed to move obstacles and destroys them when off-screen
    private void Update()
    {
        transform.Translate(Vector3.left * GameManager.Instance.gameSpeed * Time.deltaTime);
        if (transform.position.x < leftBoundary)
            Destroy(gameObject);
    }
}
