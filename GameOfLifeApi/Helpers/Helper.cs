namespace GameOfLifeApi.Helpers
{
    public class Helper
    {
        public static int[,] ConvertTo2DArray(List<List<int>> list)
        {
            int rows = list.Count;
            int cols = list[0].Count;
            int[,] array = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                if (list[i].Count != cols)
                    throw new ArgumentException("The board should be rectangular (all rows of the same size).");

                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = list[i][j];
                }
            }

            return array;
        }

        public static List<List<int>> ConvertToList(int[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            var list = new List<List<int>>();

            for (int i = 0; i < rows; i++)
            {
                var row = new List<int>();
                for (int j = 0; j < cols; j++)
                {
                    row.Add(array[i, j]);
                }
                list.Add(row);
            }

            return list;
        }
    }
}
