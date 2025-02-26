using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using GameOfLifeApi.BusinessLogic;
using GameOfLifeApi.Helpers;

namespace GameOfLifeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameOfLifeController : ControllerBase
{
    //We use memory. If we want to persist the states, we need to use a database. Ej: SQlite
    private static readonly ConcurrentDictionary<Guid, int[,]> _boards = new(); 
    private readonly GameOfLife _game;

    public GameOfLifeController()
    {
        _game = new GameOfLife();
    }

    /// <summary>
    /// Upload new board
    /// </summary>
    /// <param name="state">New Board to create</param>
    /// <returns>The id of new board created</returns>
    [HttpPost("upload")]
    public IActionResult UploadBoard([FromBody] List<List<int>> state)
    {
        try
        {
            var id = Guid.NewGuid();
            int[,] board = Helper.ConvertTo2DArray(state); // I convert to int[,] for better management than list
            _game.ValidateBoard(board);
            _boards[id] = board;
            return Ok(new { id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get the new state of an existing board
    /// </summary>
    /// <param name="id">the id of a board created to get the new state</param>
    /// <returns>The board updated</returns>
    [HttpGet("{id}/next")]
    public IActionResult GetNextState(Guid id)
    {
        if (!_boards.TryGetValue(id, out var state))
            return NotFound(new { error = "Board not found" });

        var nextState = _game.NextState(state);
        _boards[id] = nextState; // update the board in memory

        return Ok(new { state = Helper.ConvertToList(nextState) }); // I convert to list because net doesn't work very well with int[,]
    }

    /// <summary>
    /// Get the "x" generations of an existing board
    /// </summary>
    /// <param name="id">the id of a board created to get the new state</param>
    /// <param name="x">The number of generation to be executed</param>
    /// <returns>The board updated with x generations</returns>
    [HttpGet("{id}/next/{x}")]
    public IActionResult GetXGenerations(Guid id, int x)
    {
        if (!_boards.TryGetValue(id, out var state))
            return NotFound(new { error = "Board not found" });

        var futureState = _game.EvolveXGenerations(state, x);
        _boards[id] = futureState; // update the board in memory
        return Ok(new { state = Helper.ConvertToList(futureState) });
    }

    /// <summary>
    /// Get the final state of an existing board or return exception
    /// </summary>
    /// <param name="id">the id of a board created to get the new state</param>
    /// <param name="maxAttempts">The number of generation to be executed for the final form</param>
    /// <returns>The final state of the board</returns>
    [HttpGet("{id}/final/{maxAttempts}")]
    public IActionResult GetFinalState(Guid id, int maxAttempts)
    {
        if (!_boards.TryGetValue(id, out var state))
            return NotFound(new { error = "Board not found" });

        var futureState = _game.EvolveXGenerations(state, maxAttempts, true);
        _boards[id] = futureState; // update the board in memory
        return Ok(new { state = Helper.ConvertToList(futureState) });
    }
}