using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitrousScript : MonoBehaviour
{

    public float nitrousDuration = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.isGameOver) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Nitrous");
            CarController carController = other.GetComponent<CarController>();
            carController.activateNitrous(nitrousDuration);
            gameObject.SetActive(false);
            Destroy(gameObject, 3f);

            PlayerPrefs.SetInt("TotalNitro", PlayerPrefs.GetInt("TotalNitro", 0) + 1);
        }
        
    }
}
