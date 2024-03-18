using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateSection : MonoBehaviour
{
    public int width;
    static int maxSections = 5;
    GameObject[] sections;
    static List<GameObject> orderedSections;
    
    // Start is called before the first frame update
    void Start()
    {
        sections = Resources.LoadAll<GameObject>("Prefabs"); // need this in case one of the sections isn't active
        orderedSections = GameObject.FindGameObjectsWithTag("section").ToList();
        orderedSections.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));

        if (orderedSections.Count > maxSections) {
            GameObject first = orderedSections[0];
            orderedSections.RemoveAt(0);
            Destroy(first, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        int randomIndex = UnityEngine.Random.Range(0, sections.Length);
        GameObject section = sections[randomIndex];
        addSection(section);
    }

    void addSection(GameObject section) {
        Vector3 position = gameObject.transform.position + new Vector3(0, 0, width);

        bool startAdd = false;

        foreach(GameObject element in orderedSections) {
            if (startAdd) {
                position += new Vector3(0, 0, element.GetComponent<GenerateSection>().width);
            }
            else if (element == gameObject) {
                startAdd = true;
            } 
        } 
        
        orderedSections.Add(section);

        Instantiate(section, position, gameObject.transform.rotation);
    }
}
