using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    private double Interval, ElapsedTime;
    private bool TimerUp, IsActive;

    public Timer(double interval) 
    {
        Interval = interval;
    }

    public void IncrementTime(double delta)
    {
        if (IsActive)
        {
            ElapsedTime += delta;
            if (ElapsedTime >= Interval) 
            {
                TimerUp = true;
            }
        }
    }

    public void Start() 
    {
        ElapsedTime = 0;
        TimerUp = false;
        IsActive = true;
    }

    public bool TimerFinished() 
    {
        if (TimerUp) 
        {
            Start(); // restart the timer
            return true;
        } 
        else 
        {
            return false;
        }
    }

}
