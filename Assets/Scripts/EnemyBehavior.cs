using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 90;
     public float acceleration = 2;
     public float maxSpeed = 130;
    public float minDistance = 2;
    public int damageAmount = 34;
    float currentSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        currentSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = Mathf.Min(currentSpeed + (acceleration * Time.deltaTime), maxSpeed);
        float step = currentSpeed * Time.deltaTime;
        
        float distance = Vector3.Distance(transform.position, player.position);

        if(distance > minDistance)
        {
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var healthManager = other.GetComponent<HealthManager>();
            healthManager.takeDamage(damageAmount);
        }
    }
}
