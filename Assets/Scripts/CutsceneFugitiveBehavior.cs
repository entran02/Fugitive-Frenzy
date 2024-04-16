using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CutsceneFugitiveBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    public static bool runningToCar = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(GameObject.FindGameObjectWithTag("Home").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Home").transform.position) < 0.5 && !runningToCar) {
            gameObject.SetActive(false);
            Invoke("RunToCar", 3);
        }

        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Target").transform.position) < 0.5) {
            SceneManager.LoadScene(3);
        }
    }

    void RunToCar() {
        gameObject.SetActive(true);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y + 180, currentRotation.z);
        agent.SetDestination(GameObject.FindGameObjectWithTag("Target").transform.position);
        runningToCar = true;

    }
}
