using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float initialMoveSpeed = 35;
    public float acceleration = 4;
    public float maxSpeed = 300;
    public float rubberBandMaxSpeed = 400;
    public float minDistance = 2;
    public float maxDistance = 600;
    public int damageAmount = 34;
    public AudioClip hitSFX;

    private float currentSpeed;
    private AudioSource audioSource;

    private float hitCountdown = 0;

    private bool isFrozen = false;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        currentSpeed = initialMoveSpeed;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.isGameOver)
        {
            audioSource.volume = 0;
            return;
        }

        if (isFrozen)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        AdjustSpeedBasedOnDistance(distance);

        float step = currentSpeed * Time.deltaTime;
        if (hitCountdown > 0)
        {
            hitCountdown -= Time.deltaTime;
            step /= 2;
        }

        if (distance > minDistance)
        {
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }

        print(GetComponent<Rigidbody>().velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var healthManager = collision.gameObject.GetComponent<HealthManager>();
            healthManager.takeDamage(damageAmount);
            AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position);
            isFrozen = true;
            Invoke("unFreeze", 2);
        }
    }

    void AdjustSpeedBasedOnDistance(float distance)
    {
        if (distance > maxDistance)
        {
            currentSpeed = rubberBandMaxSpeed;
        }
        else
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed + ((maxDistance - distance) / maxDistance) * (rubberBandMaxSpeed - maxSpeed));
        }
    }

    public void Hit(int duration)
    {
        hitCountdown = duration;
    }

    public void unFreeze()
    {
        isFrozen = false;
    }
}
