using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapScript : MonoBehaviour
{
    public Tilemap tm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Tilemap tilemap = tm;


        if (tilemap != null)
        {
            Vector2 worldPosition = collision.GetContact(0).point;

            Debug.Log("World Position: X:" + worldPosition.x + " Y:" + worldPosition.y);


            Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

            Debug.Log("Cell Position: X:" + cellPosition.x + " Y:" + cellPosition.y + " Z:" + cellPosition.z);

            TileBase tile = tilemap.GetTile(cellPosition);

            if (tile != null)
            {
                // Example: Check tile name
                Debug.Log("Collided with tile: " + tile.name);
            }
            else
            {
                Debug.Log("Tile is null");
            }

        }

    }
}
