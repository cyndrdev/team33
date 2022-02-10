using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    public float maxHealth, currentHealth;
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            takeDamage(1);
        }
    }
    public void takeDamage(float damage) 
    {
        currentHealth -= damage;
    }
    void Update()
    {
        healthSlider.value = currentHealth;
        if (currentHealth <= 0f)
        {
            die();
        }
    }
    public void die()
    {
        Destroy(gameObject);
    }
}
