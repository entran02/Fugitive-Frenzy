using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool isGamePaused = false;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }
    }

    public void ResumeGame() {
        isGamePaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void PauseGame() {
        isGamePaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void ExitGame() {
        Application.Quit();
    }
}
