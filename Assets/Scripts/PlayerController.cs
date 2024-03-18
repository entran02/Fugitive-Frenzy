using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float motorForce;
    public float breakForce;
    public float maxSteerAngle = 30;
    public float jumpAmount = 15000;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontLeftWheelMesh;
    public Transform frontRightWheeMesh;
    public Transform rearLeftWheelMesh;
    public Transform rearRightWheelMesh;

    public GameObject nitrousTrails;

    float horizontalInput;
    float verticalInput;
    float currentSteerAngle;
    float currentbreakForce;
    bool isBreaking;
    Rigidbody rb;
    bool isNitrous = false;
    float nitrousEndTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        frontLeftWheelCollider.mass = 100;
        frontRightWheelCollider.mass = 100;
        rearLeftWheelCollider.mass = 50;
        rearRightWheelCollider.mass = 50;
    }

    // Update is called once per frame
    void Update()
    {
        // **fix jumping
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(transform.position.y < 1){
                rb.AddForce(0, jumpAmount, 0, ForceMode.Impulse);
            }
        }
    }
    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.LeftShift);
        if(isNitrous && Time.time > nitrousEndTime) {
            isNitrous = false;
            toggleNitrousEffect(false);
            Debug.Log("Nitrous off");
        }
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMotor()
    {
        float effectiveMotorForce = motorForce;
        if(isNitrous) {
            effectiveMotorForce *= 5;
        }
        frontLeftWheelCollider.motorTorque = verticalInput * effectiveMotorForce;
        frontRightWheelCollider.motorTorque = verticalInput * effectiveMotorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * effectiveMotorForce * 0.25f;
        rearRightWheelCollider.motorTorque = verticalInput * effectiveMotorForce * 0.25f;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();       
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelMesh);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeMesh);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelMesh);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelMesh);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void activateNitrous(float duration) {
        isNitrous = true;
        nitrousEndTime = Time.time + duration;
        toggleNitrousEffect(true);
    }

    private void toggleNitrousEffect(bool isOn) {
        TrailRenderer[] trails = nitrousTrails.GetComponentsInChildren<TrailRenderer>();
        foreach(TrailRenderer trail in trails) {
            trail.emitting = isOn;
        }
    }
}
