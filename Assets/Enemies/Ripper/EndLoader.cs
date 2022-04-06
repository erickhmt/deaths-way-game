using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLoader : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        SceneManager.LoadScene("End");
    }
}
