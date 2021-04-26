using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviors : MonoBehaviour
{
    private Timer ActionTimer;
    public double ActionInterval; // 1 for normal, 1 for ranged, 0.1 for melee, 3 for ufo
    public int EnemyType; // 0 for normal, 1 for ranged, 2 for melee, 3 for ufo
    public Transform PlayerTransform;
    public Rigidbody2D EnemyRigidbody;
    public bool OngoingMove;
    public Vector2 MoveTarget;
    public float MovementSpeed;
    private const float Tolerance = 0.05f;

    private void Awake()
    {
        EnemyRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ActionTimer = new Timer(ActionInterval);
        ActionTimer.Start();
        OngoingMove = false;
    }

    private void Update()
    {
        ActionTimer.IncrementTime(Time.deltaTime);

        if (ActionTimer.TimerFinished())
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
                FireProjectile();
            }
            // Melee enemy: follow player
            else if(EnemyType == 2)
            {
                FollowPlayer();
            }
            // Melee enemy: orbit planets
            else if(EnemyType == 3)
            {
                OrbitPlanets();
            }
        }

        if (Mathf.Abs(transform.position.x - MoveTarget.x) < Tolerance && Mathf.Abs(transform.position.y - MoveTarget.y) < Tolerance)
        {
            OngoingMove = false;
        }

        if (OngoingMove)
        {
            // face the target, and then move to the target
            float theta = 360 - Mathf.Atan2(MoveTarget.x - transform.position.x, MoveTarget.y - transform.position.y) * 180 / Mathf.PI;
            theta = (theta + 360) % 360;
            transform.eulerAngles = new Vector3(0f, 0f, theta);
            EnemyRigidbody.velocity = transform.up * MovementSpeed;
            EnemyRigidbody.angularVelocity = 0;
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
        // todo Instantiate() projectile prefab and send it toward player
    }

    public void FollowPlayer()
    {
        OngoingMove = true;
        MoveTarget = new Vector2(PlayerTransform.position.x, PlayerTransform.position.y);
        CheckMoveTargetOnScreen();
    }

    public void OrbitPlanets()
    {
        // todo
    }

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
