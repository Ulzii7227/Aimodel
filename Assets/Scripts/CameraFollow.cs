using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // The Robber to follow
    public Vector3 offset = new Vector3(0, 5, -7); // Adjust for better framing
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target); // Always face the Robber
    }
}
