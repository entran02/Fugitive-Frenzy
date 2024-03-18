using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Text gameText;
    public Text scoreText;
    public Text resetText;
    public AudioClip gameWonSFX;
    private float startingZPosition;
    private GameObject player;
    public static bool isGameOver = false;
    public string nextLevel;
    public int winDistance = 10000;

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
        resetText.gameObject.SetActive(false);
        gameText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        // reset level if R is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadCurrentLevel();
        }

        if (player != null)
        {
            float distanceTraveled = Mathf.Abs(player.transform.position.z - startingZPosition);
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

    public void LevelLost()
    {
        isGameOver = true;
        gameText.text = "GAME OVER!";
        gameText.gameObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().pitch = .5f;
        Invoke("LoadCurrentLevel", 2);
    }

    public void LevelBeat()
    {
        isGameOver = true;
        gameText.text = "YOU WIN!";
        gameText.gameObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().pitch = 2;
        AudioSource.PlayClipAtPoint(gameWonSFX, Camera.main.transform.position);

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
