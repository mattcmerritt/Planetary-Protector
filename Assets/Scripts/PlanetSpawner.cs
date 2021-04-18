using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public GameObject PlanetPrefab;
    public int NumPlanets;

    private void Start()
    {
        for (int i = 0; i < NumPlanets; i++)
        {
            GameObject NewPlanet = Instantiate(PlanetPrefab, new Vector3(-10, -10, 0), Quaternion.identity);
            NewPlanet.GetComponent<Planet>().SetPlacementPriority(i);
        }
    }
}
