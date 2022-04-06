using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLoader : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
            SceneManager.LoadScene("End");
    }
}
