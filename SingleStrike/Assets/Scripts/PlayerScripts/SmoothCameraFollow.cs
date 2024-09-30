using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;  // The target (cube/player) to follow
    public float smoothSpeed = 0.125f;  // How fast the camera catches up
    public Vector3 offset;  // Offset from the target's position

    private Vector3 velocity = Vector3.zero;  // Reference velocity for SmoothDamp

    void LateUpdate()
    {
        // Desired position of the camera
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        // Update the camera position
        transform.position = smoothedPosition;
    }
}
