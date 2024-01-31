using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public TetrominoData[] tetrominoes;
    public Piece activePiece { get; private set; }
    public Piece nextPiece { get; private set; }
    public Vector3Int spawnPosition;
    /******************************
     * THIS MAY CAUSE PROBLEMS. INITIALLY THE HEIGHT IS 20, BUT I ADDED 2 BECAUSE SOME OF THE PIECES SPAWN DIRECTLY
     * ABOVE THE BOUNDS AND I DON'T WANT THEM TO GET KICKED OUT SO I ADDED 2. 
     * Also the plus 1 is to set the corner at the right spot still
     * ********************************************************/
    //public Vector2Int boardSize = new Vector2Int(10, 22);
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public int difficultyLevel = 1;
    public int score = 0;
    public int comboCount = -1;
    public bool ongoingCombo = false;
    public float stepReductionMultiplier = .05f;

    public RectInt Bounds
    {
        get
        {
            //Vector2Int position = new Vector2Int(-this.boardSize.x/2, -this.boardSize.y/2 + 1);
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }
    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        
        for (int i = 0; i < tetrominoes.Length; i++) {
            this.tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, this.spawnPosition, data);

        if (IsValidPosition(this.activePiece, this.spawnPosition))
        {
            SetPiece(this.activePiece);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        this.tilemap.ClearAllTiles();

        /********************************
         * NEED TO ADD MORE GAME OVER LOGIC
         * ******************************/
    }

    public void SetPiece(Piece piece)
    {
        for (int i = 0 ;i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }

        this.score += 1;
    }

    public void ClearPiece(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            /*******************************************
             * HERE WE WILL ALSO NEED TO CHECK IF THE CURRENT PIECE IS A STONE,
             * THEN CHECK IF THE PIECE IN THE SPACE IS A BUBBLE BECAUSE IF SO THEN WE CAN OVER WRITE AND DELETE PART OF IT
             * ******************************************/
            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }

        }

        return true;
    }

    public void ClearLines()
    {
        //DIFFICULTY LEVEL IS KEPT TRACK OF IN THIS FUNCTION EVERY TIME A LEVEL IS CLEARED

        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        int newDifficultyLevel = difficultyLevel;

        while (row < bounds.yMax) 
        {
            if (IsLineFull(row))
            {
                LineClear(row);
                newDifficultyLevel++;
            }
            else
            {
                row++;
            }
        }


        int linesCleared = newDifficultyLevel - difficultyLevel;
        if (linesCleared > 0)
        {
            //line clear score is multiplied by level before the clear
            CalculateScore(linesCleared);
            DecreaseStepDelay(linesCleared);
            difficultyLevel = newDifficultyLevel;
        }
    }

    private void CalculateScore(int linesCleared)
    {
        //calculate line score
        LineScore(linesCleared);

        //now calculate combo score
        if (comboCount > 0)
        {
            score += 50 * comboCount * difficultyLevel;
        }

        
    }

    private void LineScore(int linesCleared)
    {
        if (linesCleared == 1)
        {
            score += 100 * difficultyLevel;
            comboCount++;
        }
        else if (linesCleared == 2)
        {
            score += 300 * difficultyLevel;
            comboCount++;
        }
        else if (linesCleared == 3)
        {
            score += 500 * difficultyLevel;
            comboCount++;
        }
        else if (linesCleared == 4)
        {
            score += 800 * difficultyLevel;
            comboCount++;
        }
        else if (linesCleared > 4)
        {
            score += (800 + (100 * linesCleared)) * difficultyLevel;
            comboCount++;
        }
        else
        {
            //reset combo
            comboCount = -1;
        }
    }

    private void DecreaseStepDelay(int linesCleared)
    {
        if(linesCleared > 0 && this.activePiece.stepDelay >= .06f)
        {
            float stepReduction = linesCleared * stepReductionMultiplier;
            this.activePiece.stepDelay -= stepReduction;
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;
        
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for(int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }

            row++;
        }
    }


}