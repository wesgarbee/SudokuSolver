using System;
using System.Threading;

namespace Sudoku_Solver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Controller controller = new Controller();
            controller.GetBoardFromUser();
        }
    }

    public class Controller
    {
        public Sudoku sudoku { get; set; }

        public int[][] gridToSolve = new int[9][];

        public Controller()
        {
            sudoku = new Sudoku();
        }

        public void GetBoardFromUser()
        {
            int rowNumber = 0;

            String userInput = "";

            int[] row;

            do
            {
                userInput = ConsoleUtils.GetRow();

                String[] rowInput = userInput.Split(",");

                row = new int[9];
                int idx = 0;

                foreach (String num in rowInput)
                {
                    row[idx] = Convert.ToInt32(num);
                    idx++;
                }

                gridToSolve[rowNumber] = row;

                rowNumber++;

                Console.Clear();
            } while (rowNumber < 9);

            PassUserEntry(gridToSolve);

            ConsoleUtils.PrintMessage("Sudoku grid to solve");

            ConsoleUtils.PrintMessage(sudoku.DisplayBoard());

            if (sudoku.SolveSudoku())
            {
                ConsoleUtils.PrintMessage("Sudoku solved using BackTracking");
                ConsoleUtils.PrintMessage(sudoku.DisplayBoard());
            }
            else
            {
                Console.WriteLine("Unsolvable");
            }
        }

        public void PassUserEntry(int[][] gridToSolve)
        {
            sudoku = new Sudoku(gridToSolve);
        }
    }

    public class Sudoku
    {
        public int[][] board;

        public static int empty = 0;

        public static int size = 9;

        public Sudoku()
        {

        }

        // Sudoku constructor that takes in the grid the user wants to solve
        // and passes to the board property for the display to be generated
        public Sudoku(int[][] gridToSolve)
        {
            this.board = new int[size][];

            for (int i = 0; i < size; i++)
            {
                board[i] = new int[size];

                for (int j = 0; j < size; j++)
                {
                    this.board[i][j] = gridToSolve[i][j];
                }
            }
        }

        // Checks to see if the number is in the row
        private bool IsInRow(int row, int number)
        {
            for (int i = 0; i < size; i++)
            {
                if (board[row][i] == number)
                {
                    return true;
                }
            }
            return false;
        }

        // Checks to see if the number is in the column
        private bool IsInColumn(int column, int number)
        {
            for (int i = 0; i < size; i++)
            {
                if (board[i][column] == number)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsInBox(int row, int column, int number)
        {
            int r = row - row % 3;
            int c = column - column % 3;

            for (int i = r; i < r + 3; i++)
            {
                for (int j = c; j < c + 3; j++)
                {
                    if (board[i][j] == number)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ValidNumberPlacement(int row, int column, int number)
        {
            return !IsInRow(row, number) && !IsInColumn(column, number) && !IsInBox(row, column, number);
        }

        public bool SolveSudoku()
        {
            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    if (board[row][column] == 0)
                    {
                        for (int number = 1; number <= size; number++)
                        {
                            if (ValidNumberPlacement(row, column, number))
                            {
                                board[row][column] = number;

                                if (SolveSudoku())
                                {
                                    return true;
                                }
                                else
                                {
                                    board[row][column] = 0;
                                }
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        public String DisplayBoard()
        {
            String FormattedBoard = "";
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    FormattedBoard += " " + board[i][j];
                }
                FormattedBoard += "\n";
            }
            return FormattedBoard;
        }
    }

    public class ConsoleUtils
    {
        public static String GetRow()
        {
            Console.WriteLine("Please enter line {0}, each cell separated by a comma");
            Console.WriteLine("9,0,0,1,0,0,0,0,5 for example");
            return Console.ReadLine();
        }

        public static void PrintMessage(String message)
        {
            Console.WriteLine(message);
        }
    }
}
