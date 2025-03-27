using GameOfLifeApi.GameMemory;
using GameOfLifeApi.Helpers;
using System.Collections.Concurrent;
using System.Text.Json;

public class GameStateStorage : IGameStateStorage
{
    //We use memory and SQLite for the persistence
    private readonly ConcurrentDictionary<Guid, int[,]> _boards = new();

    private int[,] LoadFromDatabase(Guid id)
    {
        using var db = new GameDbContext();
        var state = db.BoardStates.FirstOrDefault(x => x.Id == id);
        if (state != null)
        {
            var boardList = JsonSerializer.Deserialize<List<List<int>>>(state.BoardJson);
            int[,] board = Helper.ConvertTo2DArray(boardList);
            _boards[id] = board;
            return board;
        }
        return null;
    }

    private void PersistToDatabase(int[,] board)
    {
        using var db = new GameDbContext();
        db.BoardStates.Add(new BoardState { BoardJson = JsonSerializer.Serialize(Helper.ConvertToList(board)) });
        db.SaveChanges();
    }

    public Guid SaveState(int[,] board)
    {
        var id = Guid.NewGuid();
        _boards[id] = board;
        PersistToDatabase(board);
        return id;
    }

    public int[,]? GetState(Guid id)
    {
        if(_boards.TryGetValue(id, out var board))
            return board;

        return LoadFromDatabase(id);
    }

    public bool UpdateState(Guid id, int[,] board)
    {
        if (!_boards.ContainsKey(id)) return false;
        UpdateDatabase(id, board);
        _boards[id] = board;
        return true;
    }

    private void UpdateDatabase(Guid id, int[,] board)
    {
        using var db = new GameDbContext();
        var state = db.BoardStates.FirstOrDefault(x => x.Id == id);
        if (state != null)
        {
            state.BoardJson = JsonSerializer.Serialize(Helper.ConvertToList(board));
            db.SaveChanges();
        }
    }

    public bool RemoveState(Guid id)
    {
        using var db = new GameDbContext();
        var states = db.BoardStates.ToList();
        db.BoardStates.RemoveRange(states);
        db.SaveChanges();
        return _boards.Remove(id, out _);
    }
}