using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Tracking and Offset")]
    [SerializeField] Transform cameraTarget;
    [SerializeField] Vector3 cameraOffset = new Vector3(0,30,-20);
    [SerializeField] Vector3 cameraRotation = new Vector3(55,0,0);
    [Space(0.2f)]
    [Header("Camera Controls")]
    [SerializeField] float cameraDamping = 0.1f;

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTarget.position + cameraOffset, cameraDamping);
        transform.rotation = Quaternion.Euler(cameraRotation);
    }
    
}
