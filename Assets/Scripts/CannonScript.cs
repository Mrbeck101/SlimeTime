using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    [SerializeField] public float rateOfFire;
    [SerializeField] private GameObject bullet;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private float Velocity;
    private bool fired = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spriteRenderer.flipX == true)
        {
            Velocity = -Velocity;
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (spriteRenderer.sprite.name == "Cannon_3" && !fired)
        {

            fired = true;
            StartCoroutine(waitPeriod(rateOfFire));

        }
        else if (spriteRenderer.sprite.name == "Cannon_0" && fired)
        {
            animator.speed = 0f;
        }

    }

    private IEnumerator waitPeriod(float time)
    {
        Vector3 bulletPos = transform.position;
        bulletPos.y += 0.2f;

        if (spriteRenderer.flipX == false)
        {
            bulletPos.x += 1;
        } 
        else
        {
            bulletPos.x -= 1;
        }
        


        GameObject projectile = Instantiate(bullet, bulletPos, transform.rotation);
        projectile.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;

        projectile.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(Velocity, 0));
    
        yield return new WaitForSeconds(time);
        animator.speed = 1f;
        fired = false;
    }
}

