using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    const float MAX_STATS_VALUE = 100f;
    const float MIN_STATS_VALUE = 0f;

    public float staminaConsume, manaConsume;

    [HideInInspector]
    public float stamina;
    [HideInInspector]
    public float mana;

    private CharacterController characterController;
    private Scythe scythe;
    void Start()
    {
        stamina = mana = MAX_STATS_VALUE;
        characterController = transform.GetComponent<CharacterController>();
        scythe = Object.FindObjectOfType<Scythe>();
    }

    void Update()
    {
        // Restore stamina
        if(!characterController.isRunning)
            AutoRestoreStamina();

        // Restore mana
        if(!scythe.isSpecial)
            AutoRestoreMana();

        Debug.Log(string.Format("Stamina: {0} Mana: {1}",  (int)stamina, (int)mana));
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
}
