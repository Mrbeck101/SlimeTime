using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Camera menuCam;


    // Update is called once per frame
    void Update()
    {
        var loc = GameObject.Find("Slime").GetComponent<PlayerScript>();
        menuCam.transform.position = loc.loc;
    }
}
