 using UnityEngine;
 
 public class MusicController : MonoBehaviour
 {
    private AudioSource audioSource;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audioSource = transform.GetComponent<AudioSource>();
        audioSource.Play();
    }
 
 }