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
            Instantiate(PlanetPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
