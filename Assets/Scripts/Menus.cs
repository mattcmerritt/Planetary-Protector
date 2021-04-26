using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    // Each of the planets is a UI image with a world canvas 
    // that contains a countdown textbox
    public GameObject[] Planets;
    
    // This will be the index of the scene to load, also can be used as an index
    public int SelectedLevel; 

    // Time data to keep track of how long the planet has been selected
    private Timer CountdownTimer;
    private int Countdown;

    // Beep sounds for countdown
    public AudioSource Countdown1, Countdown2;

    // The textboxes to display the countdown.
    // This array is parallel to Planets.
    public TMP_Text[] CountdownTextboxes;

    // Create the countdown timer and start the countdown at 3.
    // Also clears all countdown textboxes.
    private void Start()
    {
        CountdownTimer = new Timer(0.5);
        Countdown = 3;

        foreach (TMP_Text textbox in CountdownTextboxes)
        {
            textbox.text = "";
        }
    }

    private void Update()
    {
        for (int i = 0; i < Planets.Length; i++)
        {
            RectTransform rt = Planets[i].GetComponent<RectTransform>();
            Vector2 mousePosition;

            // convert mouse location to the transforms coordinates, saves it to mousePosition
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, Camera.main, out mousePosition);
            
            // manually get bounds of transform
            float width = RectTransformUtility.CalculateRelativeRectTransformBounds(rt).extents.x;
            float height = RectTransformUtility.CalculateRelativeRectTransformBounds(rt).extents.y;
            
            // if in bounds of a planet
            if (Mathf.Abs(mousePosition.x) <= width && Mathf.Abs(mousePosition.y) <= height)
            {
                if (SelectedLevel == 0)
                {
                    SelectedLevel = i + 1; // need to count level select scene
                }
            }
            // else the mouse is not on the current planet
            else {
                // make sure this was not supposed to be the selected planet. 
                // if it was, deselect and reset countdown.
                if (i == SelectedLevel - 1)
                {
                    SelectedLevel = 0;
                    CountdownTextboxes[i].text = "";
                    CountdownTimer.Stop();
                    Countdown = 3;
                }
            }

            // not really sure why these didn't work, something was happening with the Bounds.Contains(Vector3) method
            //Debug.Log(RectTransformUtility.CalculateRelativeRectTransformBounds(rt).Contains(new Vector3(mousePosition.x, mousePosition.y, 0f)));
            //Debug.Log(RectTransformUtility.CalculateRelativeRectTransformBounds(rt).Contains(mousePosition));

        }

        // if a planet is selected, check for passage of time
        // otherwise, reset the countdown and stop the timer
        if (SelectedLevel != 0)
        {
            // updates the time if it is running
            // otherwise, starts a timer and countdown
            if (CountdownTimer.IsRunning())
            {
                CountdownTimer.IncrementTime(Time.deltaTime);
            }
            else
            {
                /*
                // old method of updating labels, was excessive
                TMP_Text[] textboxes = Planets[i].GetComponentsInChildren<TMP_Text>();
                foreach (TMP_Text textbox in textboxes)
                {
                    if (textbox.name == "Countdown")
                    {
                        textbox.text = "" + Countdown;
                    }
                }
                */

                // update countdown label 
                CountdownTextboxes[SelectedLevel - 1].text = "" + Countdown;

                CountdownTimer.Start();
                Countdown1.Play();
            }

            // check if a second has passed.
            // if yes, countdown and update the right textbox to show
            // also, play a sound based on how much time is left
            if (CountdownTimer.TimerFinished())
            {
                Countdown--;

                /*
                // old method of updating labels, was excessive
                TMP_Text[] textboxes = Planets[i].GetComponentsInChildren<TMP_Text>();
                foreach (TMP_Text textbox in textboxes)
                {
                    if (textbox.name == "Countdown")
                    {
                        textbox.text = "" + Countdown;
                    }
                }
                */

                // update countdown label
                CountdownTextboxes[SelectedLevel - 1].text = "" + Countdown;
                
                if (Countdown == 0)
                {
                    Countdown2.Play();
                    SceneManager.LoadScene(SelectedLevel);
                }
                else
                {
                    Countdown1.Play();
                }
            }
        }
        else 
        {
            // reset countdown
            Countdown = 3;
            CountdownTimer.Stop();
        }
    }

}
