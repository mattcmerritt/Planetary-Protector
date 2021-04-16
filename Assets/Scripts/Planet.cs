using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float DistanceFromShip;
    private float XDisplacement, YDisplacement;
    public Rigidbody2D PlanetRigidbody;
    private int SetLocationPasses;

    private void Start()
    {
        SetLocationPasses = 0;
        PickStartLocation();
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
        if(collision.gameObject.CompareTag("Planet"))
        {
            this.PickStartLocation();
        }
    }

    public void PickStartLocation()
    {
        // why does this line need to be here now?
        PlanetRigidbody = GetComponent<Rigidbody2D>();
        if (SetLocationPasses < 3)
        {
            XDisplacement = Random.Range(-DistanceFromShip, DistanceFromShip);
            if (Random.Range(-1f, 1f) > 0)
            {
                YDisplacement = Mathf.Sqrt(Mathf.Pow(DistanceFromShip, 2) - Mathf.Pow(XDisplacement, 2));
            }
            else
            {
                YDisplacement = -Mathf.Sqrt(Mathf.Pow(DistanceFromShip, 2) - Mathf.Pow(XDisplacement, 2));
            }
            PlanetRigidbody.MovePosition(new Vector2(XDisplacement, YDisplacement));
            SetLocationPasses++;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("A planet could not find a suitable place to load and has been skipped.");
        }
    }
}
