using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    const float MAX_STATS_VALUE = 100f;
    const float MIN_STATS_VALUE = 0f;

    public float staminaConsume, manaConsume;
    public Image healthBar, manaBar, staminaBar; 

    [HideInInspector]
    public float stamina;
    [HideInInspector]
    public float mana;
    [HideInInspector]
    public float health;

    private CharacterController characterController;
    private Scythe scythe;
    void Start()
    {
        stamina = mana = health = MAX_STATS_VALUE;
        characterController = transform.GetComponent<CharacterController>();
        scythe = Object.FindObjectOfType<Scythe>();
    }

    void Update()
    {
        // Restore stamina
        if(!characterController.isRunning)
            AutoRestoreStamina();

        manaBar.fillAmount = (1f / 100f) * mana;
        healthBar.fillAmount = (1f / 100f) * health;
        staminaBar.fillAmount = (1f / 100f) * stamina;
    }

    public void ConsumeStamina()
    {
        float newStaminaValue = stamina - (staminaConsume  * Time.deltaTime);
        stamina = newStaminaValue > MIN_STATS_VALUE ? newStaminaValue : MIN_STATS_VALUE;
    }

    public void ConsumeMana()
    {
        float newManaValue = mana - (manaConsume  * Time.deltaTime);
        mana = newManaValue > MIN_STATS_VALUE ? newManaValue : MIN_STATS_VALUE;
    }

    public void AutoRestoreStamina()
    {
        float newStaminaValue = stamina + (staminaConsume  * Time.deltaTime);
        stamina = newStaminaValue < MAX_STATS_VALUE ? newStaminaValue : MAX_STATS_VALUE;
    }

    public void AutoRestoreMana()
    {
        float newManaValue = mana + (manaConsume  * Time.deltaTime);
        mana = newManaValue < MAX_STATS_VALUE ? newManaValue : MAX_STATS_VALUE;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health < MIN_STATS_VALUE)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);    
    }

    public void RestoreMana(float amount)
    {
        mana = mana + amount < MAX_STATS_VALUE ? mana + amount : MAX_STATS_VALUE;
    }

    public void RestoreHealth(float amount)
    {
        health = health + amount < MAX_STATS_VALUE ? health + amount : MAX_STATS_VALUE;
    }
}
