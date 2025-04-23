using UnityEngine;

public class ShrineScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var particleSystem = GetComponentInChildren<ParticleSystem>();
        //play particle affect on each shrine
        if (!particleSystem.isPlaying)
        {
            particleSystem.Play();
        }
    }
}
