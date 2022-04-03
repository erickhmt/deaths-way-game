using UnityEngine;

public class Scythe : MonoBehaviour
{
    public float speed, specialSpeedMultiplier;
    public Transform handBone;
    public Sprite normalSprite, throwSprite, specialSprite;
    public RectTransform arrowRectTransform;
    public Texture2D cursorTexture;
    [HideInInspector]
    public bool isReturning, isThrowing, isSpecial = false;
    
    private Rigidbody2D rb;
    private Vector2 direction;
    private Transform spriteTransform;
    private Transform playerTransform;
    private Collider2D coll;
    private CharacterStats stats;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        rb = transform.GetComponent<Rigidbody2D>(); 
        coll = transform.GetComponent<Collider2D>();    
        spriteTransform = transform.Find("ScytheSprite");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        stats = playerTransform.GetComponent<CharacterStats>();
        spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();

        Get();
    }

    void Update() 
    {
        // Set special state
        if(isThrowing)
        {
            if(Input.GetKeyDown(KeyCode.Mouse1) && stats.mana > 0f && !isSpecial)
            {
                isSpecial = true;
                speed *= specialSpeedMultiplier;
                spriteRenderer.sprite = specialSprite;
            }
            if((Input.GetKeyUp(KeyCode.Mouse1) || stats.mana == 0f) && isSpecial)
            {
                isSpecial = false;
                speed /= specialSpeedMultiplier;
                spriteRenderer.sprite = throwSprite;
            }
        }

        if(isSpecial) stats.ConsumeMana();
    }
    void FixedUpdate()
    {
        if(isThrowing)
        {
            spriteTransform.Rotate(new Vector3(0f, 0f, -400000 * Time.fixedDeltaTime));
            
            if(isReturning)
            {
                Vector3 distance = Camera.main.WorldToScreenPoint(playerTransform.position) - Camera.main.WorldToScreenPoint(transform.position);
                if (distance.magnitude < 20)
                    Get();
                direction = distance.normalized;
            }
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

            if(!spriteRenderer.isVisible)
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
            Get();
        else if(collider.gameObject.tag == "Enemy" && isThrowing)
        {
            collider.gameObject.GetComponent<Enemy>().TakeDamage(isSpecial ? Random.Range(20f, 30f) : Random.Range(10f, 20f));
        }
    }

    public void Throw(Vector2 direction)
    {
        if(!isReturning)
        {
            transform.SetParent(null);
            this.direction = direction; 
            spriteRenderer.sprite = throwSprite;
            spriteRenderer.GetComponent<CircleCollider2D>().isTrigger = false;
            isThrowing = true;
        }
    }

    public void RequestReturn()
    {
        isReturning = true;
    }

    public void Get()
    {
        if(isSpecial)
            speed /= specialSpeedMultiplier;

        transform.SetParent(handBone);
        isReturning = isThrowing = isSpecial = false;
        spriteRenderer.sprite = normalSprite;
        spriteRenderer.GetComponent<CircleCollider2D>().isTrigger = true;
        playerTransform.Find("Character").GetComponent<Animator>().SetTrigger("catch");
        arrowRectTransform.gameObject.SetActive(false);
    }

}
