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
        FaceCursor();
    }

    public void FireShot() 
    {
        GameObject laser = Instantiate(LaserPrefab, transform.position, Quaternion.identity);
        laser.transform.eulerAngles = transform.eulerAngles;
    }

    public void FaceCursor() 
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float theta = 360 - Mathf.Atan2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y) * 180 / Mathf.PI;
        theta = (theta + 360) % 360;
        transform.eulerAngles = new Vector3(0f, 0f, theta);
    }
}
