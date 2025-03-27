using GameOfLifeApi.BusinessLogic;
using GameOfLifeApi.Controllers;
using GameOfLifeApi.GameMemory;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameOfLifeApiTest
{
    public class GameOfLifeControllerTests
    {
        private readonly GameOfLifeController _controller;
        private Mock<IGameStateStorage> _IGameStateStorage = new Mock<IGameStateStorage>();
        private Mock<IGameOfLife> _IGame = new Mock<IGameOfLife>();

        public GameOfLifeControllerTests()
        {
            _controller = new GameOfLifeController(_IGameStateStorage.Object, _IGame.Object);
        }


        [Fact]
        public void UploadBoard_ReturnsId()
        {
            // Arrange: Tablero de ejemplo
            var board = new List<List<int>>
            {
                new List<int> { 0, 1, 0 },
                new List<int> { 1, 1, 1 },
                new List<int> { 0, 1, 0 }
            };

            // Act
            var result = _controller.UploadBoard(board) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.IsType<Guid>(result.Value.GetType().GetProperty("id")?.GetValue(result.Value, null));
        }

        [Fact]
        public void GetNextState_ValidBoard_ReturnsNextState()
        {
            // Arrange
            var board = new List<List<int>>
            {
                new List<int> { 0, 1, 0 },
                new List<int> { 1, 1, 1 },
                new List<int> { 0, 1, 0 }
            };

            var uploadResult = _controller.UploadBoard(board) as OkObjectResult;
            var boardId = (Guid)uploadResult!.Value!.GetType().GetProperty("id")!.GetValue(uploadResult.Value, null)!;

            // Act
            var result = _controller.GetNextState(boardId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public void GetNextState_InvalidId_ReturnsNotFound()
        {
            // Arrange
            Guid invalidId = Guid.NewGuid();
            int[,] initialBoard = null;
            _IGameStateStorage.Setup(x => x.GetState(invalidId)).Returns(initialBoard);

            // Act
            var result = _controller.GetNextState(invalidId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void GetXGenerations_BoardNotFound_ReturnsNotFound()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            int[,] initialBoard = null;
            _IGameStateStorage.Setup(x => x.GetState(nonExistentId)).Returns(initialBoard);

            // Act
            var result = _controller.GetXGenerations(nonExistentId, 3);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}