using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Timer DurationTimer;
    private const double Duration = 0.1;
    
    private void Start() 
    {
        DurationTimer = new Timer(Duration);
        DurationTimer.Start();
    }

    private void Update() 
    {
        DurationTimer.IncrementTime(Time.deltaTime);
        if (DurationTimer.TimerFinished()) 
        {
            Destroy(gameObject);
        }
    }
}
