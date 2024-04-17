using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickupLevelManager : MonoBehaviour
{
    public Text score;
    public Text gameText;
    public static bool isGameOver = false;
    public static bool isGameWon = false;


    // Start is called before the first frame update
    void Start()
    {
        MoneyPickup.score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Collected: " + MoneyPickup.score.ToString() + "/" + MoneyPickup.totalPickups.ToString();

        if (MoneyPickup.score.ToString() == MoneyPickup.totalPickups.ToString()) {
            isGameWon = true;
        }

        if (isGameWon) {
            SceneManager.LoadScene(2);
        }
    }

    public void LevelLost()
    {
        isGameOver = true;
        gameText.color = Color.red;
        gameText.text = "BUSTED!";
        gameText.gameObject.SetActive(true);

        Invoke("LoadCurrentLevel", 2);
    }

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isGameOver = false;
    }
}
