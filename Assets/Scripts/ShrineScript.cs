using Unity.VisualScripting;
using UnityEngine;

public class ShrineScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var particleSystem = GetComponentInChildren<ParticleSystem>();

        if (!particleSystem.isPlaying)
        {
            particleSystem.Play();
        }
    }
}
