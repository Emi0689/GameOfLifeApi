using Microsoft.AspNetCore.Mvc;
using GameOfLifeApi.Helpers;
using GameOfLifeApi.GameMemory;
using GameOfLifeApi.BusinessLogic;

namespace GameOfLifeApi.Controllers;

[ApiController]
[Route("api")]
public class GameOfLifeController : ControllerBase
{
    private readonly IGameStateStorage _gameStateStorage;
    private readonly IGameOfLife _game;

    public GameOfLifeController(IGameStateStorage gameStateStorage, IGameOfLife gameOfLife)
    {
        _gameStateStorage = gameStateStorage;
        _game = gameOfLife;
    }

    /// <summary>
    /// Allows uploading a new board state, returns id of board
    /// </summary>
    /// <param name="state">New Board to create</param>
    /// <returns>The id of new board created</returns>
    [HttpPost("board")]
    public IActionResult UploadBoard([FromBody] List<List<int>> state)
    {
        try
        {
            int[,] board = Helper.ConvertTo2DArray(state);
            _game.ValidateBoard(board);
            var id = _gameStateStorage.SaveState(board);
            return Ok(new { id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Returns the new state of an existing board
    /// </summary>
    /// <param name="id">the id of a board created to get the new state</param>
    /// <returns>The board updated</returns>
    [HttpPut("{id}/next")]
    public IActionResult GetNextState(Guid id)
    {
        try
        {
            var state = _gameStateStorage.GetState(id);
            if (state == null)
                return NotFound(new { error = "Board not found" });

            var nextState = _game.NextState(state);
            _gameStateStorage.UpdateState(id, nextState);  // update the board in memory

            return Ok(new { state = Helper.ConvertToList(nextState) }); // I convert to list because net doesn't work very well with int[,]
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Returns the "x" generations of an existing board
    /// </summary>
    /// <param name="id">the id of a board created to get the new state</param>
    /// <param name="x">The number of generation to be executed</param>
    /// <returns>The board updated with x generations</returns>
    [HttpPut("{id}/next/{x}")]
    public IActionResult GetXGenerations(Guid id, int x)
    {
        try
        {
            var state = _gameStateStorage.GetState(id);
            if (state == null)
                return NotFound(new { error = "Board not found" });

            var futureState = _game.EvolveXGenerations(state, x);
            _gameStateStorage.UpdateState(id, futureState);

            return Ok(new { state = Helper.ConvertToList(futureState) });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Returns the final state of an existing board or return exception
    /// </summary>
    /// <param name="id">the id of a board created to get the new state</param>
    /// <param name="maxAttempts">The number of generation to be executed for the final form</param>
    /// <returns>The final state of the board</returns>
    [HttpPut("{id}/final/{maxAttempts}")]
    public IActionResult GetFinalState(Guid id, int maxAttempts)
    {
        try
        {
            var state = _gameStateStorage.GetState(id);
            if (state == null)
                return NotFound(new { error = "Board not found" });

            var futureState = _game.EvolveXGenerations(state, maxAttempts, true);
            _gameStateStorage.UpdateState(id, futureState);

            return Ok(new { state = Helper.ConvertToList(futureState) });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Delete the board by Id
    /// </summary>
    /// <param name="id">the id of a board created</param>
    /// <returns>OK-200</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id) {
        var state = _gameStateStorage.RemoveState(id);

        if (state)
            return NoContent();
        else
            return BadRequest();
    }
}