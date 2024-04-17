using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
        public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }

    public FSMStates currentState;

    public float attackDistance = 2;
    public float chaseDistance = 10;
    public float enemySpeed = 5;
    public GameObject player;
    public int damageAmount = 20;

    GameObject[] wanderPoints;
    Vector3 nextDestination;
    //Animator anim;
    float distanceToPlayer;
    float elapsedTime = 0;

    int currentDestinationIndex = 0;

    NavMeshAgent agent;

    public Transform enemyEyes;
    public float fieldOfView = 60f;
    Animator animator;

    public AudioClip kickSFX;

    // Start is called before the first frame update
    void Start()
    {
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        //anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch(currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
        }

        elapsedTime += Time.deltaTime;

        if (PickupLevelManager.isGameOver) {
            Destroy(gameObject);
        }
    }

    void Initialize()
    {
        currentState = FSMStates.Patrol;
        FindNextPoint();
    }

    void UpdatePatrolState()
    {
        //anim.SetInteger("animState", 1);

        agent.stoppingDistance = 0;
        agent.speed = 3.5f;

        if(Vector3.Distance(transform.position, nextDestination) < 1)
        {
            FindNextPoint();
        }
        else if(IsPlayerInClearFOV())
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);
    }

    void UpdateChaseState()
    {
        //anim.SetInteger("animState", 2);
       animator.SetBool("attack", false);  


        nextDestination = player.transform.position;

        agent.speed = 5;
        
        if(distanceToPlayer > chaseDistance)
        {
            FindNextPoint();
            currentState = FSMStates.Patrol;
        }
        if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);
    }

    void UpdateAttackState() {
       agent.isStopped = true;
       animator.SetBool("attack", true);  

       FaceTarget(GameObject.FindGameObjectWithTag("Player").transform.position);

       if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance) {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance) {
            currentState = FSMStates.Patrol;
        }
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;

        agent.SetDestination(nextDestination);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;

        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if(Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if(Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }

    void takeDamage() {
        var playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damageAmount);
        AudioSource.PlayClipAtPoint(kickSFX, transform.position);
    }

}
