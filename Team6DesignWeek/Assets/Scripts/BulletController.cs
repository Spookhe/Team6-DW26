using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public static BulletController Instance { get; private set; }

    public GameObject bullet; //assign bullet prefab here in unity
    private GameObject newBullet;

    private float leftBoundary; // Use for jump bullet destruction
    private float rightBoundary; // Incase there are no obstacles to destroy bullet


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Sets left boundary off-screen
        leftBoundary = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
        rightBoundary = Camera.main.ScreenToWorldPoint(Vector3.zero).x + 6f;
    }
    void Update()
    {
       if (newBullet != null)
       {
           newBullet.transform.Translate(Vector3.right * GameManager.Instance.gameSpeed * Time.deltaTime);
       }
    }
    public void bulletCreator()
    {
        if(bullet == null)
        {
            Debug.LogError("BulletMissing");
            return;
        }
        newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Debug.Log(bullet);
    }
}
