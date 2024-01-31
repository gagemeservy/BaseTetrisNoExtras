using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
    public static readonly float sin = Mathf.Sin(Mathf.PI / 2f);
    public static readonly float[] RotationMatrix = new float[] { cos, sin, -sin, cos };

    public static readonly Dictionary<Tetromino, Vector2Int[]> Cells = new Dictionary<Tetromino, Vector2Int[]>()
    {
        { Tetromino.BubbleI, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 2, 1) } },
        { Tetromino.BubbleJ, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.BubbleL, new Vector2Int[] { new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.BubbleO, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.BubbleS, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0) } },
        { Tetromino.BubbleT, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.BubbleZ, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        //{ Tetromino.BubbleCross, new Vector2Int[] { new Vector2Int( -1,0), new Vector2Int( 0,0), new Vector2Int(1, 0), new Vector2Int( 0, 1), new Vector2Int(0, -1) } },
        //{ Tetromino.BubbleLeftStairs, new Vector2Int[] { new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(1, 0), new Vector2Int( 0, 1), new Vector2Int(1, 1), new Vector2Int(1, 2) } },
        //{ Tetromino.BubbleRightStairs, new Vector2Int[] { new Vector2Int( -1, 0), new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( -1, 1), new Vector2Int(0, 1), new Vector2Int(-1, 2) } },
        //{ Tetromino.BubblePyramid, new Vector2Int[] { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2, 0), new Vector2Int(0,1), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(-1,0), new Vector2Int(-2, 0), new Vector2Int(-1, 1) } },
        { Tetromino.StoneI, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 2, 1) } },
        { Tetromino.StoneJ, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.StoneL, new Vector2Int[] { new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.StoneO, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.StoneS, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0) } },
        { Tetromino.StoneT, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.StoneZ, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        //{ Tetromino.StoneCross, new Vector2Int[] { new Vector2Int( -1,0), new Vector2Int( 0,0), new Vector2Int(1, 0), new Vector2Int( 0, 1), new Vector2Int(0, -1) } },
        //{ Tetromino.StoneLeftStairs, new Vector2Int[] { new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(1, 0), new Vector2Int( 0, 1), new Vector2Int(1, 1), new Vector2Int(1, 2) } },
        //{ Tetromino.StoneRightStairs, new Vector2Int[] { new Vector2Int( -1, 0), new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( -1, 1), new Vector2Int(0, 1), new Vector2Int(-1, 2) } },
        //{ Tetromino.StonePyramid, new Vector2Int[] { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2, 0), new Vector2Int(0,1), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(-1,0), new Vector2Int(-2, 0), new Vector2Int(-1, 1) } },
    };

    private static readonly Vector2Int[,] WallKicksI = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
    };

    private static readonly Vector2Int[,] WallKicksJLOSTZ = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
    };

    public static readonly Dictionary<Tetromino, Vector2Int[,]> WallKicks = new Dictionary<Tetromino, Vector2Int[,]>()
    {/***************
      * NEED TO ADJUST WALL KICKS FOR CROSS STAIRS AND PYRAMIDS
      * ******************************************************/
        { Tetromino.BubbleI, WallKicksI },
        { Tetromino.BubbleJ, WallKicksJLOSTZ },
        { Tetromino.BubbleL, WallKicksJLOSTZ },
        { Tetromino.BubbleO, WallKicksJLOSTZ },
        { Tetromino.BubbleS, WallKicksJLOSTZ },
        { Tetromino.BubbleT, WallKicksJLOSTZ },
        { Tetromino.BubbleZ, WallKicksJLOSTZ },
        //{ Tetromino.BubbleCross, WallKicksJLOSTZ },
        //{ Tetromino.BubbleLeftStairs, WallKicksJLOSTZ },
        //{ Tetromino.BubbleRightStairs, WallKicksJLOSTZ },
        //{ Tetromino.BubblePyramid, WallKicksJLOSTZ },
        { Tetromino.StoneI, WallKicksI },
        { Tetromino.StoneJ, WallKicksJLOSTZ },
        { Tetromino.StoneL, WallKicksJLOSTZ },
        { Tetromino.StoneO, WallKicksJLOSTZ },
        { Tetromino.StoneS, WallKicksJLOSTZ },
        { Tetromino.StoneT, WallKicksJLOSTZ },
        { Tetromino.StoneZ, WallKicksJLOSTZ },
        //{ Tetromino.StoneCross, WallKicksJLOSTZ },
        //{ Tetromino.StoneLeftStairs, WallKicksJLOSTZ },
        //{ Tetromino.StoneRightStairs, WallKicksJLOSTZ },
        //{ Tetromino.StonePyramid, WallKicksJLOSTZ },
    };

}