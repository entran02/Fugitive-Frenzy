using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickupLevelManager : MonoBehaviour
{
    public Text score;
    public static bool isGameOver = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Collected: " + MoneyPickup.score.ToString() + "/" + MoneyPickup.totalPickups.ToString();

        if (MoneyPickup.score.ToString() == MoneyPickup.totalPickups.ToString()) {
            isGameOver = true;
        }

        if (isGameOver) {
            SceneManager.LoadScene(2);
        }
    }
}
