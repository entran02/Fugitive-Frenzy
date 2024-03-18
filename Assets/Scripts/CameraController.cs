using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    Vector3 offset;
    bool isLookingBack = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isLookingBack = !isLookingBack;
        }

        if (isLookingBack)
        {
            transform.position = player.transform.position - offset;
            transform.LookAt(player.transform.position);
            transform.RotateAround(player.transform.position, Vector3.left, 60);
        }
        else
        {
            transform.position = player.transform.position + offset;
            transform.LookAt(player.transform.position);
        }
    }
}
