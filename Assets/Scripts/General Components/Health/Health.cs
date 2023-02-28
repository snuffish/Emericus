using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] float hurtCooldownTime;
    private bool canTakeDamage;



    void Start()
    {
        currentHealth = maxHealth;
        canTakeDamage = true;
    }

    public void Hurt(int HP)
    {
        if (canTakeDamage)
        {
            canTakeDamage = false;
            StartCoroutine(DamageCooldown());

            currentHealth -= HP;
            if (currentHealth <= 0)
                Die();

            Debug.Log($"Took {HP} damage, {currentHealth} to go!");
        }

    }

    public void Heal(int HP)
    {
        currentHealth += HP;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " has died.");
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(hurtCooldownTime);
        canTakeDamage = true;
    }
}