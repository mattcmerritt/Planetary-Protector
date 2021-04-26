using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviors : MonoBehaviour
{
    // Timer variables
    private Timer ActionTimer;
    public double ActionInterval; // 1 for normal, 2 for ranged, 0.1 for melee, 1.5 for super

    // Enemy-specific variables
    private Rigidbody2D EnemyRigidbody;
    private Animator EnemyAnimator;
    public int EnemyType; // used to pick behaviors; 0 for normal, 1 for ranged, 2 for melee, 3 for super
    public Transform PlayerTransform, CannonTransform; // used for tracking and shooting
    public bool OngoingMove, Dying; // Is the enemy not at the destination yet? Is the enemy playing the death animation?
    public Vector2 MoveTarget; // the coordinates the enemy is trying to move to
    public float MovementSpeed; // how fast the ship moves; 2 for normal, 2 for ranged, 1.5 for melee, 2 for super
    private const float Tolerance = 0.05f; // since coordinates are floats, tolerance is used to approximate "close enough" for reaching destinations
    
    // References to other objects the enemy uses
    public GameObject ProjectilePrefab; // the projectile
    private Ship Ship; // the player

    // Initially get ship and components
    private void Awake()
    {
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
        Ship = FindObjectOfType<Ship>();
    }

    // Set up timer and state booleans on startup
    private void Start()
    {
        ActionTimer = new Timer(ActionInterval);
        ActionTimer.Start();
        OngoingMove = false;
        Dying = false;
    }

    // Update is where the enemies "act" (move/shoot/follow)
    private void Update() {
        if (Ship.HasStarted)
        {
            if(!Dying)
            {
                ActionTimer.IncrementTime(Time.deltaTime);

                // boolean for timer check, since we need to check it twice in Update but it resets after a successful check
                bool TimerFinished = ActionTimer.TimerFinished();

                // First timer-specific event
                // Set the new destination for the enemy to move to based on enemy type
                if (TimerFinished)
                {
                    // Normal enemy or ranged enemy: reposition
                    if(EnemyType == 0 || EnemyType == 1)
                    {
                        Reposition();
                    }
                    // Melee enemy or super enemy: follow player (technically move to old position)
                    else if(EnemyType == 2 || EnemyType == 3)
                    {
                        FollowPlayer();
                    }
                }

                // If the target has been reached, stop trying to reach it
                // This makes the ship move forward until next move instead of flipping in place rapidly
                if (Mathf.Abs(transform.position.x - MoveTarget.x) < Tolerance && Mathf.Abs(transform.position.y - MoveTarget.y) < Tolerance)
                {
                    OngoingMove = false;
                }

                // If the ship is moving, turn to face the target, and then move to the target
                if (OngoingMove)
                {
                    float theta = 360 - Mathf.Atan2(MoveTarget.x - transform.position.x, MoveTarget.y - transform.position.y) * 180 / Mathf.PI;
                    theta = (theta + 360) % 360;
                    transform.eulerAngles = new Vector3(0f, 0f, theta);

                    EnemyRigidbody.velocity = transform.up * MovementSpeed;
                    EnemyRigidbody.angularVelocity = 0; // prevents random buggy spinning/turning on move
                }

                // Second timer-specific event
                // If the ship type matches, shoot a projectile out of the cannon spot (in front of the ship so the bullet doesn't hit the enemy)
                if (TimerFinished) {
                    if(EnemyType == 1 || EnemyType == 3)
                    {
                        FireProjectile();
                    }
                }
            }
        }
    }

    // If an enemy hits something, stop moving
    private void OnCollisionEnter2D (Collision2D collision)
    {
        EnemyRigidbody.angularVelocity = 0;
    }

    // The trigger is the laser, so play the death animation on hit using the animator boolean
    private void OnTriggerEnter2D (Collider2D collision)
    {
        Dying = true;
        EnemyAnimator.SetBool("Hit", true);
    }

    // Destroy the enemy, triggered by the animation
    public void EnemyDie()
    {
        Destroy(gameObject);
    }

    // Move to a random nearby position
    public void Reposition()
    {
        OngoingMove = true;
        float dxMagnitude = Random.Range(1f, 3f);
        if (Random.Range(0, 2) < 1)
        {
            dxMagnitude *= -1;
        }
        float dyMagnitude = Random.Range(1f, 3f);
        if (Random.Range(0, 2) < 1)
        {
            dyMagnitude *= -1;
        }
        MoveTarget = new Vector2(transform.position.x + dxMagnitude, transform.position.y + dyMagnitude);
        CheckMoveTargetOnScreen();
    }

    // Shoot a projectile at the enemy's current target position (where they're moving to)
    // If the ship is a ranged ship, it will shoot in a random direction
    // If the ship is a super ship, it will shoot at the player
    public void FireProjectile()
    {
        GameObject projectile = Instantiate(ProjectilePrefab, CannonTransform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetTarget(MoveTarget.x, MoveTarget.y);
    }

    // Move to the player's current position (at the time of the call)
    // The melee ship will closely follow, but the super ship will intentionally lag behind a bit
    public void FollowPlayer()
    {
        OngoingMove = true;
        MoveTarget = new Vector2(PlayerTransform.position.x, PlayerTransform.position.y);
        CheckMoveTargetOnScreen();
    }

    // Makes sure that the intended movement coordinates are on screen
    // If a coordinate is off screen, replace that one coordinate with 0
    // For example, a target of (9, 2) becomes (0, 2)
    public void CheckMoveTargetOnScreen()
    {
        if (Mathf.Abs(MoveTarget.x) > 8)
        {
            MoveTarget.x = 0;
        }
        if (Mathf.Abs(MoveTarget.y) > 5)
        {
            MoveTarget.y = 0;
        }
    }
}
