using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float motorForce;
    public float breakForce;
    public float maxSteerAngle = 30;
    public float jumpAmount = 3000;

    public float airControlForce = 100000;

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

    public AudioClip engineStartSFX;
    private bool hasStarted = false;
    public AudioClip engineRunningSFX;
    public AudioClip brakeSFX;
    public AudioClip nitroSFX;
    public AudioClip jumpSFX;
    public AudioClip gameWonSFX;

    public AudioSource carAudioSource;
    public AudioSource levelAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        carAudioSource.transform.position = Camera.main.transform.position;
        levelAudioSource.transform.position = Camera.main.transform.position;

        PlayEngineStartSFX();

        rb = GetComponent<Rigidbody>();

        // frontLeftWheelCollider.mass = 100;
        // frontRightWheelCollider.mass = 100;
        // rearLeftWheelCollider.mass = 50;
        // rearRightWheelCollider.mass = 50;
        SetUpWheelCollider(frontLeftWheelCollider);
        SetUpWheelCollider(frontRightWheelCollider);
        SetUpWheelCollider(rearLeftWheelCollider);
        SetUpWheelCollider(rearRightWheelCollider);
    }

    private void SetUpWheelCollider(WheelCollider wheelCollider) {
        wheelCollider.mass = 100;
        wheelCollider.suspensionDistance = 0.2f;
        wheelCollider.forceAppPointDistance = 0.1f;
        wheelCollider.suspensionSpring = new JointSpring {
            spring = 35000,
            damper = 4500,
            targetPosition = 0
        };
        wheelCollider.forwardFriction = new WheelFrictionCurve {
            asymptoteSlip = 2,
            asymptoteValue = 15,
            extremumSlip = 1,
            extremumValue = 20,
            stiffness = 1
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !IsAirborne()) {
            Debug.Log("Jumping");
            rb.AddForce(Vector3.up * jumpAmount * 1000);
            PlayJumpSFX();
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
        if (isBreaking) {
            if (carAudioSource.clip != brakeSFX) {
                carAudioSource.clip = brakeSFX;
                carAudioSource.loop = false;
                carAudioSource.volume = 1f;
                carAudioSource.Play();
            }
        } else if (!carAudioSource.isPlaying && carAudioSource.clip != engineRunningSFX) {
            PlayEngineRunningSFX();
        }

        if (IsAirborne()) {
            Debug.Log("Airborne");
            rb.AddForce(transform.up * Physics.gravity.y * 5, ForceMode.Acceleration);
            rb.AddTorque(transform.right * airControlForce * verticalInput);
            rb.AddTorque(-1 * transform.forward * airControlForce * horizontalInput);
        }

        HandleMotor();
        HandleSteering();
        UpdateWheels();

        if (LevelManager.isGameOver && levelAudioSource.clip != gameWonSFX) {
            carAudioSource.volume = 0;
            PlayGameWonSFX();
        }
    }

    private bool IsAirborne() {
        return !Physics.Raycast(transform.position, Vector3.down, 1.0f);
    }

    private void HandleMotor()
    {
        float effectiveMotorForce = motorForce;
        if(isNitrous) {
            effectiveMotorForce *= 4;
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
        PlayNitrousSFX();
    }

    private void toggleNitrousEffect(bool isOn) {
        TrailRenderer[] trails = nitrousTrails.GetComponentsInChildren<TrailRenderer>();
        foreach(TrailRenderer trail in trails) {
            trail.emitting = isOn;
        }
    }
    
    private void PlayNitrousSFX() {
        levelAudioSource.clip = nitroSFX;
        levelAudioSource.loop = false;
        levelAudioSource.volume = 0.5f;
        levelAudioSource.Play();
    }

    private void PlayGameWonSFX() {
        levelAudioSource.clip = gameWonSFX;
        levelAudioSource.loop = false;
        levelAudioSource.volume = 0.5f;
        levelAudioSource.Play();
    }

    private void PlayEngineStartSFX() {
        carAudioSource.clip = engineStartSFX;
        carAudioSource.loop = false;
        carAudioSource.volume = 1f;
        carAudioSource.Play();
        hasStarted = true;
    }

    private void PlayEngineRunningSFX() {
        if (carAudioSource.clip != engineRunningSFX && hasStarted && !LevelManager.isGameOver) {
            carAudioSource.clip = engineRunningSFX;
            carAudioSource.loop = true;
            carAudioSource.volume = 0.05f;
            carAudioSource.Play();
        }
    }

    private void PlayJumpSFX() {
        carAudioSource.clip = jumpSFX;
        carAudioSource.loop = false;
        carAudioSource.volume = 1f;
        carAudioSource.Play();
    }
}
