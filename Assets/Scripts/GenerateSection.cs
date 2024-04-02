using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateSection : MonoBehaviour
{
    public int length;
    public int spawnWidth;
    static int maxSections = 10;
    public GameObject[] sections;
    static List<GameObject> orderedSections;

    public GameObject obstaclePrefab;
    public GameObject nitroPrefab;
    public float obstacleSpawnChance = 0.4f;
    public float nitroSpawnChance = 0.3f;
    
    // Start is called before the first frame update
    void Start()
    {
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
        Vector3 position = gameObject.transform.position + new Vector3(0, 0, length);

        bool startAdd = false;

        foreach(GameObject element in orderedSections) {
            if (startAdd) {
                position += new Vector3(0, 0, element.GetComponent<GenerateSection>().length);
            }
            else if (element == gameObject) {
                startAdd = true;
            } 
        } 
        
        orderedSections.Add(section);

        Instantiate(section, position, section.transform.rotation);
    }

    void SpawnObstacles() {
        float randomX = Random.Range(-spawnWidth, spawnWidth);
        float randomZ = Random.Range(length / 2, length);
        if (Random.value < obstacleSpawnChance) {
            GameObject obj = Instantiate(obstaclePrefab, new Vector3(gameObject.transform.position.x + randomX, gameObject.transform.position.y, gameObject.transform.position.z + randomZ), obstaclePrefab.gameObject.transform.rotation);
            obj.transform.parent = GameObject.FindGameObjectWithTag("PropParent").transform;
        }
    }

    void SpawnNitro() {
        float randomX = Random.Range(-spawnWidth, spawnWidth);
        float randomZ = Random.Range(length / 2, length);
        if (Random.value < nitroSpawnChance) {
            GameObject obj = Instantiate(nitroPrefab, new Vector3(gameObject.transform.position.x + randomX, gameObject.transform.position.y + 20, gameObject.transform.position.z + randomZ), nitroPrefab.gameObject.transform.rotation);
            obj.transform.parent = GameObject.FindGameObjectWithTag("PropParent").transform;
        }
    }
}
