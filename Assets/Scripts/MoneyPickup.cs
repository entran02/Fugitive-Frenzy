using System.Collections;
using System.Collections.Generic;
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
        // if (other.CompareTag("Player")) {
        //     SceneManager.LoadScene(2);
        // }
        score++;
        Destroy(gameObject);
    }
}
