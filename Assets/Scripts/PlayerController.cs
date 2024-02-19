using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpAmount = 6;
    public float rotationSpeed = 100.0f; // Speed at which wheels rotate
    public float steeringAngle = 30.0f; // Max steering angle for the front wheels

    public GameObject car_fl_wheel_mesh;
    public GameObject car_fr_wheel_mesh;
    public GameObject car_rr_wheel_mesh;
    public GameObject car_rl_wheel_mesh;

    Rigidbody rb;
    AudioSource jumpSFX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(transform.position.y < 1){
                rb.AddForce(0, jumpAmount, 0, ForceMode.Impulse);
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            speed *= 2;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)) {
            speed /= 2;
        }

        // Rotate front wheels for steering
        float steerDirection = Input.GetAxis("Horizontal") * steeringAngle;
        car_fl_wheel_mesh.transform.localEulerAngles = new Vector3(0, 0, steerDirection);
        car_fr_wheel_mesh.transform.localEulerAngles = new Vector3(0, 0, steerDirection);
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = -Input.GetAxis("Vertical");

        // Adjusting the force vector to account for the car's rotated orientation
        Vector3 forceVector = new Vector3(0.0f, 0.0f, moveVertical);
        rb.AddForce(transform.TransformDirection(forceVector) * speed);

        // Simulate wheel rotation
        float rotation = moveVertical * rotationSpeed * Time.fixedDeltaTime;
        RotateWheels(car_fl_wheel_mesh, car_fr_wheel_mesh, car_rr_wheel_mesh, car_rl_wheel_mesh, rotation);
    }

    void RotateWheels(GameObject fl, GameObject fr, GameObject rr, GameObject rl, float rotation)
    {
        fl.transform.Rotate(Vector3.right, rotation);
        fr.transform.Rotate(Vector3.right, rotation);
        rr.transform.Rotate(Vector3.right, rotation);
        rl.transform.Rotate(Vector3.right, rotation);
    }
}
