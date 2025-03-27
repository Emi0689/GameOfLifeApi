namespace GameOfLifeApi.BusinessLogic
{
    public interface IGameOfLife
    {
        int[,] NextState(int[,] board);
        int[,] EvolveXGenerations(int[,] board, int generations, bool finalStep = false);
        bool AreBoardsEqual(int[,] board1, int[,] board2);
        void ValidateBoard(int[,] board);
    }
}
