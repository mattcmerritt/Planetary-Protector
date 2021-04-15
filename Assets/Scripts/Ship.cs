using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private const double ShotInterval = 2;
    private Timer ShotTimer;
    public GameObject LaserPrefab;

    private void Start()
    {
        ShotTimer = new Timer(ShotInterval); // fires a shot every INTERVAL
        ShotTimer.Start();
    }

    private void Update()
    {
        ShotTimer.IncrementTime(Time.deltaTime);
        if (ShotTimer.TimerFinished())
        {
            FireShot();
        }
    }

    public void FireShot() 
    {
        //Debug.Log("Fired a laser!");
        Instantiate(LaserPrefab, transform.position, Quaternion.identity);
    }
}
