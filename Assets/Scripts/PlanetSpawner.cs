using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public GameObject PlanetPrefab;

    private void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Instantiate(PlanetPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
