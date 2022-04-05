using UnityEngine.UI;
using UnityEngine;

public class UpdateEnemyHealthBar : MonoBehaviour
{
    public Transform enemyTransform;
    public Vector2 offset;
    private Image bar;
    public Enemy normalEnemy;
    public DistanceEnemy hardEnemy;

    void Start() 
    {
        bar = transform.Find("HealthBar").GetComponent<Image>();
    }
    void FixedUpdate()
    {
        if(enemyTransform != null)
        {
            Vector2 barPosition = Camera.main.WorldToScreenPoint(enemyTransform.transform.position);
            transform.position = barPosition + offset;

            if(normalEnemy != null)
                bar.fillAmount = (1f / 100f) * normalEnemy.health;
            else if(hardEnemy != null)
                bar.fillAmount = (1f / 100f) * hardEnemy.health;
        }
        else
            GameObject.Destroy(transform.gameObject);
        
    }
}
