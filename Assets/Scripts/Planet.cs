using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float DistanceFromShip;
    private float XDisplacement, YDisplacement;
    private Rigidbody2D PlanetRigidbody;
    private int SetLocationPasses, PlacementPriority;
    private const int MaxPasses = 30;

    private void Start()
    {
        SetLocationPasses = 0;
        PickStartLocationRandomly();
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
        if (collision.gameObject.CompareTag("Planet"))
        {
            RelocateOnePlanet(collision.gameObject);
        }
    }

    // if planets managed to get placed on top of one another twice in a row, they wouldn't reposition again without this
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            RelocateOnePlanet(collision.gameObject);
        }
    }

    // if the planet spawned on another planet, move the lower priority planet clockwise a bit
    public void PickStartLocationUsingPrevious()
    {
        // why does this line need to be here now?
        PlanetRigidbody = GetComponent<Rigidbody2D>();
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

    // if the planet spawned on another planet, move the lower priority planet to a new random location
    public void PickStartLocationRandomly()
    {
        // why does this line need to be here now?
        PlanetRigidbody = GetComponent<Rigidbody2D>();
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
            Debug.Log("A planet could not find a suitable place to load and has been skipped.");
        }
    }

    // this priority is supposed to determine which planet moves to minimize operations
    // on a two-planet collision during spawning, only the planet with the higher priority will relocate
    public void SetPlacementPriority(int priority)
    {
        PlacementPriority = priority;
    }

    public int GetPlacementPriority()
    {
        return PlacementPriority;
    }

    public void RelocateOnePlanet(GameObject otherPlanet)
    {
        if(this.GetPlacementPriority() > otherPlanet.GetComponent<Planet>().GetPlacementPriority())
        {
            this.PickStartLocationRandomly();
            //this.PickStartLocationUsingPrevious();
        }
        // else do nothing, because the priority states that the other planet is the one that should move
    }
}
