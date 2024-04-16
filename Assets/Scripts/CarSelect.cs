using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelect : MonoBehaviour
{
    public Texture[] textures;
    public GameObject carPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTexture(int index) {
        Transform body = carPrefab.transform.Find("Body");
        foreach (Material material in body.GetComponent<Renderer>().sharedMaterials) {
            if (material.name.Contains("AFRC_Mat")) {
                material.SetTexture("_MainTex", textures[index]);
            }
        }
    }
}
