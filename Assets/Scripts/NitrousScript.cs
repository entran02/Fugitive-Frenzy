using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitrousScript : MonoBehaviour
{

    public float nitrousDuration = 5f;
    public AudioClip nitroSFX;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.tag == "Player")
        {
            Debug.Log("Nitrous");
            CarController carController = other.GetComponent<CarController>();
            carController.activateNitrous(nitrousDuration);
            AudioSource.PlayClipAtPoint(nitroSFX, transform.position);

            // Destroy(gameObject, 3f);
        }
        
    }
}
