using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    BubbleI,
    BubbleO,
    BubbleT,
    BubbleJ,
    BubbleL,
    BubbleS,
    BubbleZ,
    StoneI,
    StoneO,
    StoneT,
    StoneJ,
    StoneL,
    StoneS,
    StoneZ,
}

/*public enum TetrominoWithExtras
{
    BubbleI,
    BubbleO,
    BubbleT,
    BubbleJ,
    BubbleL,
    BubbleS,
    BubbleZ,
    BubbleCross,
    BubbleLeftStairs,
    BubbleRightStairs,
    BubblePyramid,
    StoneI,
    StoneO,
    StoneT,
    StoneJ,
    StoneL,
    StoneS,
    StoneZ,
    StoneCross,
    StoneLeftStairs,
    StoneRightStairs,
    StonePyramid
}*/

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }
    public Vector2Int[,] wallKicks { get; private set; }

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetromino];
        this.wallKicks = Data.WallKicks[this.tetromino];
    }
}