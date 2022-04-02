using UnityEngine;

public class Scythe : MonoBehaviour
{
    public float speed;
    public Transform handBone;
    public Sprite normalSprite, throwSprite;
    public RectTransform arrowRectTransform;
    public Texture2D cursorTexture;
    
    private Rigidbody2D rb;
    private Vector2 direction;
    private Transform spriteTransform;
    private Transform playerTransform;
    private Collider2D coll;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        rb = transform.GetComponent<Rigidbody2D>(); 
        coll = transform.GetComponent<Collider2D>();    
        spriteTransform = transform.Find("ScytheSprite");

        Get(GameObject.FindGameObjectWithTag("Player").transform);
    }
    void FixedUpdate()
    {
        if(!playerTransform)
        {
            spriteTransform.Rotate(new Vector3(0f, 0f, -speed * 100 * Time.fixedDeltaTime));
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

            if(!spriteTransform.GetComponent<SpriteRenderer>().isVisible)
            {
                //  Update scythe direction arrow
                Vector3 aimTargetDiff = Camera.main.WorldToScreenPoint(transform.position) - arrowRectTransform.position;
                aimTargetDiff.Normalize();
                float rot_z = Mathf.Atan2(aimTargetDiff.y, aimTargetDiff.x) * Mathf.Rad2Deg;
                arrowRectTransform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                arrowRectTransform.gameObject.SetActive(true);
            }
            else
                arrowRectTransform.gameObject.SetActive(false);
        }
        else
        {
            transform.localPosition = new Vector3(0f, 0f, 0f);
            transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            spriteTransform.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
            Get(collider.gameObject.transform);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        ContactPoint2D contact = collision.GetContact(0);

        int randomDirectionValue = Random.Range(-10, 10);
        randomDirectionValue = randomDirectionValue > 0 ? 1 : -1;

        if(contact.normal == new Vector2(0f, -1f) || contact.normal == new Vector2(0f, 1f))
            direction = new Vector2(direction.x > 0f ? 1f : -1f, contact.normal.y).normalized;
        else if(contact.normal == new Vector2(-1f, 0f) || contact.normal == new Vector2(1f, 0f))
            direction = new Vector2(contact.normal.x, direction.y > 0f ? 1f : -1f).normalized;
        else
            direction = contact.normal.normalized;
    }

    public void Throw(Vector2 direction)
    {
        if(playerTransform)
        {
            transform.SetParent(null);
            this.direction = direction; 
            spriteTransform.GetComponent<SpriteRenderer>().sprite = throwSprite;
            playerTransform = null;
        }
    }

    public void Get(Transform playerTrasform)
    {
        transform.SetParent(handBone);
        this.playerTransform = playerTrasform;
        spriteTransform.GetComponent<SpriteRenderer>().sprite = normalSprite;
    }


}
