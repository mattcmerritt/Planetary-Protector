using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    // Laser shot setup
    private const double ShotInterval = 1.5f;
    private Timer ShotTimer;
    public GameObject LaserPrefab;

    // Ship info
    private Rigidbody2D ShipRigidbody;
    private const float MinRotationSpeed = 200f, MaxRotationSpeed = 600f, BaseMovementSpeed = 3f, MaxMovementSpeed = 5f;
    private bool IsMoving;
    
    // startup information
    private Timer StartTimer;
    public bool HasStarted;
    private int StartCountdown;
    private TMP_Text CountdownLabel;
    private AudioSource Countdown1, Countdown2;

    // game state info
    private bool IsAlive;    

    private void Start()
    {
        ShotTimer = new Timer(ShotInterval); // fires a shot every INTERVAL
        ShotTimer.Start();

        // Startup
        StartTimer = new Timer(1);
        HasStarted = false;
        StartCountdown = 3;

        // initial game state
        IsAlive = true;
    }

    private void Update()
    {
        // if the ship is alive, check if the game has not started
        // if not started, wait until the player hovers over the ship for 3 seconds
        // otherwise, have the ship follow the cursor and fire lasers occasionally
        if (IsAlive) 
        {
            if (!HasStarted) 
            {
                // game has not begun, make sure that the mouse is close to ship, then countdown
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 distanceToMouse = transform.position - mousePosition;

                if (distanceToMouse.magnitude <= 1)
                {
                    if (StartTimer.IsRunning())
                    {
                        StartTimer.IncrementTime(Time.deltaTime);
                    }
                    else
                    {
                        StartTimer.Start();
                        CountdownLabel.text = "" + StartCountdown;
                        Countdown1.Play();
                    }

                    if (StartTimer.TimerFinished())
                    {
                        StartCountdown--;
                        CountdownLabel.text = "" + StartCountdown;
                        if (StartCountdown == 0)
                        {
                            HasStarted = true;
                            CountdownLabel.text = "";
                            Countdown2.Play();
                        }
                        else
                        {
                            Countdown1.Play();
                        }
                    }
                }
                else 
                {
                    StartTimer.Stop();
                    StartCountdown = 3;
                    CountdownLabel.text = "";
                }
            }
            else {
                // game has started
                // ship movement and laser firing
                ShotTimer.IncrementTime(Time.deltaTime);
                if (ShotTimer.TimerFinished())
                {
                    FireShot();
                }
                ChaseMouse();
            }

            // check for out of bounds
            if ((transform.position - Vector3.zero).magnitude > 10f)
            {
                Debug.Log("You have gone too far.");
                DestroyShip(); // returns to main menu
            }

            // check to see if all enemies have been defeated
            if (FindObjectsOfType<EnemyBehaviors>().Length == 0)
            {
                Debug.Log("You won");
                DestroyShip(); // returns to main menu
            }
        }
        else 
        {
            // game over, shouldn't reach this
        }
    }

    // loading in various components and children for later
    private void Awake()
    {
        ShipRigidbody = GetComponent<Rigidbody2D>();
        CountdownLabel = GetComponentInChildren<TMP_Text>();
        AudioSource[] sources = GetComponentsInChildren<AudioSource>();
        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i].gameObject.name.Contains("1"))
            {
                Countdown1 = sources[i];
            }
            else if (sources[i].gameObject.name.Contains("2"))
            {
                Countdown2 = sources[i];
            }
        }
    }

    // fires a laser and angles it properly
    public void FireShot() 
    {
        GameObject laser = Instantiate(LaserPrefab, transform.position, Quaternion.identity);
        laser.transform.eulerAngles = transform.eulerAngles;
    }

    // old, unused code used to turn in place to look at the mouse
    public void FaceCursor() 
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float theta = 360 - Mathf.Atan2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y) * 180 / Mathf.PI;
        theta = (theta + 360) % 360;
        transform.eulerAngles = new Vector3(0f, 0f, theta);
    }

    // has the cursor home in on the mouse location
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

        /*
        // old, complex movement code with two different states (moving and turning in place)
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
        */

        // comment these two lines to revert to old movement
        movementSpeed = Mathf.Clamp(BaseMovementSpeed * distanceToMouse.magnitude, -MaxMovementSpeed, MaxMovementSpeed);
        rotationSpeed = MinRotationSpeed;

        // apply speed to ship
        ShipRigidbody.angularVelocity = -rotationCorrection * rotationSpeed;
        ShipRigidbody.velocity = transform.up * movementSpeed;
    }

    // various bad collisions that will result in a death
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Planet") || collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<Animator>().Play("PlayerDestroy");
            IsAlive = false;
            ShipRigidbody.angularVelocity = 0;
            ShipRigidbody.velocity = Vector2.zero;
        }
    }

    // method used to end the scene when the ship dies, doubles as a back to menu function
    public void DestroyShip()
    {
        // Destroy(gameObject); // adding line back this will cause issues with enemies
        SceneManager.LoadScene(0); // back to level select on death
    }

    // finds and plays the explosion sound
    public void PlayExplosionSound() 
    {
        AudioSource[] sources = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource source in sources)
        {
            if (source.name.Equals("Explosion"))
            {
                source.Play();
            }
        }
    }
}
