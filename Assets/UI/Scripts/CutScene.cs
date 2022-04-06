using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CutScene : MonoBehaviour
{
    public string sceneName;
    public Sprite[] sprites;
    private Image img;
    
    private int index;
    void Start()
    {
        index = 0;
        img = transform.GetComponent<Image>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            index++;

            if(index == sprites.Length - 1)
            {
                if(sceneName != "")
                    GameObject.Find("Label").GetComponent<TextMeshProUGUI>().text = "[SPACE] Start Game";
                else
                    GameObject.Find("Label").GetComponent<TextMeshProUGUI>().text = "[SPACE] Quit Game";
            }


            if(index < sprites.Length)
                img.sprite = sprites[index];
            else 
            {
                if(sceneName == "")
                    Application.Quit();
                else
                    SceneManager.LoadScene(sceneName);
            }
        }
    }
}
