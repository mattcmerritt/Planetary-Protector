using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    // Planet spawning parameters
    public GameObject PlanetPrefab;
    public int NumPlanets;
    public bool isRandom; // Is random generation being used, or fixed-location generation?

    // Pick which planet spawning method is being used, and spawn the planets
    // In reality, all of the levels used fixed-location generation, but random still technically works and is supported.
    private void Start()
    {
        if (isRandom)
        {
            RandomStart();
        }
        else
        {
            FixedStart();
        }

    }

    // Randomly instantiate and reposition a total of numPlanets planets.
    public void RandomStart()
    {
        for (int i = 0; i < NumPlanets; i++)
        {
            GameObject NewPlanet = Instantiate(PlanetPrefab, new Vector3(-10 - i, -10 - i, 0), Quaternion.identity);
            NewPlanet.GetComponent<Planet>().SetPlacementPriority(i);
            NewPlanet.GetComponent<Planet>().PickStartLocationRandomly();
        }
    }

    // Procedurally instantiate and reposition a total of numPlanets equidistant planets.
    public void FixedStart()
    {
        for (int i = 0; i < NumPlanets; i++) {
            GameObject NewPlanet = Instantiate(PlanetPrefab, new Vector3(-10 - i, -10 - i, 0), Quaternion.identity);
            NewPlanet.GetComponent<Planet>().SetPlacementPriority(i);
            NewPlanet.GetComponent<Planet>().SetPositionWithAngle(2 * Mathf.PI / NumPlanets * i);
        }
    }
}
