using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Timer is a utility that allows us to track many events that are
// supposed to occur after a set amount of time. 
// It was used for projectile, laser, and movement intervals, as well as
// the countdowns.

// In order to work, you must call IncrementTime() with Time.deltaTime in 
// the Update method of a MonoBehaviour.

// Time is kept in seconds.

public class Timer 
{
    private double Interval, ElapsedTime;
    private bool TimerUp, IsActive;

    // Creates a new timer that will go off after the given interval passes.
    public Timer(double interval) 
    {
        Interval = interval;
    }

    // Method to update the duration of time that the timer has been running for.
    // If the time reaches the threshold, the timer "goes off," changing TimerUp to true.
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

    // Method to start or reset a timer. The interval stays the same.
    public void Start() 
    {
        ElapsedTime = 0;
        TimerUp = false;
        IsActive = true;
    }

    // This method should be called regularly in order to check if the timer has gone off.
    // If the timer goes off, it will restart before returning true.
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

    // Method to check if a timer is active.
    // Used to prevent unintentional resetting.
    public bool IsRunning()
    {
        return IsActive;
    }

    // Stops a timer and clears the current time.
    // The timer must be started again to be useful.
    public void Stop()
    {
        ElapsedTime = 0;
        TimerUp = false;
        IsActive = false;
    }

}
