using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class LevelManager : MonoBehaviour
{
    public Text gameText;
    public Text scoreText;
    public Text resetText;
    private float startingZPosition;
    private GameObject player;
    public static bool isGameOver = false;
    public string nextLevel;
    public int winDistance = 10000;
    
    public static bool isGameWon = false;

    float distanceTraveled;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // store the starting Z position
            startingZPosition = player.transform.position.z;
        }
        isGameOver = false;
        isGameWon = false;
        resetText.gameObject.SetActive(false);
        gameText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        distanceTraveled = Mathf.Abs(player.transform.position.z - startingZPosition);
        
        if (isGameOver) {
            return;
        }

        // reset level if R is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadCurrentLevel();
        }

        if (player != null)
        {
            scoreText.text = "Score: " + distanceTraveled.ToString("F2");

            // check if cars local z axis is flipped to tell user to reset
            if (Mathf.Abs(player.transform.localEulerAngles.z) > 90 && Mathf.Abs(player.transform.localEulerAngles.z) < 270)
            {
                resetText.gameObject.SetActive(true);
            }
            else
            {
                resetText.gameObject.SetActive(false);
            }

            // if distance is over win condition, call LevelBeat
            if (distanceTraveled >= winDistance)
            {
                LevelBeat();
            }
        }
    }

    void OnDestroy() {
        PlayerPrefs.SetFloat("TotalMinutes", PlayerPrefs.GetFloat("TotalMinutes", 0f) + Time.time / 60);
    }

    public void LevelLost()
    {
        isGameOver = true;
        gameText.color = Color.red;
        gameText.text = "GAME OVER!";
        gameText.gameObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().pitch = .5f;
        Camera.main.GetComponent<AudioSource>().volume = 0;
        Invoke("LoadCurrentLevel", 2);
        PlayerPrefs.SetFloat("TotalDistance", PlayerPrefs.GetFloat("TotalDistance", 0f) + distanceTraveled);

        PlayerPrefs.SetInt("TotalDeaths", PlayerPrefs.GetInt("TotalDeaths", 0) + 1);
    }

    public void LevelBeat()
    {
        isGameOver = true;
        isGameWon = true;
        gameText.color = Color.green;
        gameText.text = "YOU WIN!";
        gameText.gameObject.SetActive(true);
        scoreText.text = "Score: " + winDistance.ToString("F2");

        // Camera.main.GetComponent<AudioSource>().pitch = 2;
        Camera.main.GetComponent<AudioSource>().volume = 0;
        PlayerPrefs.SetFloat("TotalDistance", PlayerPrefs.GetFloat("TotalDistance", 0f) + distanceTraveled);

        if (!string.IsNullOrEmpty(nextLevel))
        {
            Invoke("LoadNextLevel", 2);
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
