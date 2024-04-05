using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlickScript : MonoBehaviour
{
    public int duration = 5;

    public bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = active;
        Invoke("detachParent", 0.5f);  // detach to prevent it bobbing up and down
    }

    private void detachParent()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activate()
    {
        active = true;
        Invoke("displayRender", 0.5f);  // allow enough time for player to pass through
    }

    private void displayRender()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = active;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && active)
        {
            EnemyBehavior enemyBehavior = other.GetComponent<EnemyBehavior>();
            enemyBehavior.Hit(duration);
            
            Destroy(gameObject, duration);
        }
        
    }
}
