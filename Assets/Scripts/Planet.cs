using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float DistanceFromShip;
    private float XDisplacement, YDisplacement;
    private Rigidbody2D PlanetRigidbody;

    private void Start()
    {
        PlanetRigidbody = GetComponent<Rigidbody2D>();
        XDisplacement = Random.Range(-DistanceFromShip, DistanceFromShip);
        if(Random.Range(-1f, 1f) > 0)
        {
            YDisplacement = Mathf.Sqrt(Mathf.Pow(DistanceFromShip, 2) - Mathf.Pow(XDisplacement, 2));
        }
        else
        {
            YDisplacement = -Mathf.Sqrt(Mathf.Pow(DistanceFromShip, 2) - Mathf.Pow(XDisplacement, 2));
        }
        PlanetRigidbody.MovePosition(new Vector2(XDisplacement, YDisplacement));
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Planet"))
        {
            Destroy(gameObject);
            // do some fancy explosion stuff later
            // lose the game
        }
    }
}
