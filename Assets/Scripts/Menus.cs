using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    public GameObject[] Planets;
    public int SelectedLevel;
    private Timer CountdownTimer;
    private int Countdown;
    public AudioSource Countdown1, Countdown2;
    public TMP_Text[] CountdownTextboxes;

    private void Start()
    {
        CountdownTimer = new Timer(1);
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
            else {
                // make sure this was not the selected planet. If it is, deselect
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

        if (SelectedLevel != 0)
        {
            if (CountdownTimer.IsRunning())
            {
                CountdownTimer.IncrementTime(Time.deltaTime);
            }
            else
            {
                // update countdown label
                /*
                TMP_Text[] textboxes = Planets[i].GetComponentsInChildren<TMP_Text>();
                foreach (TMP_Text textbox in textboxes)
                {
                    if (textbox.name == "Countdown")
                    {
                        textbox.text = "" + Countdown;
                    }
                }
                */
                CountdownTextboxes[SelectedLevel - 1].text = "" + Countdown;

                CountdownTimer.Start();
                Countdown1.Play();
            }

            if (CountdownTimer.TimerFinished())
            {
                Countdown--;

                // update countdown label
                /*
                TMP_Text[] textboxes = Planets[i].GetComponentsInChildren<TMP_Text>();
                foreach (TMP_Text textbox in textboxes)
                {
                    if (textbox.name == "Countdown")
                    {
                        textbox.text = "" + Countdown;
                    }
                }
                */
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
            // hide countdown label
            /*
            TMP_Text[] textboxes = Planets[i].GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text textbox in textboxes)
            {
                if (textbox.name == "Countdown")
                {
                    textbox.text = "";
                }
            }
            */
            // reset countdown
            Countdown = 3;
            CountdownTimer.Stop();
        }
    }

}
