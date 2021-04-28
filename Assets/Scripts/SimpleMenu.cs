using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// This is basically a single planet version of Menus, without all the arrays
// Also, it can only load the first scene (build index 0)

public class SimpleMenu : MonoBehaviour
{
    // UI image with a world canvas that contains a countdown textbox
    public GameObject Planet;
    
    // Time data to keep track of how long the planet has been selected
    private Timer CountdownTimer;
    private int Countdown;

    // Beep sounds for countdown
    public AudioSource Countdown1, Countdown2;

    // The textbox to display the countdown.
    public TMP_Text CountdownTextbox;

    // Create the countdown timer and start the countdown at 3.
    // Also clears all countdown textboxes.
    private void Start()
    {
        CountdownTimer = new Timer(0.5);
        Countdown = 3;

        CountdownTextbox.text = ""; // clear text
    }

    private void Update()
    {
        RectTransform rt = Planet.GetComponent<RectTransform>();
        Vector2 mousePosition;

        // convert mouse location to the transforms coordinates, saves it to mousePosition
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, Camera.main, out mousePosition);
        
        // manually get bounds of transform
        float width = RectTransformUtility.CalculateRelativeRectTransformBounds(rt).extents.x;
        float height = RectTransformUtility.CalculateRelativeRectTransformBounds(rt).extents.y;
        
        // if in bounds of a planet
        if (Mathf.Abs(mousePosition.x) <= width && Mathf.Abs(mousePosition.y) <= height)
        {
            if (CountdownTimer.IsRunning())
            {
                CountdownTimer.IncrementTime(Time.deltaTime);
            }
            else
            {
                CountdownTimer.Start();
                CountdownTextbox.text = "" + Countdown;
                Countdown1.Play();
            }

            if (CountdownTimer.TimerFinished())
            {
                Countdown--;
                CountdownTextbox.text = "" + Countdown;
                if (Countdown == 0)
                {
                    CountdownTextbox.text = "";
                    Countdown2.Play();
                    SceneManager.LoadScene(0); // level select
                }
                else 
                {
                    Countdown1.Play();
                }
            }
        }
        // else the mouse is not on the current planet
        else {
            CountdownTimer.Stop();
            Countdown = 3;
            CountdownTextbox.text = "";
        }

        // not really sure why these didn't work, something was happening with the Bounds.Contains(Vector3) method
        //Debug.Log(RectTransformUtility.CalculateRelativeRectTransformBounds(rt).Contains(new Vector3(mousePosition.x, mousePosition.y, 0f)));
        //Debug.Log(RectTransformUtility.CalculateRelativeRectTransformBounds(rt).Contains(mousePosition));
    }

}
