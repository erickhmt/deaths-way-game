using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform connectPoint;
    public GameObject wavePanel;
    public bool isActive;
    private SpriteRenderer sprite;
    private Transform player;
    void Start()
    {
        isActive = true;
        sprite = transform.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate() 
    {
        sprite.enabled = !isActive;
    }

    void Update()
    {
        
        if(isActive && ((player.position - transform.position).magnitude < 10f) && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(DoTeleport());
        }   
    }

    IEnumerator DoTeleport()
    {
        yield return new WaitForSeconds(.5f);
        player.position = connectPoint.position; 
        Object.FindObjectOfType<Scythe>().Get();
        wavePanel.SetActive(false);
    }
}
