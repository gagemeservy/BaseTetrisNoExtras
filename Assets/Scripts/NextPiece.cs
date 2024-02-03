using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class NextPiece : MonoBehaviour
{
    public Board board;
    public Vector3Int nextPieceSpawnPosition;
    public NextPiece nextPiece { get; private set; }
    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public TetrominoData data { get; private set; }

    public void SpawnNextPiece(TetrominoData data)
    {
        this.data = data;

        Clear();

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }

        SetPiece();
    }

    public void Initialize(Board board, Vector3Int position, TetrominoData data, Tilemap tilemap)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.tilemap = tilemap;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }

        SetPiece();
    }

    private void Clear()
    {
            for (int i = 0; i < this.cells.Length; i++)
            {
                Vector3Int tilePosition = this.cells[i] + this.position;
                this.tilemap.SetTile(tilePosition, null);
            }
    }
    private void SetPiece()
    {
            for (int i = 0; i < this.cells.Length; i++)
            {
                Vector3Int tilePosition = this.cells[i] + this.position;
                this.tilemap.SetTile(tilePosition, this.data.tile);
            }
    }

}
