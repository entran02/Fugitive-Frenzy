using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    //public float spawnDistanceBehindPlayer = 150.0f;
    //public float spawnInterval = 15.0f;
    public float minDistanceFromPlayer = 100.0f;
    public float maxDistanceFromPlayer = 600.0f;
    public float baseSpeed = 10.0f;
    public float maxSpeed = 20.0f;

    private GameObject currentEnemy;
    private float  currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // spawn enemy 3 seconds after start
        Invoke("SpawnEnemyBehindPlayer", 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemy != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
               AdjustEnemySpeed(player);
               MoveEnemyTowardsPlayer(player);
            }
        }
        else
        {
            Invoke("SpawnEnemyBehindPlayer", 3.0f);
        }
    }

    void AdjustEnemySpeed(GameObject player)
    {
        float distanceToPlayer = Vector3.Distance(currentEnemy.transform.position, player.transform.position);
        // Adjust speed based on distance to player
        if (distanceToPlayer > maxDistanceFromPlayer)
        {
            currentSpeed = maxSpeed;
        }
        else
        {
            // Calculate speed based on distance
            float speedRatio = (distanceToPlayer - minDistanceFromPlayer) / (maxDistanceFromPlayer - minDistanceFromPlayer);
            currentSpeed = Mathf.Lerp(baseSpeed, maxSpeed, 1 - Mathf.Clamp01(speedRatio));
        }
    }

    void MoveEnemyTowardsPlayer(GameObject player)
    {
        // Move the enemy towards the player at the current speed
        Vector3 direction = (player.transform.position - currentEnemy.transform.position).normalized;
        currentEnemy.transform.position += direction * currentSpeed * Time.deltaTime;
    }

    void SpawnEnemyBehindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && enemyPrefab != null)
        {
            Vector3 spawnPosition = player.transform.position - player.transform.forward * minDistanceFromPlayer;

            currentEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            currentSpeed = baseSpeed;
        }
    }
}
