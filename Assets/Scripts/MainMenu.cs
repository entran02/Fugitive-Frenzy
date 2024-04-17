using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene(1);
        MoneyPickup.score = 0;
    }

    public void ExitGame() {
        Application.Quit();
    }
}
