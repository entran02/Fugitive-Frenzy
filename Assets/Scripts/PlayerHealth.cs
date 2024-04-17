using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public Slider healthSlider;

    int currentHealth;
    bool isAlive = true;
    public AudioClip arrestSFX;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthSlider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();
        healthSlider.value = currentHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        if(isAlive == true)
        {
            if(currentHealth > 0)
            {
                currentHealth -= damageAmount;
                healthSlider.value = currentHealth;
            }
            if(currentHealth <= 0)
            {
                PlayerDies();
            }
        }
    }

    void PlayerDies()
    {
        isAlive = false;
        LevelManager.isGameOver = true;
        FindObjectOfType<PickupLevelManager>().LevelLost();
        AudioSource.PlayClipAtPoint(arrestSFX, transform.position);
        MoneyPickup.totalPickups = 0;
    }
       
}
