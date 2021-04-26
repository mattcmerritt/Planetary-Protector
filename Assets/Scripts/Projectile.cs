using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Projectile components
    public Rigidbody2D ProjectileRigidbody;
    // Projectile variables
    public float MovementSpeed; // default 5

    // Using the coordinates passed into the function, send the projectile in the direction of the target point
    // The enemy will pass the values from MoveTarget in for x and y.
    // This value is not updated, so even if the enemy changes its MoveTarget, the projectile will continue going in the direction of the original point it was passed.
    public void SetTarget(float x, float y) {
        float theta = 360 - Mathf.Atan2(x - transform.position.x, y - transform.position.y) * 180 / Mathf.PI;
        theta = (theta + 360) % 360;
        transform.eulerAngles = new Vector3(0f, 0f, theta);
        ProjectileRigidbody.velocity = transform.up * MovementSpeed;
        ProjectileRigidbody.angularVelocity = 0;
    }

    // If the projectile goes far off screen without hitting anything, destroy it to save memory
    private void Update()
    {
        if (Mathf.Abs(transform.position.x) > 9)
        {
            Destroy(gameObject);
        }
        if (Mathf.Abs(transform.position.y) > 6)
        {
            Destroy(gameObject);
        }
    }

    // If the projectile hits something, destroy the projectile
    // The other objects will handle their side of the collision, so we don't need to tell the other object to do anything
    private void OnCollisionEnter2D (Collision2D collision)
    {
        Destroy(gameObject);
    }
}
