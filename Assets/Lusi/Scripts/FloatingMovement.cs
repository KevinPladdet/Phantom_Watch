using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    public float floatAmplitude = 0.5f;
    public float floatSpeed = 1.0f;
    public float rotationAngle = 10f;
    public float rotationSpeed = 1.0f;
    public Vector3 startingRotation = new Vector3(-90f, 0f, -183.158f);

    private Vector3 startPosition;
    private Quaternion baseRotation;

    void Start()
    {
        baseRotation = Quaternion.Euler(startingRotation);
        transform.rotation = baseRotation;
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        float tiltZ = Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        transform.rotation = baseRotation * Quaternion.Euler(0, 0, tiltZ);
    }
}