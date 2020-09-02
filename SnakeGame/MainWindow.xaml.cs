using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using Cubit32;
using Cubit32.Neurals;
using System.Threading.Tasks;

namespace SnakeGame
{
   public partial class MainWindow : Window
   {
      private int BoardWH = 7;
      internal Board Board1 { get; set; }
      private Rectangle[] FieldRectangles { get; set; }
      private Brain SnakeBrain { get; set; }
      private System.Timers.Timer MyTimer { get; set; }
      private double ScoreTreshold { get; set; } = 2e3;
      private int TimerInterval { get; set; } = 75;
      private double MinScore { get; set; }
      private readonly int RectWidth = 20;
      private readonly int RectSpacing = 2;
      private readonly Random rng = new Random();
      private readonly BinaryFormatter Formatter = new BinaryFormatter();
      private int savedFilesCount = 0;
      private int savedFilesMax = 10000;
      public Task AvgScoreTask { get; set; }
      public int Counter { get; set; }

      public MainWindow()
      {
         InitializeComponent();

         MainWindow1.Width = 22 + (RectWidth + RectSpacing) * 15;
         MainWindow1.Height = 42 + (RectWidth + RectSpacing) * 15;
      }
      /// <summary>
      /// Draws a physical representation of the board
      /// </summary>
      private void DrawFieldRectangles()
      {
         FieldRectangles = new Rectangle[BoardWH * BoardWH];
         for (int i = 0; i < BoardWH; i++)
            for (int j = 0; j < BoardWH; j++)
            {
               Rectangle rect = new Rectangle
               {
                  Width = RectWidth,
                  Height = RectWidth,
                  Margin = new Thickness(5 + (RectWidth + RectSpacing) * i, 0, 0, 5 + (RectWidth + RectSpacing) * j),
                  Fill = ReturnFieldBrush(Board1.Fields[i + j * BoardWH]),
                  VerticalAlignment = VerticalAlignment.Bottom,
                  HorizontalAlignment = HorizontalAlignment.Left
               };
               FieldRectangles[i + j * BoardWH] = rect;
               MainGrid.Children.Add(FieldRectangles[i + j * BoardWH]);
            }

      }

      /**
       * <summary>Generates new brains, the size of the brain is determined by height * width</summary>
       * <param name="hlHeight">determines thd width of the hidden layer</param>
       * <param name="hlWidth">determines the height of the hidden layers</param>
       */
      private void ModeGenerateNewBrains(int hlWidth, int hlHeight)
      {
         int iterator = 0;

         NewBoard(iterator);
         DrawFieldRectangles();
         NewBrain(hlWidth, hlHeight);

         while (savedFilesCount < savedFilesMax)
         {
            RedrawField();
            bool gameOver = true;
            for (int i = 0; i < 1000; i++)
            {
               if (Board1.Progress1Tick())
               {
                  RunBrain(Board1, SnakeBrain);
                  gameOver = false;
               }
               else
               {
                  gameOver = true;
                  break;
               }
            }
            if (!gameOver)
               continue;

            Board1.GameOver = true;
            //gameover
            double score = CalculateScore(Board1);

            if (score > ScoreTreshold)
            {
               try
               {
                  string filename = "SnakeBrainFile" + Math.Floor(score).ToString() + "-" + DateTime.Now.Ticks.ToString() + ".dat";
                  FileStream SnakeBrainFile =
                      new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                  Formatter.Serialize(SnakeBrainFile, SnakeBrain);
                  SnakeBrainFile.Close();
                  savedFilesCount++;
                  //MessageBox.Show("Saved snakebrain as " + filename);
               }
               catch (Exception err)
               {
                  MessageBox.Show("Error saving snake brain.");
                  MessageBox.Show(err.Message);
               }

               //MessageBox.Show("You crashed into your own tail and died or you ran out of time, your final score was " + string.Format("{0:N2}", score) + "\n\nGrow more quickly and grow larger to gain a larger score");
            }

            iterator++;
            NewBoard(iterator);
            NewBrain(hlWidth, hlHeight);
         }
         MessageBox.Show("End of program, " + savedFilesCount.ToString() + " brain files generated.\nReview the executables' folder to see the brain files");
         Restart();
      }

      /// <summary>
      /// this mode is intended to demonstrate an AI (aka. snake brain) from a file
      /// </summary>
      private void ModeDemoAi()
      {
         NewBoard(Counter);
         DrawFieldRectangles();

         MyTimer = new System.Timers.Timer();
         MyTimer.Elapsed += DemoAIEvent;
         MyTimer.Interval = TimerInterval;
         MyTimer.Enabled = true;
      }

      /// <summary>
      /// runs the main brain a few times after mutating it and returns its average score
      /// </summary>
      /// <param name="mutateMagnitude">The magnitude by which mutations can happen</param>
      /// <param name="iterations">The number of times the snake plays the game before returning the average score.</param>
      /// <param name="mutateNRR">Determines how much mutation magnitude distrubution conforms to a normal distribution.</param>
      /// <param name="randomly">Whether or not to use deterministic brain changes</param>
      /// <param name="incrementor">if non-random then this must should increment by 1 each call</param>
      /// <returns>the average score</returns>
      private double ModeTrainAI(double mutateMagnitude = .1, int mutateNRR = 4, int iterations = 100, int nrOfMutations = 2, bool randomly = false, int incrementor = 0)
      {
         if (randomly) SnakeBrain.Mutate(mutateMagnitude, mutateNRR, nrOfMutations);
         double averageScore = 0;

         for (int i = 0; i < iterations; i++)
         {
            NewBoard(i);
            while (Board1.Progress1Tick())
               RunBrain(Board1, SnakeBrain);
            averageScore += CalculateScore(Board1);
         }
         return averageScore / iterations;
      }

      private void DemoAIEvent(object source, ElapsedEventArgs e)
      {
         Dispatcher.Invoke(() =>
         {
            DemoAICycle();
         });
      }

      private void DemoAICycle()
      {
         RedrawField();
         if (Board1.Progress1Tick())
            RunBrain(Board1, SnakeBrain);
         else
         {//gameOver
            Counter++;
            NewBoard(Counter);
         }
      }

      private void RedrawField()
      {
            Board.Field[,] board = RotateBoard(Board1);
         if (false)
            board = RotateBoard(Board1);
         else {
            
            int counter = 0;
            board = new Board.Field[Board1.WidthHeight, Board1.WidthHeight];
            foreach (var field in Board1.Fields)
            {
               board[counter % Board1.WidthHeight, counter / Board1.WidthHeight] = field;
               counter++;
            }
         } 

         for (int i = 0; i < BoardWH; i++)
            for (int j = 0; j < BoardWH; j++)
            {
               int t = i + j * BoardWH;
               FieldRectangles[t].Fill = ReturnFieldBrush(board[t/BoardWH, t%BoardWH]);
            }
      }

      /// <summary>
      /// Rotates the board to a standard orientation so that the snake direction alwyas ends up facing the same way
      /// </summary>
      /// <param name="initBoard"></param>
      /// <returns></returns>
      private Board.Field[,] RotateBoard(Board initBoard)
      {
         int times = (int)initBoard.SnakeDirection;

         int counter = 0;
         Board.Field[,] board = new Board.Field[initBoard.WidthHeight, initBoard.WidthHeight];
         foreach (var field in initBoard.Fields)
         {
            board[counter % initBoard.WidthHeight, counter / initBoard.WidthHeight] = field;
            counter++;
         }

         board = Arrays.RotateHorizontally(board, initBoard.WidthHeight / 2 - initBoard.SnakeHeadX < 0 ? initBoard.WidthHeight / 2 - initBoard.SnakeHeadX + initBoard.WidthHeight : initBoard.WidthHeight / 2 - initBoard.SnakeHeadX);
         board = Arrays.RotateVertically(board, initBoard.WidthHeight / 2 - initBoard.SnakeHeadY < 0 ? initBoard.WidthHeight / 2 - initBoard.SnakeHeadY + initBoard.WidthHeight : initBoard.WidthHeight / 2 - initBoard.SnakeHeadY);
         
         for (int i = 0; i < 3 - times; i++)
         {
            board = Arrays.RotateMatrix(board, initBoard.WidthHeight);
         }

         return board;
      }

      private void RunBrain(Board board, Brain brain)
      {
         Board.Field[,] temp = RotateBoard(board);
         double[,] perceptronValues = new double[board.WidthHeight, board.WidthHeight];

         int counter = 0;
         foreach (Board.Field field in temp)
         {
            perceptronValues[counter % BoardWH, counter / BoardWH] = (double)field;
            counter++;
         }

         List<double> perceptronsOutput = new List<double>();
         foreach (double val in perceptronValues)
         {
            perceptronsOutput.Add(val);
         }

         //calculate what the brain "thinks" that it should do
         double[] brainThoughts = brain.InputToOutput(perceptronsOutput.ToArray());
         double maxvalue = brainThoughts.Max();
         for (int i = 0; i < 3; i++)
         {
            if (maxvalue == brainThoughts[i])
            {
               if (i == 1)
               {
                  board.ChangeDirection(Board.Direction.Right);
               }
               else if (i == 2)
               {
                  board.ChangeDirection(Board.Direction.Left);
               }
               break;
            }
         }
      }

      /// <summary> Calculates an average score for a given brain and reports it to a label </summary>
      /// <param name="brain"></param>
      private void ModeAvgScore(Brain brain)
      {

         AvgScoreTask = Task.Run(() =>
         {

            double totalScore = 0;
            Board board;
            int iterations = 0;
            double minScore = 999999999;
            double avgTime = 0;
            double avgTurns = 0;

            while (iterations < 100)
            {
               board = new Board(BoardWH, iterations % 100);
               while (board.Progress1Tick())
               {
                  RunBrain(board, brain);
               }
               minScore = Math.Min(CalculateScore(board), minScore);
               avgTime += board.Tick;
               avgTurns += board.Turns;
               totalScore += CalculateScore(board);
               
               if (iterations == 99)
                  Dispatcher.Invoke(() =>
                  {
                     MoveScoreToLabel(totalScore / iterations);
                  });
               iterations++;
            }

            avgTime /= 100;
            avgTurns /= 100;
         
            Console.WriteLine(minScore);
            Console.WriteLine(avgTime);
            Console.WriteLine(avgTurns);
         });

      }

      private void NewBrain(int hlWidth, int hlHeight)
      {
         SnakeBrain = new Brain(Board1.FieldsCount, hlWidth, hlHeight, 3);
      }

      private void MoveScoreToLabel(double score)
      {
         ModeGetAvgScoreLabel.Content = Math.Floor(score * 100) / 100;
      }

      private void NewBoard(int i = -1)
      {
         Board1 = new Board(BoardWH, i);
      }

      private Brush ReturnFieldBrush(Board.Field field)
      {

         switch (field)
         {
            case Board.Field.Empty:
               return Brushes.White;
            case Board.Field.Head:
               return Brushes.Black;
            case Board.Field.Tail:
               return Brushes.Green;
            case Board.Field.Food:
               return Brushes.LightGreen;
         }
         return Brushes.White;
      }

      private void Pause()
      {
         if (MyTimer.Enabled)
            MyTimer.Enabled = false;
         else
            MyTimer.Enabled = true;
      }

      private double CalculateScore(Board board)
      {
         return board.TailLength * 1e3 / Math.Log(board.Tick) / Math.Log(board.Turns) / board.FieldsCount * board.TailLength;
      }

      /// <summary>
      /// Restart the program
      /// </summary>
      private void Restart()
      {
         Process.Start(Application.ResourceAssembly.Location);
         Thread.Sleep(100);
         Application.Current.Shutdown();
      }

      private void Window_KeyDown(object sender, KeyEventArgs e)
      {
         switch (e.Key)
         {
            case Key.R:
               Restart();
               break;
            case Key.P:
               RedrawField();
               Pause();
               break;
            default:
               break;
         }
      }

      private void ModeNewBrainsBtn_Click(object sender, RoutedEventArgs e)
      {
         if (!double.TryParse(ModeNewBrainsTresholdBox.Text, out double dbl))
         {
            MessageBox.Show("Invalid treshold value");
            return;
         }
         ScoreTreshold = dbl;
         if (!int.TryParse(ModeNewBrainsQuantityBox.Text, out int fileMax))
         {
            MessageBox.Show("Invalid desired brains value, input an integer");
            return;
         }
         if (!int.TryParse(ModeNewBrainsHLWidthBox.Text, out int hlWidth))
         {
            MessageBox.Show("Invalid desired HL width value, input an integer");
            return;
         }
         if (!int.TryParse(ModeNewBrainsHLHeightBox.Text, out int hlHeight))
         {
            MessageBox.Show("Invalid desired HL height value, input an integer");
            return;
         }
         savedFilesMax = fileMax;

         int len = MainGrid.Children.Count;
         for (int i = 0; i < len; i++)
            MainGrid.Children.RemoveAt(0);

         ModeGenerateNewBrains(hlWidth, hlHeight);
      }

      private void ModeDemoAIButton_Click(object sender, RoutedEventArgs e)
      {
         if (!int.TryParse(ModeDemoAIInterval.Text, out int interval))
         {
            MessageBox.Show("Invalid value for interval, please input an integer");
            return;
         }
         if (!int.TryParse(ModeDemoAISeed.Text, out int Counter) || Counter < 0)
         {
            MessageBox.Show("Invalid seed or too small seed, please input an integer >= 0");
            return;
         }

         TimerInterval = interval;
         SnakeBrain = GetBrainFromFile();

         int len = MainGrid.Children.Count;
         for (int i = 0; i < len; i++)
            MainGrid.Children.RemoveAt(0);

         ModeDemoAi();
      }

      private void ModeTrainAIButton_Click(object sender, RoutedEventArgs e)
      {
         // trust but verify
         if (!double.TryParse(ModeTrainAIDegreeBox.Text, out double degree))
         {
            MessageBox.Show("Invalid interval value, please input a double (aka. decimal) value");
            return;
         }
         if (!int.TryParse(ModeTrainAIIterationsBox.Text, out int iterations))
         {
            MessageBox.Show("Invalid iterations value, please input an integer");
            return;
         }
         if (!int.TryParse(ModeTrainAIMutationChanceBox.Text, out int mutationChance))
         {
            MessageBox.Show("Invalid mutation chance value, please input an integer");
            return;
         }
         if (!int.TryParse(ModeTrainAINRRBox.Text, out int nrr))
         {
            MessageBox.Show("Invalid NRR value, please input an integer");
            return;
         }
         if (!double.TryParse(ModeTrainAITresholdBox.Text, out double threshold))
         {
            MessageBox.Show("Invalid treshold value, please input a double");
            return;
         }

         SnakeBrain = GetBrainFromFile();

         //switch to deterministic
         if (TrainAiDeterministically.IsChecked == true)
         {
            ModeTrainDeterministically(iterations, degree);
            return;
         }

         int len = MainGrid.Children.Count;
         for (int i = 0; i < len; i++)
            MainGrid.Children.RemoveAt(0);

         NewBoard();
         DrawFieldRectangles();

         double prevScore = AverageScore(SnakeBrain, iterations);
         MinScore = prevScore * 0.75;

         Brain prevBrain = DeepCopy(SnakeBrain);
         int count = 0;
         while (++count <= 1000000000)
         {
            double score = ModeTrainAI(degree, nrr, iterations, mutationChance);
            if (score / threshold > prevScore && score > MinScore)
            {
               if (score * .75 > MinScore)
               {
                  MinScore = score * .75;
                  try
                  {
                     string filename = "TrainedSnakeBrainFile" + Math.Floor(score).ToString() + "-" + SnakeBrain.HiddenLayerWidth + "-" + SnakeBrain.HiddenLayerHeight + "-" + iterations.ToString() + "-" + DateTime.Now.Ticks.ToString() + ".dat";
                     SaveBrainToFile(filename, SnakeBrain);
                     savedFilesCount++;
                  }
                  catch (Exception err)
                  {
                     MessageBox.Show("Error saving snake brain.");
                     MessageBox.Show(err.Message);
                  }
               }
               prevScore = score;
               prevBrain = DeepCopy(SnakeBrain);
            }
            else
            {
               SnakeBrain = DeepCopy(prevBrain);
            }
            if (savedFilesCount > savedFilesMax)
               break;
         }
      }

      private void ModeTrainDeterministically(int iterations, double mag)
      {
         double prevScore = AverageScore(SnakeBrain, iterations);
         double oldScore = prevScore;

         int count = 0;
         Brain newBrain = DeepCopy(SnakeBrain);
         while (count++ < 2_000_000_000)
         {
            foreach (double magnitude in new double[]{ 0.1, 0.5, 0.8, 0.95, -1, 1.05, 1.3, 1.5, 2, 4, 8 })
            {
               for (int i = 0; i < 2; i++)
               {
                  newBrain.MutateDeterministically(count, magnitude * mag, i % 2  == 0);
                  double prevScore2 = AverageScore(newBrain, iterations);

                  //apply new brain if it scores better
                  if (prevScore2 > prevScore) {
                     SnakeBrain = DeepCopy(newBrain);
                     
                     //don't save for every other  minute improvement
                     if (prevScore2 > oldScore * 1.05) {
                        string filename = "NonRandomBrain" + Math.Floor(prevScore2).ToString() + "-" + SnakeBrain.HiddenLayerWidth + "-" + SnakeBrain.HiddenLayerHeight + "-" + iterations.ToString() + "-" + DateTime.Now.Ticks.ToString() + ".dat";
                        SaveBrainToFile(filename, newBrain);
                        oldScore = prevScore2;
                     }

                     prevScore = prevScore2;
                  }
                  //reverse if it doesn't score better
                  else
                  {
                     newBrain = DeepCopy(SnakeBrain);
                  }
               }
            }
         }
      }

      /// <summary>
      /// Gets the average score a brain gets over a number of boards
      /// </summary>
      /// <returns></returns>
      private double AverageScore(Brain brain, int runs)
      {
         double minScore = 999999999;
         double avgTime = 0;
         double prevScore = 0;
         double avgTurns = 0;
         for (int i = 0; i < runs; i++)
         {
            NewBoard(i);
            while (Board1.Progress1Tick())
               RunBrain(Board1, brain);
            prevScore += CalculateScore(Board1);
            minScore = Math.Min(CalculateScore(Board1), minScore);
            avgTime += Board1.Tick;
            avgTurns += Board1.Turns;
         }
         avgTime /= runs;
         avgTurns /= runs;
         Console.WriteLine(minScore);
         Console.WriteLine(avgTime);
         Console.WriteLine(avgTurns);
         return prevScore / runs;
      }

      private Brain GetBrainFromFile()
      {
         OpenFileDialog dlg = new OpenFileDialog
         {
            DefaultExt = ".dat",
            Filter = "Data Files (*.dat)|*.dat"
         };

         bool? result = dlg.ShowDialog();
         if (result != true)
         {
            MessageBox.Show("Couldn't recover file path");
            return null;
         }


         if (!File.Exists(dlg.FileName))
         {
            MessageBox.Show("Couldn't find that file");
            return null;
         }

         try
         {
            FileStream SnakeBrainInstanceFromFile = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read);
            Brain brain = (Brain)Formatter.Deserialize(SnakeBrainInstanceFromFile);
            SnakeBrainInstanceFromFile.Close();
            return brain;
         }
         catch (Exception err)
         {
            MessageBox.Show("Could not load selected snake brain file for this demo");
            MessageBox.Show(err.Message);
            return null;
         }

      }

      private void SaveBrainToFile(string filename, Brain brain)
      {
         FileStream SnakeBrainFile =
               new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
         Formatter.Serialize(SnakeBrainFile, brain);
         SnakeBrainFile.Close();
      }

      /// <summary>
      /// Makes an independent deep copy of a class instance<br/>
      /// https://stackoverflow.com/a/1031062/10055628
      /// </summary>
      /// <typeparam name="T">The Class type</typeparam>
      /// <param name="other"></param>
      /// <returns></returns>
      public static T DeepCopy<T>(T other)
      {
         using (MemoryStream ms = new MemoryStream())
         {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, other);
            ms.Position = 0;
            return (T)formatter.Deserialize(ms);
         }
      }

      private void ModeTrainAIInfoButton_Click(object sender, RoutedEventArgs e)
      {
         MessageBox.Show("This mutates a snake brain and checks whether or not it scores better than the last best mutation. The saved file name numbers (in order) represent average score, hidden layer width, hidden layer height, number of iterations and universal time in ticks.");
      }

      private void ModeNewBrainHelpButton_Click(object sender, RoutedEventArgs e)
      {
         MessageBox.Show("Generates new brains that can be input into the training function. The first number in the file represents its obtained score (given only 1 run)");
      }

      private void ModeGetAvgScore_Click(object sender, RoutedEventArgs e)
      {
         ModeAvgScore(GetBrainFromFile());
      }
   }
}
