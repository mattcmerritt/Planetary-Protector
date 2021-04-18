using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public GameObject PlanetPrefab;
    public int NumPlanets;
    public bool isRandom;

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

    public void RandomStart()
    {
        for (int i = 0; i < NumPlanets; i++)
        {
            GameObject NewPlanet = Instantiate(PlanetPrefab, new Vector3(-10 - i, -10 - i, 0), Quaternion.identity);
            NewPlanet.GetComponent<Planet>().SetPlacementPriority(i);
            NewPlanet.GetComponent<Planet>().PickStartLocationRandomly();
        }
    }

    public void FixedStart()
    {
        for (int i = 0; i < NumPlanets; i++) {
            GameObject NewPlanet = Instantiate(PlanetPrefab, new Vector3(-10 - i, -10 - i, 0), Quaternion.identity);
            NewPlanet.GetComponent<Planet>().SetPlacementPriority(i);
            NewPlanet.GetComponent<Planet>().SetPositionWithAngle(2 * Mathf.PI / NumPlanets * i);
        }
    }
}
