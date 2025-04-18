using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour, IDataPersistence
{
    //Serilaized variables handled in unity engine
    [SerializeField] public Rigidbody2D playerRB;
    [SerializeField] private Animator animator;
    [SerializeField] private CircleCollider2D playerCC;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] private Text respawnText;
    [SerializeField] private bool godMode;

    //private variables used below
    private float moveSpeed = 3.5f;
    private bool grounded;
    private float horizontalMove = 0f;
    private bool facingRight = true;
    private bool prepareLongJump = false;
    private float longJumpDis = 0f;
    private bool inFluid = false;
    private Vector3 curVelocity;
    private string mod = "";
    private bool isDead = false;
    private float jumpHeight = 150f;
    private bool inPast = false;
    private string nextScene = "";
    private bool interactable = false;
    private Vector3 respawnPoint;
    public Vector3 loc;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (respawnPoint == Vector3.zero || respawnPoint == null)
           {
                respawnPoint = new Vector3(-5.58f, 2.44f, 0f);
           }

    }

    //Update is called once per frame 
    void Update()
    {
        loc = transform.position;
        if (godMode)
        {
            mod = "Yellow";   
        }

        if (!isDead)
        {
            respawnText.enabled = false;
            //Used for long jumping, and charging your jump
            if (Input.GetKey(KeyCode.LeftShift) == true && grounded)
            {
                prepareLongJump = true;
                horizontalMove = 0f;
            }
            else
            {
                prepareLongJump = false;
                jumpHeight = 150f;
            }

            if (!prepareLongJump && grounded)
            {
                horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
            }
            else if (prepareLongJump && jumpHeight < 200f && mod != "Yellow")
            {
                jumpHeight += 1f;

                if (longJumpDis < 2.33f)
                {
                    longJumpDis += 0.05f;
                }

            }
            else if (prepareLongJump && jumpHeight < 600f && mod == "Yellow")
            {
                jumpHeight += 5f;

            }
            else if (!prepareLongJump && !grounded)
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (Mathf.Abs(horizontalMove) < 3.5f)
                    {
                        horizontalMove -= 0.1f;
                    }

                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (Mathf.Abs(horizontalMove) < 3.5f)
                    {
                        horizontalMove += 0.1f;
                    }
                }


            }

            //Makes the character jump when space is pressed
            if (Input.GetKeyDown(KeyCode.Space) == true && grounded)
            {
                playerRB.AddForce(transform.up * jumpHeight);
            }


            if (Input.GetKeyDown(KeyCode.E) == true && interactable && nextScene != "")
            {
                StartCoroutine(SceneChanger.LoadLevel(nextScene, LoadSceneMode.Single));
                respawnPoint = transform.position;
            } 



            //Adds long jump distant to movement speed and adds drag for air time
            if (!grounded && longJumpDis > 0f)
            {
                moveSpeed += longJumpDis;
                jumpHeight = 150f;
                longJumpDis = 0f;

                horizontalMove = facingRight ? moveSpeed : moveSpeed * -1;
                moveSpeed = 3.5f;
                //moveSpeed = moveSpeed > 3.5f ? moveSpeed -= 0.01f : 3.5f;
            }
            

            //Flip character on X axis, depending on which direction the sprite is facing.
            if (horizontalMove < 0f && facingRight)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                facingRight = false;
            }
            else if (horizontalMove > 0f && !facingRight)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                facingRight = true;
            }

            curVelocity = playerRB.linearVelocity;

            //Setting values to be used in animator in unity, to swap animations dependent on variable
            animator.SetFloat("speed", Mathf.Abs(horizontalMove));
            animator.SetBool("prepareLongJump", prepareLongJump);
            animator.SetBool("grounded", grounded);

        }
        else
        {
            respawnText.enabled = true;
            horizontalMove = 0f;
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
        }
    }

    private void FixedUpdate()
    {
        //add horizontal movement
        playerRB.linearVelocityX = horizontalMove;
        //ray casting to determine if character is in the air or not\
        RaycastHit2D rayCastResults = Physics2D.Raycast(transform.localPosition, Vector2.down, 1f);

        grounded = inFluid ? true : rayCastResults;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        //get name of tile collided with
        TileBase tile = getTileCollide(collision);

        if (tile != null)
        {

            if (tile.name.Contains("Spike"))
            {
                Die();
            }

        }
        else
        {
            GameObject gameObject = collision.gameObject;
            if (gameObject.name.Contains("Bullet") || gameObject.name.Contains("WreckingBall"))
            {
                Die();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if ((collision.name.Contains("Lava") && mod == "Red") || (collision.name.Contains("Water") && mod == "Purple"))
        {
            inFluid = true;
        }
        else if((collision.name.Contains("Water") && mod != "Purple") || (collision.name.Contains("Lava") && mod != "Red"))
        {
            Die();
        }

        if (collision.name.Contains("Gate"))
        {
            if(mod != "" && collision.name.Contains(mod) || godMode)
            {
                interactable = true;
                nextScene = collision.name.Split("_")[1];
            }
            else if (!collision.name.Contains("Green") &&  !collision.name.Contains("Purple") && !collision.name.Contains("Red") && !collision.name.Contains("Yellow"))
            {
                interactable = true;
                nextScene = collision.name.Split("_")[1];
            }

        }

        if (collision.name.Contains("Shrine") && mod == "")
        {
            mod = collision.name.Split("_")[1];

            switch (mod)
            {
                case "Green":
                    spriteRenderer.color = Color.green;
                    break;
                case "Purple":
                    spriteRenderer.color = Color.magenta;
                    break;
                case "Yellow":
                    spriteRenderer.color = Color.yellow;
                    break;
                case "Red":
                    spriteRenderer.color = Color.red;
                    break;
                default:
                    spriteRenderer.color = Color.white;
                    break;

            }
                
          
            

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Contains("Lava") || collision.name.Contains("Water"))
        {
            inFluid = false;
        }

        if (collision.name.Contains("Gate"))
        {
            interactable = false;
            nextScene = "";
        }

    }

    private void Die()
    {
        if (!godMode)
        {
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            playerRB.simulated = false;
            spriteRenderer.enabled = false;
            isDead = true;
        }
        
    }

    private void Respawn()
    {

        if (!inPast)
        {

            transform.position = respawnPoint;
            isDead = false;
            mod = "";
            spriteRenderer.color = Color.white;
        }

        if (inPast)
        {
            isDead = false;
            var dpm = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
            dpm.LoadCurrent();
        }
    }

    public void Obliterate()
    {
        if (!isDead && !inPast)
        {
            Die();
            mod = "";
        }
        
    }

    public void LoadData(GameData data)
    {
        inPast = inPast ? false : true;
        transform.position = data.playerPosition;
    }
    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
    }

    private TileBase getTileCollide(Collision2D collision)
    {
        Tilemap tilemap = collision.collider.GetComponentInParent<Tilemap>();

        if (tilemap != null)
        {
            //get the contact point of collision and convert that position to the cell position to determine collision with tile map
            var contact = collision.GetContact(0);
            Vector3 worldPosition = contact.point - 0.05f * contact.normal;
            Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);


            //get name of tile collided with
            TileBase tile = tilemap.GetTile(cellPosition);
            return tile;

        }
        else
        {
            return null;
        }
    }

    public void endGame()
    {
        Destroy(gameObject);
    }
}