using UnityEngine;
using UnityEngine.Tilemaps;

public class ShrinePuple : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var trap = GameObject.Find("TrapMap").GetComponent<Tilemap>();
        var slide = trap.GetComponent<SliderJoint2D>();
        slide.enabled = true;
    }
}