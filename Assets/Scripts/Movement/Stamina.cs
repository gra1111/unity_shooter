using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider stamina;

    public float maxStamina = 100;
    public float currentStamina;

    private float regTime = 0.3f;
    private float loseTime = 0.1f;

    private float regAmount = 2;
    private float loseAmount = 2;  

    private float loseTimer = 0f;
    private float regenTimer = 0f;
    private float timeSinceLastUse = 0f;

    void Start()
    {
        currentStamina = maxStamina;
        stamina.maxValue = maxStamina;
        stamina.value = currentStamina;
    }

    private void Update()
    {
        float amount = 0f;  

        if (Input.GetKey(KeyCode.LeftShift))
        {
            timeSinceLastUse = 0f;
            loseTimer += Time.deltaTime;

            if (loseTimer >= loseTime)
            {
                loseTimer = 0f;

                if (currentStamina > 0)
                    amount = -loseAmount;
            }
        }
        else
        {
            timeSinceLastUse += Time.deltaTime;

            if (timeSinceLastUse >= 1f)
            {
                regenTimer += Time.deltaTime;

                if (regenTimer >= regTime)
                {
                    regenTimer = 0f;

                    if (currentStamina < maxStamina)
                        amount = regAmount;
                }
            }
        }

        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);
        stamina.value = currentStamina;
    }
}
