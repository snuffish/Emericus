using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;



    void Start() {
        health = maxHealth;
    }

    public void TakeDamage(int HP) {
        health -= HP;
        if (health <= 0)
            Die();
    }

    public void Heal(int HP) {
        health += HP;
        if (health > maxHealth)
            health = maxHealth;
    }

    private void Die() {
        Debug.Log(gameObject.name + " has died.");
    }
}
