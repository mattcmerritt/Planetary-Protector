using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviors : MonoBehaviour
{
    private Timer ActionTimer;
    public double ActionInterval; // 1 for normal, 2 for ranged, 0.1 for melee, 1.5 for super
    public int EnemyType; // 0 for normal, 1 for ranged, 2 for melee, 3 for super
    public Transform PlayerTransform, CannonTransform;
    public Rigidbody2D EnemyRigidbody;
    public bool OngoingMove;
    public Vector2 MoveTarget;
    public float MovementSpeed;
    private const float Tolerance = 0.05f;
    public GameObject ProjectilePrefab;

    // Ship
    private Ship Ship;

    private void Awake()
    {
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        Ship = FindObjectOfType<Ship>();
    }

    private void Start()
    {
        ActionTimer = new Timer(ActionInterval);
        ActionTimer.Start();
        OngoingMove = false;
    }

    private void Update() {
        if (Ship.HasStarted)
        {
            ActionTimer.IncrementTime(Time.deltaTime);

            bool TimerFinished = ActionTimer.TimerFinished();

            if (TimerFinished)
            {
                // Normal enemy: reposition
                if(EnemyType == 0)
                {
                    Reposition();
                }
                // Ranged enemy: reposition and fire projectile
                else if(EnemyType == 1)
                {
                    Reposition();
                }
                // Melee enemy: follow player
                else if(EnemyType == 2)
                {
                    FollowPlayer();
                }
                // Super enemy: follow player (technically move to old position) and fire projectile
                else if(EnemyType == 3)
                {
                    FollowPlayer();
                }
            }

            // if the target has been reached, stop trying to reach it
            // this makes the ship move forward until next move instead of flipping in place
            if (Mathf.Abs(transform.position.x - MoveTarget.x) < Tolerance && Mathf.Abs(transform.position.y - MoveTarget.y) < Tolerance)
            {
                OngoingMove = false;
            }

            // face the target, and then move to the target
            if (OngoingMove)
            {
                float theta = 360 - Mathf.Atan2(MoveTarget.x - transform.position.x, MoveTarget.y - transform.position.y) * 180 / Mathf.PI;
                theta = (theta + 360) % 360;
                transform.eulerAngles = new Vector3(0f, 0f, theta);
                EnemyRigidbody.velocity = transform.up * MovementSpeed;
                EnemyRigidbody.angularVelocity = 0;
            }

            if (TimerFinished) {
                if(EnemyType == 1 || EnemyType == 3)
                {
                    FireProjectile();
                }
            }
        }
    }

    // if an enemy hits something, stop moving
    private void OnCollisionEnter2D (Collision2D collision)
    {
        EnemyRigidbody.angularVelocity = 0;
    }

    // the trigger is the laser, so die on hit
    private void OnTriggerEnter2D (Collider2D collision)
    {
        EnemyDie();
    }

    // Destroy the enemy and play an animation
    public void EnemyDie()
    {
        // todo animation
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

    // Shoot a projectile at the player's current position
    public void FireProjectile()
    {
        GameObject projectile = Instantiate(ProjectilePrefab, CannonTransform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetTarget(MoveTarget.x, MoveTarget.y);
    }

    // move to the player's current position
    public void FollowPlayer()
    {
        OngoingMove = true;
        MoveTarget = new Vector2(PlayerTransform.position.x, PlayerTransform.position.y);
        CheckMoveTargetOnScreen();
    }

    // make sure that the intended movement point is on screen
    // if a coordinate is off screen, replace with 0
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
