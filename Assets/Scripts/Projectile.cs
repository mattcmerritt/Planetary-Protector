using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D ProjectileRigidbody;
    public float MovementSpeed;

    public void SetTarget(float x, float y) {
        float theta = 360 - Mathf.Atan2(x - transform.position.x, y - transform.position.y) * 180 / Mathf.PI;
        theta = (theta + 360) % 360;
        transform.eulerAngles = new Vector3(0f, 0f, theta);
        ProjectileRigidbody.velocity = transform.up * MovementSpeed;
        ProjectileRigidbody.angularVelocity = 0;
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x) > 8)
        {
            Destroy(gameObject);
        }
        if (Mathf.Abs(transform.position.y) > 6)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        Destroy(gameObject);
    }
}
