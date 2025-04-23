using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [SerializeField] Camera menuCam;


    //I was trying to make the revisit past menu/scene transparent so I had the camera overlay ontop of the slime, but I could not figure out how to make it transparent despite setting the alpha in
    //color to 0, so this code is used but not completely utilized
    void Update()
    {

        if (GameObject.Find("Slime") != null)
        {
            var loc = GameObject.Find("Slime").GetComponent<PlayerScript>();
            menuCam.transform.position = loc.loc;
        }

    }
}
