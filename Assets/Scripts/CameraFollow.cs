using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Camera menuCam;


    // Update is called once per frame
    void Update()
    {

        if (GameObject.Find("Slime") != null)
        {
            var loc = GameObject.Find("Slime").GetComponent<PlayerScript>();
            menuCam.transform.position = loc.loc;
        }

    }
}
