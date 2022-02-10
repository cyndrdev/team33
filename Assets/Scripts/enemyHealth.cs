using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
public class enemyHealth : MonoBehaviour
{
    public float maxHealth, currentHealth;
    public Slider healthSlider;
    public AIPath aipath;
    public Transform Sprite;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("flailHead"))
        {
            takeDamageEnemy(1);
        }
    }
    public void takeDamageEnemy(float damage) 
    {
        currentHealth -= damage;
    }
    void Update()
    {
        if (aipath.desiredVelocity.x >= 0.1f)
        {
            Sprite.localScale = new Vector3(1f, 1f, 1f);
        } else if (aipath.desiredVelocity.x <= -0.1f)
        {
            Sprite.localScale = new Vector3(-1f, 1f, 1f);
        }
        healthSlider.value = currentHealth;
        if (currentHealth <= 0f)
        {
            dieEnemy();
        }
    }
    public void dieEnemy()
    {
        Destroy(gameObject);
    }
}
