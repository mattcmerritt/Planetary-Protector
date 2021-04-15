using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private double interval, elapsedTime;
    private bool timerUp, isActive;

    public Timer(double interval) 
    {
        this.interval = interval;
    }

    private void Update()
    {
        if (isActive)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= interval) 
            {
                timerUp = true;
            }
        }
    }

    public void Start() 
    {
        elapsedTime = 0;
        timerUp = false;
    }

    public bool TimerFinished() 
    {
        if (timerUp) 
        {
            timerUp = false;
            Start(); // restart the timer
            return true;
        } 
        else 
        {
            return false;
        }
    }

}
