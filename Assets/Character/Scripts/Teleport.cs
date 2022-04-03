using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform connectPoint;
    public bool isActive;
    private SpriteRenderer sprite;
    void Start()
    {
        isActive = true;
        sprite = transform.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() 
    {
        sprite.enabled = !isActive;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(isActive && collider.gameObject.tag == "Player" && Input.GetKeyUp(KeyCode.E))
        {
            collider.gameObject.transform.position = connectPoint.position;
        }
    }
}
