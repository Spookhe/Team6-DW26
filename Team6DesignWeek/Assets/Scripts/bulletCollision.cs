using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (other.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}
