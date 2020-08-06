using System;
using System.Collections.Generic;
using System.Text;

namespace Cubit32
{
   public static class Arrays
   {
      /// <summary>
      /// Rotates a 2D array
      /// </summary>
      /// <param name="matrix">the 2D array</param>
      /// <param name="n">Number of times to rotate clockwise</param>
      /// <returns></returns>
      //credit to https://stackoverflow.com/a/42535/10055628 & https://stackoverflow.com/a/20351411/10055628
      public static T[,] RotateMatrix<T>(T[,] matrix, int n) {

          T[,] ret = new T[n,n];

          for (int i = 0; i < n; ++i) {
              for (int j = 0; j < n; ++j) {
                  ret[i, j] = matrix[n - j - 1, i];
              }
          }

          return ret;
      }

      /// <summary> move things over to the right and put the things that fall off back in on the left. </summary
      public static T[,] RotateHorizontally<T>(T[,] matrix, int theNumberOfColumnsToShift)
      {
         int rows = matrix.GetLength(0);
         int columns = matrix.GetLength(1);

         T[,] newMatrix = new T[rows, columns];
         for (int x = 0; x < columns; x++)
         {
            for (int y = 0; y < rows; y++)
            {
               newMatrix[x,y] = matrix[(x+columns-theNumberOfColumnsToShift)%columns, y];
            }
         }

         return newMatrix;
      }
      /// <summary> move things over to the bottom and put the things that fall off back in on the top. </summary>=
      public static T[,] RotateVertically<T>(T[,] matrix, int theNumberOfRowsToShift)
      {
         int rows = matrix.GetLength(0);
         int columns = matrix.GetLength(1);

         T[,] newMatrix = new T[rows, columns];
         for (int x = 0; x < columns; x++)
         {
            for (int y = 0; y < rows; y++)
            {
               newMatrix[x,y] = matrix[x, (y+rows-theNumberOfRowsToShift)%rows];
            }
         }

         return newMatrix;
      }

   }
}
