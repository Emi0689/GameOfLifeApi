namespace GameOfLifeApi.GameMemory
{
    public interface IGameStateStorage
    {
        Guid SaveState(int[,] board);
        int[,]? GetState(Guid id);
        bool UpdateState(Guid id, int[,] board);
        bool RemoveState(Guid id);
    }
}
