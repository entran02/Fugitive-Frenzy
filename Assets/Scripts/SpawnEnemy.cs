using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDistanceBehindPlayer = 150.0f;
    public float spawnInterval = 15.0f;
    public float maxDistanceFromPlayer = 600.0f;

    private GameObject currentEnemy;

    // Start is called before the first frame update
    void Start()
    {
        // spawn enemy 3 seconds after start
        InvokeRepeating("SpawnEnemyBehindPlayer", 3, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemy != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(currentEnemy.transform.position, player.transform.position);
                // if enemy is too far, destroy and spawn new one
                if (distanceToPlayer > maxDistanceFromPlayer)
                {
                    Destroy(currentEnemy);
                    currentEnemy = null;
                    Invoke("SpawnEnemyBehindPlayer", 0);
                }
            }
        }
    }

    void SpawnEnemyBehindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && enemyPrefab != null)
        {
            Vector3 spawnPosition = player.transform.position - player.transform.forward * spawnDistanceBehindPlayer;

            if (currentEnemy != null)
            {
                Destroy(currentEnemy);
            }

            currentEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
