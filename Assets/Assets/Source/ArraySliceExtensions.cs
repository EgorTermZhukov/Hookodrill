using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Assets.Source
{
    internal static class ArraySliceExtensions
    {
        public static IEnumerable<Vector2Int> GetSliceIndices<T>(this T[,] array, int startRow, int startColumn, bool isHorizontal)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            if (startRow < 0 || startRow >= rows)
                throw new ArgumentOutOfRangeException(nameof(startRow));
            if (startColumn < 0 || startColumn >= cols)
                throw new ArgumentOutOfRangeException(nameof(startColumn));

            if (isHorizontal)
            {
                for (int j = startColumn; j < cols; j++)
                {
                    yield return new(startRow, j);
                }
            }
            else
            {
                for (int i = startRow; i < rows; i++)
                {
                    yield return new(i, startColumn);
                }
            }
        }
    }
}
