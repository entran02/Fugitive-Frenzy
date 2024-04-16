using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            PlayerPrefs.SetInt("TotalOil", PlayerPrefs.GetInt("TotalOil", 0) + 1);
        }
    }
}
