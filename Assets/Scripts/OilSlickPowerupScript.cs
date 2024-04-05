using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class OilSlickPowerupScript : MonoBehaviour
{
    public GameObject oilSlick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            oilSlick.GetComponent<OilSlickScript>().activate();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
