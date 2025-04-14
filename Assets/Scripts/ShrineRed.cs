using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RedShrine : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        var cannon = GameObject.Find("Cannon5").GetComponent<CannonScript>();
        cannon.rateOfFire = 0.16f;
    }
}
