using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float DistanceFromShip;
    private float XDisplacement, YDisplacement;

    private void Start()
    {
        XDisplacement = Random.Range(-DistanceFromShip, DistanceFromShip);
        if(Random.Range(-1f, 1f) > 0)
        {
            YDisplacement = Mathf.Sqrt(Mathf.Pow(DistanceFromShip, 2) - Mathf.Pow(XDisplacement, 2));
        }
        else
        {
            YDisplacement = -Mathf.Sqrt(Mathf.Pow(DistanceFromShip, 2) - Mathf.Pow(XDisplacement, 2));
        }
        transform.position = new Vector3(XDisplacement, YDisplacement, 0);
    }

    private void Update()
    {
        
    }
}
