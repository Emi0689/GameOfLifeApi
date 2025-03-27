using GameOfLifeApi.GameMemory;
using System.Collections.Concurrent;

public class InMemoryGameStateStorage : IGameStateStorage
{
    //We use memory. If we want to persist the states, we need to use a database. Ej: SQlite
    private readonly ConcurrentDictionary<Guid, int[,]> _boards = new();

    public Guid SaveState(int[,] board)
    {
        var id = Guid.NewGuid();
        _boards[id] = board;
        return id;
    }

    public int[,]? GetState(Guid id)
    {
        _boards.TryGetValue(id, out var board);
        return board;
    }

    public bool UpdateState(Guid id, int[,] board)
    {
        if (!_boards.ContainsKey(id)) return false;
        _boards[id] = board;
        return true;
    }

    public bool RemoveState(Guid id)
    {
        return _boards.Remove(id, out _);
    }
}