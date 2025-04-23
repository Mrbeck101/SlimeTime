using UnityEngine;


public class RedShrine : MonoBehaviour
{
    
    //trigger trap on red shrine
    private void OnTriggerEnter2D(Collider2D other)
    {
        var cannon = GameObject.Find("Cannon5").GetComponent<CannonScript>();
        cannon.rateOfFire = 0.16f;
    }
}
