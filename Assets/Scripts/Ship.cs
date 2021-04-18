using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private const double ShotInterval = 2;
    private Timer ShotTimer;
    private Rigidbody2D ShipRigidbody;
    public GameObject LaserPrefab;
    private const float MinRotationSpeed = 200f, MaxRotationSpeed = 600f, BaseMovementSpeed = 5f, MaxMovementSpeed = 7.5f;
    private bool IsMoving;

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
        ChaseMouse();
    }

    private void Awake()
    {
        ShipRigidbody = GetComponent<Rigidbody2D>();
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

    public void ChaseMouse() 
    {
        // The basis of the math in this method comes from https://www.youtube.com/watch?v=0v_H3oOR0aU.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2) mousePosition - (Vector2) transform.position;

        direction.Normalize();
        float rotationCorrection = Vector3.Cross(direction, transform.up).z;

        // My own math for variable speed and angular adjustment
        Vector2 distanceToMouse = transform.position - mousePosition;
        float movementSpeed, rotationSpeed;

        // if mouse close to ship, stop moving and start adjusting angle quickly. This will stop the ship until the user moves the mouse far away again
        if (distanceToMouse.magnitude < 1f) 
        {
            movementSpeed = 0f;
            rotationSpeed = MaxRotationSpeed;
            IsMoving = false;         
        }
        else 
        {
            // if in motion, give movement speed and slower angular speed
            if (IsMoving) 
            {
                movementSpeed = Mathf.Clamp(BaseMovementSpeed * distanceToMouse.magnitude, -MaxMovementSpeed, MaxMovementSpeed);
                rotationSpeed = MinRotationSpeed;
            }
            else 
            {
                // mouse needs to go farther away to start the movement again
                if (distanceToMouse.magnitude > 1f)
                {
                    IsMoving = true;
                    movementSpeed = Mathf.Clamp(BaseMovementSpeed * distanceToMouse.magnitude, -MaxMovementSpeed, MaxMovementSpeed);
                    rotationSpeed = MinRotationSpeed;
                }
                // if not past the threshold, keep using the fast angle adjustment with no movement
                else 
                {
                    movementSpeed = 0f;
                    rotationSpeed = MaxRotationSpeed;
                }
            }
        }

        ShipRigidbody.angularVelocity = -rotationCorrection * rotationSpeed;
        ShipRigidbody.velocity = transform.up * movementSpeed;
    }
}
