using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public int minHealth = 0;
    public int regenPeriod = 10; // time in seconds before health starts regenerating
    public int regenAmount = 2; // amount of health to regenerate per second

    public int smokeEffectThreshold = 50;

    public GameObject smokeEffect;
    public Slider healthSlider;

    private float lastDamageTaken = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (smokeEffect != null)
        {
            smokeEffect.SetActive(false);
        }
        lastDamageTaken = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastDamageTaken > regenPeriod && health < maxHealth)
        {
            lastDamageTaken += 1;  // increment to prevent regen from happening every frame
            health += regenAmount;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            if (health > smokeEffectThreshold)
            {
                smokeEffect.SetActive(false);
            }
        }
        updateSlider();
        
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        lastDamageTaken = Time.time;

        if (health <= smokeEffectThreshold)
        {
            smokeEffect.SetActive(true);
        }

        if (health <= minHealth)
        {
            health = minHealth;
            Debug.Log("Health is 0");
            FindObjectOfType<LevelManager>().LevelLost();
        }
    }

    public void takeHealth(int healthAmount)
    {
        health += healthAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (health > smokeEffectThreshold)
        {
            smokeEffect.SetActive(false);
        }
    }

    private void updateSlider()
    {
        healthSlider.value = health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Explosion"))
        {
            takeDamage(34);
        }
    }
}
