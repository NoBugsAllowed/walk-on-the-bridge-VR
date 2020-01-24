using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRotation : MonoBehaviour
{
    private const float MAX_ANGLE = 10.0f;
    private const float ROTATION_POINT_Y_OFFSET = 8.0f;
    private const float ROTATION_POINT_Z_OFFSET = 4.0f;
    private float angle = 0;
    private Vector3 rotateAroundPoint;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public double Offset { get; set; }

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rotateAroundPoint = new Vector3(transform.position.x, transform.position.y + ROTATION_POINT_Y_OFFSET, transform.position.z - ROTATION_POINT_Z_OFFSET);
    }

    void Update()
    {
        // Update bridge rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.RotateAround(rotateAroundPoint, Vector3.right, OffsetToAngle(Offset));
    }

    private float OffsetToAngle(double offset)
    {
        return (float)(-Offset * MAX_ANGLE);
    }
}
