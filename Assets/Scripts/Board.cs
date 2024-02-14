using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public TetrominoData[] tetrominoes;
    public Piece activePiece { get; private set; }
    public NextPiece nextPiece { get; private set; }
    public Vector3Int spawnPosition;
    public Board twoPlayerBoard;
    public int finalScore = -1;
    /******************************
     * THIS MAY CAUSE PROBLEMS. INITIALLY THE HEIGHT IS 20, BUT I ADDED 2 BECAUSE SOME OF THE PIECES SPAWN DIRECTLY
     * ABOVE THE BOUNDS AND I DON'T WANT THEM TO GET KICKED OUT SO I ADDED 2. 
     * Also the plus 1 is to set the corner at the right spot still
     * ********************************************************/
    //public Vector2Int boardSize = new Vector2Int(10, 22);
    public Vector2Int boardSize = new Vector2Int(10, 20);

    //ALL STARTING VALUES FOR THE GAME TO WORK
    public int difficultyLevel = 1;
    public int score = 0;
    public int comboCount = -1;
    public bool ongoingCombo = false;
    public float stepReductionMultiplier = .05f;
    public int tenLinesCleared = 0;
    public TMP_Text textLevel;
    public TMP_Text textScore;
    private int nextPieceNum;

    public bool isPaused = false;
    public GameObject gameOverScreen;
    public int PlayerNumber = 0;
    public TMP_Text gameOverText;
    public GameObject pauseScreen;
    private Vector3Int oldNextPieceSpawnPosition;
    private int oldYSpawnPosition;

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
        this.gameOverScreen.SetActive(false);
        this.pauseScreen.SetActive(false);
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        this.nextPiece = GetComponentInChildren<NextPiece>();
        this.oldYSpawnPosition = this.spawnPosition.y;
        this.oldNextPieceSpawnPosition = this.nextPiece.position;

        unPause(this, this.pauseScreen);
        unPause(this, this.gameOverScreen);

        for (int i = 0; i < tetrominoes.Length; i++) {
            this.tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnRandomPieces();
    }

    private void Update()
    {
        //player number check is to make sure that only player one presses it in either one or two player that way the menu doesn't trigger twice since there are two boards in two player
        if (Input.GetKeyDown(KeyCode.Escape) && (this.PlayerNumber == 0 || this.PlayerNumber == 1))
        {
            PauseButton();
        }
    }

    public void PauseButton()
    {
        if (!isPaused)
        {
            Pause(this, this.pauseScreen);
        }
        else
        {
            unPause(this, this.pauseScreen);
        }
    }

    public void AddScore(int numberToAdd) 
    {
        if(finalScore == -1)
        {
            this.score += numberToAdd;

            //update the UI
            this.textScore.SetText(score.ToString());
        }
    }

    public void SpawnRandomPieces()
    {
        int random = UnityEngine.Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];
        this.activePiece.Initialize(this, this.spawnPosition, data);

        this.nextPieceNum = UnityEngine.Random.Range(0, this.tetrominoes.Length);
        data = this.tetrominoes[nextPieceNum];
        this.nextPiece.Initialize(this, this.nextPiece.nextPieceSpawnPosition, data, this.tilemap);


        if (IsValidPosition(this.activePiece, this.spawnPosition))
        {
            SetPiece(this.activePiece);
        }
        else
        {
            GameOver();
        }
    }

    public void SpawnNextPieces()
    {
        //use next piece number to set active piece
        TetrominoData data = this.tetrominoes[nextPieceNum];
        this.activePiece.Initialize(this, this.spawnPosition, data);

        //Set next piece
        this.nextPieceNum = UnityEngine.Random.Range(0, this.tetrominoes.Length);
        data = this.tetrominoes[nextPieceNum];
        this.nextPiece.SpawnNextPiece(data);


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
        if (this.twoPlayerBoard == null)
        {
            Pause(this, this.gameOverScreen);
        }
        else
        {
            this.finalScore = score;
            
            this.spawnPosition.y = -1000;
            this.nextPiece.position = new Vector3Int(-1000, -1000, 0);

            if ((this.twoPlayerBoard.finalScore > -1) && !this.isPaused && !this.twoPlayerBoard.isPaused)
            {
                int winningPlayerNumber = -1;
                int winningScore = -1;

                if (this.twoPlayerBoard.finalScore > this.finalScore)
                {
                    winningPlayerNumber = this.twoPlayerBoard.PlayerNumber;
                    winningScore = this.twoPlayerBoard.finalScore;
                }
                else
                {
                    winningPlayerNumber = this.PlayerNumber;
                    winningScore = this.finalScore;
                }

                this.gameOverText.SetText("Player " + winningPlayerNumber + " won with a score of " + winningScore + "!");
                Pause(this, this.gameOverScreen);
            }
        }
    }

    public static void Pause(Board board, GameObject screenToShow)
    {
        Time.timeScale = 0;
        board.isPaused = true;
        screenToShow.SetActive(true);
    }

    public static void unPause(Board board, GameObject screenToHide)
    {
        Time.timeScale = 1;
        board.isPaused = false;
        screenToHide.SetActive(false);
    }

    public void Resume()
    {
        unPause(this, this.pauseScreen);
    }

    public void Restart()
    {
        this.tilemap.ClearAllTiles();
        this.activePiece.currentSwipe = Vector2.zero;
        this.activePiece.firstPressPos = Vector2.zero;
        this.activePiece.secondPressPos = Vector2.zero;

        //reset2playermodestuff
        if (this.twoPlayerBoard != null)
        {
            this.gameOverText.SetText("GAME OVER");
            this.finalScore = -1;
            this.spawnPosition.y = oldYSpawnPosition;
            this.nextPiece.position = oldNextPieceSpawnPosition;
            this.twoPlayerBoard.Restart2Player();
        }

        //Reset all starting values
        this.difficultyLevel = 1;
        this.score = 0;
        this.comboCount = -1;
        this.ongoingCombo = false;
        this.stepReductionMultiplier = .05f;
        this.tenLinesCleared = 0;
        this.textLevel.SetText(1.ToString());
        this.textScore.SetText(0.ToString());
        SpawnRandomPieces();
        unPause(this, this.gameOverScreen);
    }

    public void Restart2Player()
    {
        this.tilemap.ClearAllTiles();
        this.activePiece.currentSwipe = Vector2.zero;
        this.activePiece.firstPressPos = Vector2.zero;
        this.activePiece.secondPressPos = Vector2.zero;

        this.gameOverText.SetText("GAME OVER");
        this.finalScore = -1;
        this.spawnPosition.y = oldYSpawnPosition;
        this.nextPiece.position = oldNextPieceSpawnPosition;

        //Reset all starting values
        this.difficultyLevel = 1;
        this.score = 0;
        this.comboCount = -1;
        this.ongoingCombo = false;
        this.stepReductionMultiplier = .05f;
        this.tenLinesCleared = 0;
        this.textLevel.SetText(1.ToString());
        this.textScore.SetText(0.ToString());
        SpawnRandomPieces();
        //unPause(this, this.gameOverScreen);
    }

    public void RestartFromPauseMenu()
    {
        this.tilemap.ClearAllTiles();
        this.activePiece.currentSwipe = Vector2.zero;
        this.activePiece.firstPressPos = Vector2.zero;
        this.activePiece.secondPressPos = Vector2.zero;

        //reset2playermodestuff
        if (this.twoPlayerBoard != null)
        {
            this.gameOverText.SetText("GAME OVER");
            this.finalScore = -1;
            this.spawnPosition.y = oldYSpawnPosition;
            this.nextPiece.position = oldNextPieceSpawnPosition;
            this.twoPlayerBoard.Restart2Player();
        }

        //Reset all starting values
        this.difficultyLevel = 1;
        this.score = 0;
        this.comboCount = -1;
        this.ongoingCombo = false;
        this.stepReductionMultiplier = .05f;
        this.tenLinesCleared = 0;
        this.textLevel.SetText(1.ToString());
        this.textScore.SetText(0.ToString());
        SpawnRandomPieces();
        unPause(this, this.pauseScreen);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        StartCoroutine(LoadAsyncScene("MainMenu"));
    }

    IEnumerator LoadAsyncScene(String sceneToSwitchTo)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToSwitchTo);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void SetPiece(Piece piece)
    {
        for (int i = 0 ;i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void SetNextPiece(NextPiece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
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
        int newTenLinesCleared = tenLinesCleared;

        while (row < bounds.yMax) 
        {
            if (IsLineFull(row))
            {
                LineClear(row);
                newTenLinesCleared++;
            }
            else
            {
                row++;
            }
        }

        
        int linesCleared = newTenLinesCleared - tenLinesCleared;
        tenLinesCleared = newTenLinesCleared;
        if (linesCleared > 0)
        {
            //line clear score is multiplied by level before the clear
            CalculateScore(linesCleared);
            while(tenLinesCleared >= 10)
            {
                DecreaseStepDelay(1);
                difficultyLevel++;
                tenLinesCleared -= 10;
            }

            this.textLevel.SetText(difficultyLevel.ToString());
        }
    }

    private void CalculateScore(int linesCleared)
    {
        //calculate line score
        LineScore(linesCleared);

        //now calculate combo score
        if (comboCount > 0)
        {
            AddScore(50 * comboCount * difficultyLevel);
        }
    }

    private void LineScore(int linesCleared)
    {
        if (linesCleared == 1)
        {
            AddScore(100 * difficultyLevel);
            comboCount++;
        }
        else if (linesCleared == 2)
        {
            AddScore(300 * difficultyLevel);
            comboCount++;
        }
        else if (linesCleared == 3)
        {
            AddScore(500 * difficultyLevel);
            comboCount++;
        }
        else if (linesCleared == 4)
        {
            AddScore(800 * difficultyLevel);
            comboCount++;
        }
        else if (linesCleared > 4)
        {
            AddScore((800 + (100 * linesCleared)) * difficultyLevel);
            comboCount++;
        }
        else
        {
            //reset combo
            comboCount = -1;
        }
    }

    private void DecreaseStepDelay(int timesToDecrease)
    {
        if(timesToDecrease > 0 && this.activePiece.stepDelay >= .06f)
        {
            float stepReduction = timesToDecrease * stepReductionMultiplier;
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