using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyPickup : MonoBehaviour
{
    public static int totalPickups = 0;
    public static int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        totalPickups++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {

        score++;
        Destroy(gameObject);
        GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().Play();
    }

    private void OnDestroy() {
        if (!PickupLevelManager.isGameOver) {
            if (score == totalPickups) {
                PickupLevelManager.isGameWon = true;
            }
        }
    }


}
