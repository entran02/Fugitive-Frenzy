using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CutsceneFugitiveBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);

        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 0.5) {
            gameObject.SetActive(false);
            SceneManager.LoadScene(3);
            // Destroy(gameObject);
        }
    }
}
