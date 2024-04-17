using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCutsceneBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    bool called = false;
    bool changeDestination = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(GameObject.FindGameObjectWithTag("Home").transform.position);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Home").transform.position) < 0.5 && !called) {
            agent.isStopped = true;
            animator.SetBool("argue", true);
        }

        if (CutsceneFugitiveBehavior.runningToCar && !called) {
            called = true;
            changeDestination = true;
            agent.isStopped = false;
            animator.SetBool("argue", false);
            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y + 180, currentRotation.z);
        }

        if (changeDestination && GameObject.FindGameObjectWithTag("Player") != null) {
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        }



    }
}
