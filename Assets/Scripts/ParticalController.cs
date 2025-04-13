using System.Collections;
using UnityEngine;

public class ParticalController : MonoBehaviour
{
    [SerializeField] ParticleSystem movementParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem respawnParticle;

    [Range(0.0f, 1.5f)]
    [SerializeField] float occurAfterVelocity;

    [Range (0, 0.2f)]
    [SerializeField] float slimeFormationPeriod;

    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] SpriteRenderer playerSR;

    float counter;
    bool deathPlayed = false;

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        var emission = movementParticle.emission;
        var shape = movementParticle.shape;
        
        //check if character is moving horizontally, if so add particles
        if(Mathf.Abs(playerRB.linearVelocityX) > occurAfterVelocity)
        {
            if (playerRB.linearVelocityX < 0 && shape.scale.x > 0)
            {
                shape.scale *= -1;
            } 
            else if (playerRB.linearVelocityX > 0 && shape.scale.x < 0)
            {
                shape.scale *= -1;
            }

            if (counter > slimeFormationPeriod)
            {
                //Debug.Log("Where's my particle");
                emission.enabled = true;
                counter = 0;
            }
        }
        else
        {
            emission.enabled = false;
        }

        if(playerRB.constraints == RigidbodyConstraints2D.FreezeAll && !deathPlayed)
        {
            deathParticle.Play();
            deathPlayed = true;
        } 
        else if(playerRB.constraints == RigidbodyConstraints2D.FreezeRotation)
        {
            deathPlayed = false;
        }

        if(Input.GetKeyDown(KeyCode.R) && deathPlayed)
        {
            deathParticle.Stop();
            respawnParticle.Play();
            StartCoroutine(waitForRespawn());
            
        }

        


    }

    private IEnumerator waitForRespawn()
    {
        while (respawnParticle.isPlaying)
        {
            yield return null;
        }

        
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerSR.enabled = true;
        playerRB.simulated = true;

    }

}
