using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(SceneChanger.LoadLevel("EndScreen", LoadSceneMode.Single));
        var slime = GameObject.Find("Slime").GetComponent<PlayerScript>();
        slime.endGame();
    }
}
