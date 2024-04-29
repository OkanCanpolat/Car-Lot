using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TileBlock : MonoBehaviour
{
    [SerializeField] private Renderer meshRenderer;
    private List<Tile> blockedTiles;
    private GridSystem gridSystem;

    public List<Tile> BlockedTiles => blockedTiles;

    [Inject]
    public void Construct(GridSystem gridSystem)
    {
        this.gridSystem = gridSystem;
    }

    private void Start()
    {
        blockedTiles = gridSystem.GetTile(meshRenderer.bounds);
        BlockTiles();
    }
    private void BlockTiles()
    {
        if (blockedTiles == null) return;
        foreach (Tile tile in blockedTiles)
        {
            tile.IsBlocked = true;
            tile.GetComponent<Collider>().enabled = false;
        }
    }
}
