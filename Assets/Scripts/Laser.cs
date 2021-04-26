using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // variables to show laser for brief moment
    private Timer DurationTimer;
    private const double Duration = 0.1;
    
    // As soon as the Laser is created, its timer begins
    private void Start() 
    {
        DurationTimer = new Timer(Duration);
        DurationTimer.Start();
    }

    // Incrementing the timer until it goes off, meaning that the laser
    // has expired
    private void Update() 
    {
        DurationTimer.IncrementTime(Time.deltaTime);
        if (DurationTimer.TimerFinished()) 
        {
            Destroy(gameObject);
        }
    }
}
