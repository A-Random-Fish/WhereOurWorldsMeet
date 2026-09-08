using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Tracking and Offset")]
    [SerializeField] Transform cameraTarget;
    [SerializeField] Vector3 cameraOffset;
    [Space(0.2f)]
    [Header("Camera Controls")]
    [SerializeField] float cameraDamping;

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTarget.position + cameraOffset, cameraDamping);
    }
    
}
