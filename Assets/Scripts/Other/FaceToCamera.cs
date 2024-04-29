using UnityEngine;

public enum BillboardType
{
    LookAtCamera,CameraForward 
}
public class FaceToCamera : MonoBehaviour
{
    [SerializeField] private BillboardType billboardType;
    private Camera targetCamera;
    private Vector3 initialRotation;
    [Header("Lock Rotation")]
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;

    private void Awake()
    {
        targetCamera = Camera.main;
        initialRotation = transform.rotation.eulerAngles;
    }
    private void Update()
    {
        switch (billboardType)
        {
            case BillboardType.LookAtCamera:
                transform.LookAt(targetCamera.transform.position);
                break;
            case BillboardType.CameraForward:
                transform.forward = targetCamera.transform.forward;
                break;
        }

        Vector3 rotation = transform.rotation.eulerAngles;

        if (lockX) { rotation.x = initialRotation.x; }
        if (lockY) { rotation.y = initialRotation.y; }
        if (lockZ) { rotation.z = initialRotation.z; }

        transform.rotation = Quaternion.Euler(rotation);
    }
}
