using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    private bool slowDownButtonPress = false;
    private float timeElapsed = 0;
    public int rotationIndex { get; private set; }
    [SerializeField]
    private double buttonTimer = .3;
    public float stepDelay = 1f;
    public float lockDelay = .5f;
    private float stepTime;
    private float lockTime;

    //mouse controls
    public Vector2 firstPressPos;
    public Vector2 secondPressPos;
    public Vector2 currentSwipe;
    public float mouseRoomForError = .5f;
    private float mouseLockTime = 0;


    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    public void PreInitialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = 0f;
        this.lockTime = 0f;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update()
    {
        if (!this.board.isPaused)
        {
            this.board.ClearPiece(this);

            this.lockTime += Time.deltaTime;

            int swipe = -1;
            swipe = Swipe();

            if (this.board.twoPlayerBoard == null)
            {
                //STRAIGHT MOVEMENTS

                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || (swipe == 3))
                {
                    //Move left
                    Move(Vector2Int.left);
                    timeElapsed = 0;
                    //in each key down reset the time elapsed because that means they stopped holding the key from before
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || (swipe == 4))
                {
                    //Move right
                    Move(Vector2Int.right);
                    timeElapsed = 0;
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || (swipe == 2))
                {
                    //Move down
                    Move(Vector2Int.down);
                    timeElapsed = 0;
                }
                else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || (swipe == 1))
                {
                    //slam down
                    SlamDrop();
                    timeElapsed = 0;
                }
                else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftBracket) || (swipe == 5))
                {
                    //rotate left
                    // || Input.GetMouseButtonDown(0)
                    Rotate(-1);
                }
                else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightBracket))
                {
                    //rotate right
                    Rotate(1);
                }
                else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    MoveWithTimer(Vector2Int.left);
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    MoveWithTimer(Vector2Int.right);
                }
                else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    MoveWithTimer(Vector2Int.down);
                }
                

                if (Time.time >= this.stepTime)
                {
                    Step();
                }
            }
            else
            {
                //Split up 1 player and 2 player

                //1 player
                if(board.PlayerNumber == 1)
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        Move(Vector2Int.left);
                        timeElapsed = 0;
                        //in each key down reset the time elapsed because that means they stopped holding the key from before
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        Move(Vector2Int.right);
                        timeElapsed = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        Move(Vector2Int.down);
                        timeElapsed = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        SlamDrop();
                        timeElapsed = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftBracket))
                    {
                        Rotate(-1);
                    }
                    else if (Input.GetKeyDown(KeyCode.RightBracket))
                    {
                        Rotate(1);
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        MoveWithTimer(Vector2Int.left);
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        MoveWithTimer(Vector2Int.right);
                    }
                    else if (Input.GetKey(KeyCode.DownArrow))
                    {
                        MoveWithTimer(Vector2Int.down);
                    }

                    if (Time.time >= this.stepTime)
                    {
                        Step();
                    }
                }
                //2 player
                if (board.PlayerNumber == 2)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        Move(Vector2Int.left);
                        timeElapsed = 0;
                        //in each key down reset the time elapsed because that means they stopped holding the key from before
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        Move(Vector2Int.right);
                        timeElapsed = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        Move(Vector2Int.down);
                        timeElapsed = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.W))
                    {
                        SlamDrop();
                        timeElapsed = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.Q))
                    {
                        Rotate(-1);
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        Rotate(1);
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        MoveWithTimer(Vector2Int.left);
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        MoveWithTimer(Vector2Int.right);
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        MoveWithTimer(Vector2Int.down);
                    }

                    if (Time.time >= this.stepTime)
                    {
                        Step();
                    }
                }
            }

            this.board.SetPiece(this);
        }
    }

    

    public int Swipe()
    {
        //if(!this.board.isPaused && mouseLockTime <= 0f) 
        if (!this.board.isPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            if (Input.GetMouseButtonUp(0))
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                //create vector from the two points
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //if(firstPressPos == secondPressPos)
                if ((secondPressPos.x - mouseRoomForError) < firstPressPos.x && firstPressPos.x < (secondPressPos.x + mouseRoomForError) && (secondPressPos.y - mouseRoomForError) < firstPressPos.y && firstPressPos.y < (secondPressPos.y + mouseRoomForError))
                {
                    //secondPressPos = firstPressPos - (Vector2.one * .1f);
                    return 5;
                }
                else if (currentSwipe.y > 0 && currentSwipe.x > -mouseRoomForError && currentSwipe.x < mouseRoomForError)
                {//swipe upwards
                    return 1;
                }
                else if (currentSwipe.y < 0 && currentSwipe.x > -mouseRoomForError && currentSwipe.x < mouseRoomForError)
                {//swipe down
                    return 2;
                }
                else if (currentSwipe.x < 0 && currentSwipe.y > -mouseRoomForError && currentSwipe.y < mouseRoomForError)
                {//swipe left
                    return 3;
                }
                else if (currentSwipe.x > 0 && currentSwipe.y > -mouseRoomForError && currentSwipe.y < mouseRoomForError)
                {//swipe right
                    return 4;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        //else if(mouseLockTime > 0)
        //{
            //mouseLockTime -= Time.deltaTime;
        //    mouseLockTime -= .1f;
        //    return -1;
        //}
        else
        {
        //    mouseLockTime = 30f;
            return -1;
        }
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;

        Move(Vector2Int.down);

        if(this.lockTime >= this.lockDelay)
        {
            Lock () ;
        }
    }

    private void Lock()
    {
        //add this score for any time a piece gets locked in place
        this.board.AddScore(1);
        this.board.SetPiece(this);
        this.board.ClearLines();
        this.board.SpawnNextPieces();
    }

    //MoveWithTimer has 2 delays. One it uses a bool to only move every other update so the piece doesn't move too quickly.
    //The other delay is a timer delay that won't start moving the piece until the user has been holding the key for a certain amount of time.
    private void MoveWithTimer(Vector2Int translation)
    {
        if (timeElapsed > buttonTimer)
        {
            if (slowDownButtonPress)
            {
                Move(translation);
            }
            else { slowDownButtonPress = !slowDownButtonPress; }
        }
        else { timeElapsed += Time.deltaTime; }
    }


    private void SlamDrop()
    {
        /************************************************************
         * HERE IS WHERE YOU NEED TO CALL CUTE SPARKLES METHOD WITH LINES TO MAKE IT LOOK LIKE THE PIECE DROPPED QUICKLY
         * ***********************************************************/
        while (Move(Vector2Int.down))
        {
            continue;
        }

        //add one point for hard drop
        //and then in the board controller 1 point is always added when the piece is placed
        this.board.AddScore(1);
        Lock();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        if (valid)
        {
            this.position = newPosition;
            this.lockTime = 0;
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.tetromino)
            {
                case Tetromino.BubbleI:
                case Tetromino.StoneI:
                case Tetromino.BubbleO:
                case Tetromino.StoneO:
                    cell.x -= .5f;
                    cell.y -= .5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                /*case Tetromino.StoneLeftStairs:
                case Tetromino.BubbleLeftStairs:
                    cell.x += .5f;
                    cell.y += .5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;*/
                /*case Tetromino.BubbleRightStairs:
                case Tetromino.StoneRightStairs:
                    cell.x -= .5f;
                    cell.y += .5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;*/
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationDirection * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }
    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
