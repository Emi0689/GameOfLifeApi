using GameOfLifeApi.BusinessLogic;

namespace GameOfLifeApiTest
{
    public class GameOfLifeBITests
    {
        private readonly GameOfLife _game;

        public GameOfLifeBITests()
        {
            _game = new GameOfLife();
        }

        [Fact]
        public void ValidateBoard_EmptyBoard_ThrowsException()
        {
            int[,] emptyBoard = new int[0, 0];
            var exception = Assert.Throws<ArgumentException>(() => _game.ValidateBoard(emptyBoard));
            Assert.Equal("The board cannot be empty.", exception.Message);
        }

        [Fact]
        public void ValidateBoard_LargeBoard_ThrowsException()
        {
            int[,] largeBoard = new int[101, 101];
            var exception = Assert.Throws<ArgumentException>(() => _game.ValidateBoard(largeBoard));
            Assert.Equal("The board can not be larger than 100x100.", exception.Message);
        }

        [Fact]
        public void NextState_BlockRemainsStable()
        {
            // Arrange
            int[,] initialBoard = 
            {
                { 1, 1 },
                { 1, 1 }
            };

            int[,] expectedBoard = 
            {
                { 1, 1 },
                { 1, 1 }
            };

            // Act
            int[,] result = _game.NextState(initialBoard);

            // Assert
            Assert.Equal(expectedBoard, result);
        }

        [Fact]
        public void NextState_BlinkerOscillates()
        {
            // Arrange
            int[,] initialBoard = 
            {
                { 0, 1, 0 },
                { 0, 1, 0 },
                { 0, 1, 0 }
            };

            int[,] expectedBoard = 
            {
                { 0, 0, 0 },
                { 1, 1, 1 },
                { 0, 0, 0 }
             };

            // Act
            int[,] result = _game.NextState(initialBoard);

            // Assert
            Assert.Equal(expectedBoard, result);
        }

        [Fact]
        public void NextState_SingleCellDies()
        {
            // Arrange
            int[,] initialBoard = 
                {
            { 0, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 0 }
            };

            int[,] expectedBoard = 
                {
            { 0, 0, 0 },
            { 0, 0, 0 },
            { 0, 0, 0 }
            };

            // Act
            int[,] result = _game.NextState(initialBoard);

            // Assert
            Assert.Equal(expectedBoard, result);
        }

        [Fact]
        public void NextState_DeadCellRevivesWithThreeNeighbors()
        {
            // Arrange
            int[,] initialBoard = 
            {
                { 0, 1, 0 },
                { 0, 0, 1 },
                { 1, 0, 0 }
            };

            int[,] expectedBoard = 
            {
                { 0, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 0 }
            };

            // Act
            int[,] result = _game.NextState(initialBoard);

            // Assert
            Assert.Equal(expectedBoard, result);
        }

        [Fact]
        public void NextState_OvercrowdingCausesDeath()
        {
            // Arrange
            int[,] initialBoard = 
            {
                { 1, 1, 1 },
                { 1, 1, 1 },
                { 1, 1, 1 }
            };

            int[,] expectedBoard = 
            {
                { 1, 0, 1 },
                { 0, 0, 0 },
                { 1, 0, 1 }
            };

            // Act
            int[,] result = _game.NextState(initialBoard);

            // Assert
            Assert.Equal(expectedBoard, result);
        }
    


        [Fact]
        public void EvolveXGenerations_ValidBoard_ReturnsCorrectState()
        {
            // Arrange
            int[,] initialBoard = 
            {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 }
            };

            int[,] expectedBoard = 
            {
                { 1, 0, 1 },
                { 0, 0, 0 },
                { 1, 0, 1 }
             };

            // Act
            int[,] result = _game.EvolveXGenerations(initialBoard, 2);

            // Assert
            Assert.Equal(expectedBoard, result);
        }

        [Fact]
        public void EvolveXGenerations_StableBoard_ReturnsSameState()
        {
            // Arrange
            int[,] stableBoard = 
            {
                { 1, 1 },
                { 1, 1 }
            };

            // Act
            int[,] result = _game.EvolveXGenerations(stableBoard, 5);

            // Assert
            Assert.Equal(stableBoard, result);
        }

        [Fact]
        public void EvolveXGenerations_ZeroGenerations_ReturnsSameBoard()
        {
            // Arrange
            int[,] board = 
            {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 }
            };

            // Act
            int[,] result = _game.EvolveXGenerations(board, 0);

            // Assert
            Assert.Equal(board, result);
        }

        [Fact]
        public void EvolveXGenerations_FinalStepCicle()
        {
            // Arrange
            int[,] board = 
            {
                { 1, 0, 1 },
                { 0, 1, 0 },
                { 1, 0, 1 }
            };

            int[,] boardExpected =
{
                { 0, 1, 0 },
                { 1, 0, 1 },
                { 0, 1, 0 }
            };

            // Act & Assert
            var result = _game.EvolveXGenerations(board, 5, true);

            // Assert
            Assert.Equal(result, boardExpected);
        }
    }
}
