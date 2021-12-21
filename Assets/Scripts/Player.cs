using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed, turnSpeed, SmoothTurn;

    Rigidbody rb;

    float refVelocity;
    float smoothInputMagnitude;
    float angle;
    Vector3 velocity;

    private void Awake()
    {
        CinemachineFreeLook freeLook = FindObjectOfType<CinemachineFreeLook>();
        freeLook.Follow = transform;
        freeLook.LookAt = transform;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")).normalized;
        float InputMagintude = input.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, InputMagintude, ref refVelocity, SmoothTurn);

        AngelCalc(input, InputMagintude);

        velocity = transform.forward * speed * smoothInputMagnitude;
    }

    private void FixedUpdate()
    {
        Rotation(angle);
        Movement(velocity);
    }

    void AngelCalc(Vector3 input, float InputMagnitude)
    {
        float targetAngle = Mathf.Atan2(input.z, input.x) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed * InputMagnitude * Time.deltaTime);
    }

    void Movement(Vector3 velocity)
    {
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    void Rotation(float angle)
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
    }
}
