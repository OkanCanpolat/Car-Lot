using UnityEngine;
using Zenject;

public class CameraAutoTransformController : MonoBehaviour
{
    [SerializeField] private float tileSize;
    [SerializeField, HideInInspector] private int width;
    [SerializeField, HideInInspector] private int height;
    [SerializeField, HideInInspector] private Tile midTile;
    [SerializeField, HideInInspector] private Tile lastXTile;

    private GridSystem gridSystem;

    [Header("Perspective Camera Settings")]
    private const float yOffset_P = 5f;
    private const float zOffset_P = -6f;

    [Header("Orthographic Camera Settings")]
    private const float zOffset_O = 2;
    private const float yOffset_O = 10;

    [Inject]
    public void Construct(GridSystem gridSystem)
    {
        this.gridSystem = gridSystem;
    }
    private void Start()
    {
        width = gridSystem.Width;
        height = gridSystem.Height;
        midTile = gridSystem.GetTile(width / 2, height / 2);
        lastXTile = gridSystem.GetTile(width - 1, 0);
    }

    public void RepositionCamera()
    {
        if (height > 5)
        {
            RepositionOrthographic();
        }
        else
        {
            RepositionPerspective();
        }
    }
    private void RepositionPerspective()
    {
        Camera.main.orthographic = false;
        Vector3 lookAtPos = midTile.transform.position;

        float heightExcess = height > width ? height - width : 0;
        float cameraX = lastXTile.transform.position.x - tileSize * 0.5f;
        float cameraY = width * (2 * tileSize) + yOffset_P + heightExcess;
        float cameraZ = lastXTile.transform.position.z + zOffset_P;
        transform.position = new Vector3(cameraX, cameraY, cameraZ);

        if (width % 2 == 0)
        {
            float newX = lookAtPos.x - tileSize * 0.5f;
            lookAtPos.x = newX;
        }
        if (height % 2 == 0)
        {
            float newZ = lookAtPos.z - tileSize * 0.5f;
            lookAtPos.z = newZ;
        }
        transform.LookAt(lookAtPos);
    }
    private void RepositionOrthographic()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 15;
        Vector3 cameraRotation = new Vector3(75, 0, 0);
        Vector3 midPos = midTile.transform.position;

        if (width % 2 == 0)
        {
            float newX = midPos.x - tileSize * 0.5f;
            midPos.x = newX;
        }
        if (height % 2 == 0)
        {
            float newZ = midPos.z - tileSize * 0.5f;
            midPos.z = newZ;
        }

        float cameraX = midPos.x;
        float cameraY = midPos.y + yOffset_O;
        float cameraZ = midPos.z + zOffset_O;

        transform.position = new Vector3(cameraX, cameraY, cameraZ);
        transform.rotation = Quaternion.Euler(cameraRotation);
    }
}
