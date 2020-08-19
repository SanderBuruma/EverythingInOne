using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
   class Board
   {
      public int WidthHeight { get; set; }
      public int FieldsCount { get; set; }
      public Field[] Fields { get; set; }
      public Direction SnakeDirection { get; set; }
      public int SnakeHeadX { get; set; }
      public int SnakeHeadY { get; set; }
      public int FoodX { get; set; }
      public int FoodY { get; set; }
      public int[] TailX { get; set; }
      public int[] TailY { get; set; }
      public int TailLength { get; set; }
      public int Score { get; set; }
      public int Tick { get; set; } = 0;
      public int Turns { get; set; } = 2;//needs to be >1 so log10 doesn't make 0 of it.
      public int TicksLeft { get; set; } = 100;
      public bool GameOver = false;
      private bool CanSwitchDir = true;
      private readonly Random Rng = new Random();
      public Board(int widthHeight, int i = -1)
      {
         if (i > -1) Rng = new Random(i);
         WidthHeight = widthHeight;

         SnakeDirection = Direction.Right;
         SnakeHeadX = (int)Math.Floor((double)WidthHeight / 2);
         SnakeHeadY = (int)Math.Floor((double)WidthHeight / 2);

         TailX = new int[WidthHeight * WidthHeight];
         TailY = new int[WidthHeight * WidthHeight];
         TailLength = 1;
         TailX[0] = SnakeHeadX - 1;
         TailY[0] = SnakeHeadY;

         int rnNum = FindEmptyField();
         FoodX = X(rnNum);
         FoodY = Y(rnNum);

         FieldsCount = widthHeight * widthHeight;
         Fields = new Field[FieldsCount];

         RedrawFields();
      }

      /// <summary>
      /// Changes direction relative to the snake's current heading.
      /// </summary>
      /// <param name="dir"></param>
      public void ChangeDirection(Direction dir)
      {
         int k = (int)SnakeDirection;
         if (CanSwitchDir)
         {
            if (dir == Direction.Right)
            {
               SnakeDirection = (Direction)((k + 1) % 4);
               Turns++;
            }
            else if (dir == Direction.Left)
            {
               SnakeDirection = (Direction)((k + 3) % 4);
               Turns++;
            }
         }
      }
      ///<summary> Progresses the board by 1 tick </summary>
      /// <returns> true if the game is not in a defeat state </returns>
      public bool Progress1Tick()
      {
         CanSwitchDir = true;

         //move tail
         int prevX = TailX[0];
         int prevY = TailY[0];
         TailX[0] = SnakeHeadX;
         TailY[0] = SnakeHeadY;
         for (int i = 1; i < TailLength; i++)
         {
            int prevX2 = TailX[i];
            int prevY2 = TailY[i];

            TailX[i] = prevX;
            TailY[i] = prevY;

            prevX = prevX2;
            prevY = prevY2;
         }

         //move head
         switch (SnakeDirection)
         {
            case Direction.Up:
               SnakeHeadY++;
               if (SnakeHeadY >= WidthHeight) SnakeHeadY = 0;
               break;
            case Direction.Right:
               SnakeHeadX++;
               if (SnakeHeadX >= WidthHeight) SnakeHeadX = 0;
               break;
            case Direction.Down:
               SnakeHeadY--;
               if (SnakeHeadY < 0) SnakeHeadY = WidthHeight - 1;
               break;
            case Direction.Left:
               SnakeHeadX--;
               if (SnakeHeadX < 0) SnakeHeadX = WidthHeight - 1;
               break;
         }


         //check if we're eating food
         if (SnakeHeadX == FoodX && SnakeHeadY == FoodY)
         {
            int ef = FindEmptyField();
            TailX[TailLength] = TailX[TailLength - 1];
            TailY[TailLength] = TailY[TailLength - 1];
            if (TailLength < FieldsCount)
               TailLength++;
            Score += 10;
            FoodX = X(ef);
            FoodY = Y(ef);

            TicksLeft = 200;
         }

         Tick++;
         RedrawFields();

         //check for collissions
         for (int i = 0; i < TailLength; i++)
         {
            if (TailX[i] == SnakeHeadX && TailY[i] == SnakeHeadY)
            {
               //return defeat if we crash into our tail
               return false;
            }
         }
         //return defeat (because of timeout)
         if (--TicksLeft < 1)
            return false;

         return true;
      }
      private void RedrawFields()
      {
         for (int i = 0; i < FieldsCount; i++)
            Fields[i] = Field.Empty;

         for (int i = 0; i < TailLength; i++)
            Fields[TailX[i] + TailY[i] * WidthHeight] = Field.Tail;

         Fields[SnakeHeadX + SnakeHeadY * WidthHeight] = Field.Head;
         Fields[FoodX + FoodY * WidthHeight] = Field.Food;
      }
      private int FindEmptyField()
      {//randomly finds empty field

         int max = (int)Math.Pow(WidthHeight, 2);
         int randomField;

         while (true)
         {
            randomField = Rng.Next(max);

            bool isEmpty = true;
            if (SnakeHeadX == X(randomField) ||
                SnakeHeadY == Y(randomField))
            {
               isEmpty = false;
            }

            if (isEmpty)
               for (int i = 0; i < TailLength; i++)
               {
                  if (TailX[i] == X(randomField) &&
                      TailY[i] == Y(randomField))
                  {
                     isEmpty = false;
                     break;
                  }
               }
            if (isEmpty) return randomField;
         }
      }
      public int DistanceLeft(int x1, int x2)
      {
         if (x1 > x2)
            return x1 - x2;
         else if (x1 < x2)
            return x1 + (WidthHeight - x2);
         else
            return 0;
      }
      public int DistanceUp(int y1, int y2)
      {
         if (y2 > y1)
            return y2 - y1;
         else if (y2 < y1)
            return y2 + (WidthHeight - y1);
         else
            return 0;
      }
      public int DistanceRight(int x1, int x2)
      {
         if (x2 > x1)
            return x2 - x1;
         else if (x2 < x1)
            return x2 + (WidthHeight - x1);
         else
            return 0;
      }
      public int DistanceDown(int y1, int y2)
      {
         if (y1 > y2)
            return y1 - y2;
         else if (y1 < y2)
            return y1 + (WidthHeight - y2);
         else
            return 0;
      }
      private int X(int num)
      {// returns the X position congruent with the value of num
         return num % WidthHeight;
      }
      private int Y(int num)
      {// returns the Y position congruent with the value of num
         return (int)Math.Floor((decimal)num / WidthHeight);
      }
      internal enum Field
      {
         Empty,
         Head,
         Tail,
         Food = -10
      }
      internal enum Direction
      {
         Left = 0,
         Up = 1,
         Right = 2,
         Down = 3
      }
   }

}
