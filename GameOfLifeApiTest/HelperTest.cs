using GameOfLifeApi.Helpers;

namespace GameOfLifeApiTest
{
    public class HelperTest
    {
        [Fact]
        public void ConvertTo2DArray_ValidList_ReturnsCorrectArray()
        {
            // Arrange
            var list = new List<List<int>>
            {
                new List<int> { 1, 0, 1 },
                new List<int> { 0, 1, 0 },
                new List<int> { 1, 0, 1 }
            };

            int[,] expectedArray =
            {
                { 1, 0, 1 },
                { 0, 1, 0 },
                { 1, 0, 1 }
            };

            // Act
            int[,] result = Helper.ConvertTo2DArray(list);

            // Assert
            Assert.Equal(expectedArray, result);
        }

        [Fact]
        public void ConvertTo2DArray_NonRectangularList_ThrowsException()
        {
            // Arrange
            var invalidList = new List<List<int>>
            {
                new List<int> { 1, 0 },
                new List<int> { 0, 1, 0 }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Helper.ConvertTo2DArray(invalidList));
            Assert.Equal("The board should be rectangular (all rows of the same size).", exception.Message);
        }

        [Fact]
        public void ConvertToList_ValidArray_ReturnsCorrectList()
        {
            // Arrange
            int[,] array =
            {
                { 1, 0, 1 },
                { 0, 1, 0 },
                { 1, 0, 1 }
            };

            var expectedList = new List<List<int>>
            {
                new List<int> { 1, 0, 1 },
                new List<int> { 0, 1, 0 },
                new List<int> { 1, 0, 1 }
            };

            // Act
            var result = Helper.ConvertToList(array);

            // Assert
            Assert.Equal(expectedList, result);
        }

        [Fact]
        public void ConvertToList_EmptyArray_ReturnsEmptyList()
        {
            // Arrange
            int[,] emptyArray = new int[0, 0];

            // Act
            var result = Helper.ConvertToList(emptyArray);

            // Assert
            Assert.Empty(result);
        }
    }
}
