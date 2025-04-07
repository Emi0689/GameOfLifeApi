namespace GameOfLifeApi.BusinessLogic;

public class GameOfLife: IGameOfLife
{
    public int[,] NextState(int[,] board)
    {
        ValidateBoard(board);

        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        int[,] nextBoard = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int aliveNeighbors = CountAliveNeighbors(board, i, j);
                if (board[i, j] == 1)
                {
                    nextBoard[i, j] = (aliveNeighbors == 2 || aliveNeighbors == 3) ? 1 : 0;
                }
                else
                {
                    nextBoard[i, j] = (aliveNeighbors == 3) ? 1 : 0;
                }
            }
        }
        return nextBoard;
    }

    private int CountAliveNeighbors(int[,] board, int x, int y)
    {
        int rows = board.GetLength(0); //Get the number of the row
        int cols = board.GetLength(1); //Get the number of the col
        int count = 0;
        int[] directions = { -1, 0, 1 };

        foreach (int directionX in directions)
        {
            foreach (int directionY in directions)
            {
                if (directionX == 0 && directionY == 0) //same place
                {  
                    continue; 
                }

                int nx = x + directionX;
                int ny = y + directionY;

                if (nx >= 0 && nx < rows //inside of the board - Row
                    && ny >= 0 && ny < cols //inside of the board - Cal
                    && board[nx, ny] == 1) //this cell is alive
                {
                    count++;
                }
            }
        }
        return count;
    }

    public int[,] EvolveXGenerations(int[,] board, int generations, bool finalStep = false)
    {
        ValidateBoard(board);

        int[,] previousState = board;
        int[,] currentBoard = board;

        for (int i = 0; i < generations; i++)
        {
            previousState = (int[,])currentBoard.Clone(); 
            currentBoard = NextState(currentBoard);
        }

        if (finalStep && !AreBoardsEqual(previousState, currentBoard))
        {
            throw new("The board did not reach a stable state within the given attempts.");
        }

        return currentBoard;
    }

    public bool AreBoardsEqual(int[,] board1, int[,] board2)
    {
        int rows = board1.GetLength(0);
        int cols = board1.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (board1[i, j] != board2[i, j])
                    return false;
            }
        }
        return true;
    }

    public void ValidateBoard(int[,] board)
    {
        if (board == null || board.Length == 0)
            throw new ArgumentException("The board cannot be empty.");

        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        if (rows > 100 || cols > 100)
            throw new ArgumentException("The board can not be larger than 100x100.");
    }
}