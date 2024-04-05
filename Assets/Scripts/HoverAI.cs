using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Stunned
    }

    public EnemyState currentState = EnemyState.Idle;
    public Transform target; 
    public Transform player;
    public GameObject explosionPrefab;
    public float explosionSpawnDistance = 330f;
    public float chaseSpeed = 400f;
    public float attackRange = 10f;
    public float stunDuration = 3f;
    public float idleDuration = 5f;
    public float attackInterval = 2f;
    //public AudioClip hoverSFX;
    public AudioClip rocketSFX;

    private float stateChangeTime = 0f;
    private float stunTimeEnd = 0f;
    private float nextAttackTime = 0f;
    public UIManager uiManager;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Lead").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        stateChangeTime = Time.time + idleDuration;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Chase:
                Chase();
                CheckAttackCondition();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Stunned:
                Stunned();
                break;
        }
    }

    void ChangeState(EnemyState newState)
    {
        currentState = newState;
        if (newState == EnemyState.Stunned)
        {
            stunTimeEnd = Time.time + stunDuration;
        }
    }

    void Idle()
    {
        if (Time.time >= stateChangeTime)
        {
            ChangeState(EnemyState.Chase);
        }
    }

    public void Hit()
    {
        ChangeState(EnemyState.Stunned);
    }

    void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
    }

    void CheckAttackCondition()
    {
        if (Vector3.Distance(transform.position, target.position) <= attackRange && Time.time >= nextAttackTime)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackInterval;
            StartCoroutine(AttackSequence());
        }
    }

    IEnumerator AttackSequence()
    {
        uiManager.ShowWarning();

        yield return new WaitForSeconds(1f);

        Vector3 spawnPosition = player.position + player.forward * explosionSpawnDistance;
        Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);
        AudioSource.PlayClipAtPoint(rocketSFX, Camera.main.transform.position);

        ChangeState(EnemyState.Chase);
    }

    void Stunned()
    {
        if (Time.time >= stunTimeEnd)
        {
            ChangeState(EnemyState.Chase);
        }
    }
}
