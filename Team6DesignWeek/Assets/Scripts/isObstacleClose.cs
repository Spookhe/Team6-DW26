using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isObstacleClose : MonoBehaviour
{
    public bool isClose;

    public void Start()
    {
        isClose = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            isClose = true;
        }

    }
}
