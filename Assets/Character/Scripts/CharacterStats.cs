using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    const float MAX_STAMINA = 100;

    public float staminaConsume;

    [HideInInspector]
    public float stamina;

    private CharacterController characterController;
    void Start()
    {
        stamina = 100;
        characterController = transform.GetComponent<CharacterController>();
    }

    void Update()
    {
        // Restore stamina
        if(!characterController.isRunning)
            RestoreStamina();

        Debug.Log(string.Format("Stamina: {0}", stamina));
    }

    public void ConsumeStamina()
    {
        float newStaminaValue = stamina - (staminaConsume  * Time.deltaTime);
        stamina = newStaminaValue > 0f ? newStaminaValue : 0f;
    }

    public void RestoreStamina()
    {
        float newStaminaValue = stamina + (staminaConsume  * Time.deltaTime);
        stamina = newStaminaValue < 100f ? newStaminaValue : 100f;
    }
}
