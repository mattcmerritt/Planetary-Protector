using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    // Planet placement variables
    public float DistanceFromShip; // placement radius from center, default 3.5
    private float XDisplacement, YDisplacement, PlacementAngle;

    // Planet componenets
    private Rigidbody2D PlanetRigidbody;
    private Animator PlanetAnimator;
    
    // Random placement variables
    // (Although functional, they aren't used anywhere, but we left them in the code in case we wanted to reuse the features they support.)
    private int SetLocationPasses, PlacementPriority;
    private const int MaxPasses = 30;

    // Set up rigidbody
    private void Awake()
    {
        PlanetRigidbody = GetComponent<Rigidbody2D>();
    }

    // Set up animator and random pass counter
    private void Start()
    {
        PlanetAnimator = GetComponent<Animator>();
        SetLocationPasses = 0;
    }

    // If a planet is hit by a trigger which isn't another planet (the laser), start the animation to destroy the planet
    // The player loses the game if they destroy a planet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Planet"))
        {
            PlanetAnimator.SetBool("PlanetHit", true);
            Debug.Log("Game Over: A planet was destroyed by your laser.");
        }
    }

    // (Random placement method)
    // If two planets are on top of one another, relocate the newer one
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            RelocateOnePlanet(collision.gameObject);
        }
    }

    // (Random placement method)
    // If planets, managed to get placed on top of one another twice in a row, relocate the newer one again
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            RelocateOnePlanet(collision.gameObject);
        }
    }

    // (Random placement method)
    // If the planet spawned on another planet, move the lower priority planet clockwise a bit
    public void PickStartLocationUsingPrevious()
    {
        if (SetLocationPasses < MaxPasses)
        {
            // in terms of a clock, this handles 9 inclusive to 3 exclusive
            if (YDisplacement > 0 || XDisplacement == -DistanceFromShip)
            {
                XDisplacement += Random.Range(0.2f, 0.5f);
                // keep bound to radius
                if(XDisplacement > DistanceFromShip)
                {
                    XDisplacement = DistanceFromShip;
                }
                YDisplacement = Mathf.Sqrt(Mathf.Pow(DistanceFromShip, 2) - Mathf.Pow(XDisplacement, 2));
            }
            // in terms of a clock, this handles 3 inclusive to 9 exclusive
            else
            {
                XDisplacement -= Random.Range(0.2f, 1f);
                // keep bound to radius
                if (XDisplacement < -DistanceFromShip)
                {
                    XDisplacement = -DistanceFromShip;
                }
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

    // (Random placement method)
    // If the planet spawned on another planet, move the lower priority planet to a new random location
    public void PickStartLocationRandomly()
    {
        if (SetLocationPasses < MaxPasses)
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
            Debug.LogError("A planet could not find a suitable place to load and has been skipped.");
        }
    }

    // (Random placement method)
    // Mutator for placement priority
    // This priority is supposed to determine which planet moves to minimize operations
    // On a two-planet collision during spawning, only the planet with the higher priority (the newer planet) will relocate
    public void SetPlacementPriority(int priority)
    {
        PlacementPriority = priority;
    }

    // (Random placement method)
    // Accessor for placement priority
    public int GetPlacementPriority()
    {
        return PlacementPriority;
    }

    // (Random placement method)
    // Relocate the newer planet only
    // Both planets will call this method, so the else doesn't need to tell the other planet to do anything
    public void RelocateOnePlanet(GameObject otherPlanet)
    {
        if(this.GetPlacementPriority() > otherPlanet.GetComponent<Planet>().GetPlacementPriority())
        {
            this.PickStartLocationRandomly();
            //this.PickStartLocationUsingPrevious();
        }
        // else do nothing, because the priority states that the other planet is the one that should move
    }

    // (Fixed-location placement method)
    // Places the planets in a pattern
    // Code based off of sample from Professor Blake
    public void SetPositionWithAngle(float angle)
    {
        PlacementAngle = angle;
        XDisplacement = 3.5f * Mathf.Cos(PlacementAngle);
        YDisplacement = 3.5f * Mathf.Sin(PlacementAngle);
        PlanetRigidbody.MovePosition(new Vector2(XDisplacement, YDisplacement));
    }

    // Called when the destruction animation is over, destroys the planet object and goes back to the main menu
    public void DestroyPlanet()
    {
        Destroy(gameObject);
        FindObjectOfType<Ship>().DestroyShip(); // send back to main menu
    }

    // Called by animation when planet is hit, plays the explosion sound effect
    public void PlayExplosion()
    {
        GetComponentInChildren<AudioSource>().Play();
    }
}
