using UnityEngine;

public class Scythe : MonoBehaviour
{
    public float speed;
    public Transform handBone;
    
    private Rigidbody2D rb;
    private Vector2 direction;
    private Transform spriteTransform;
    private Transform playerTransform;
    private Collider2D coll;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>(); 
        direction = transform.up;
        spriteTransform = transform.Find("ScytheSprite");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        coll = transform.GetComponent<Collider2D>(); 
    }
    void FixedUpdate()
    {
        if(!playerTransform)
        {
            spriteTransform.Rotate(new Vector3(0f, 0f, -speed * 30 * Time.fixedDeltaTime));
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            // rb.velocity = Vector2.zero;
            transform.SetParent(handBone);
            transform.localPosition = new Vector3(0f, 0f, 0f);
            transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            spriteTransform.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerTransform = collider.gameObject.transform;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        ContactPoint2D contact = collision.GetContact(0);

        int randomDirectionValue = Random.Range(-10, 10);
        randomDirectionValue = randomDirectionValue > 0 ? 1 : -1;

        if(contact.normal == new Vector2(0f, -1f) || contact.normal == new Vector2(0f, 1f))
        {
            direction = new Vector2(direction.x > 0f ? 1f : -1f, contact.normal.y).normalized;
        }
        else if(contact.normal == new Vector2(-1f, 0f) || contact.normal == new Vector2(1f, 0f))
        {
            direction = new Vector2(contact.normal.x, direction.y > 0f ? -1f : 1f).normalized;
        }
    }

    public void Throw(Vector2 direction)
    {
        if(playerTransform)
        {
            transform.SetParent(null);
            playerTransform = null;
            this.direction = direction; 
        }
    }
}
