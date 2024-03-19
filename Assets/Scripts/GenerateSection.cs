using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateSection : MonoBehaviour
{
    public int width;
    static int maxSections = 10;
    GameObject[] sections;
    static List<GameObject> orderedSections;

    public GameObject obstaclePrefab;
    public GameObject nitroPrefab;
    public float obstacleSpawnChance = 0.4f;
    public float nitroSpawnChance = 0.3f;

    void SpawnObstacles() {
        float randomX = Random.Range(-100f, 0f);
        float randomZ = Random.Range(width / 2, width);
        if (Random.value < obstacleSpawnChance) {
            Instantiate(obstaclePrefab, new Vector3(randomX, 0, gameObject.transform.position.z + randomZ), obstaclePrefab.gameObject.transform.rotation);
        }
    }

    void SpawnNitro() {
        float randomX = Random.Range(-100f, 0f);
        float randomZ = Random.Range(width / 2, width);
        if (Random.value < nitroSpawnChance) {
            Instantiate(nitroPrefab, new Vector3(randomX, 0, gameObject.transform.position.z + randomZ), nitroPrefab.gameObject.transform.rotation);
        }
    }
    
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

        SpawnObstacles();
        SpawnNitro();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            int randomIndex = UnityEngine.Random.Range(0, sections.Length);
            GameObject section = sections[randomIndex];
            addSection(section);
        }
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
