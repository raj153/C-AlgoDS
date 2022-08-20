using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.OOP2
{
	public class Board
	{
		public int nRows;
		public int nColumns;
		public int nBombs = 0;
		public Cell[,] cells;
		public Cell[] bombs;
		public int numUnexposedRemaining;


		public Board(int r, int c, int b)
		{
			nRows = r;
			nColumns = c;
			nBombs = b;

			initializeBoard();
			shuffleBoard();
			setNumberedCells();

			numUnexposedRemaining = nRows * nColumns - nBombs;
		}

		public void initializeBoard()
		{
			cells = new Cell[nRows, nColumns];
			bombs = new Cell[nBombs];
			for (int r = 0; r < nRows; r++)
			{
				for (int c = 0; c < nColumns; c++)
				{
					cells[r,c] = new Cell(r, c);
				}
			}

			for (int i = 0; i < nBombs; i++)
			{
				int r = i / nColumns;
				int c = (i - r * nColumns) % nColumns;
				bombs[i] = cells[r,c];
				bombs[i].setBomb(true);
			}
		}

		public void shuffleBoard()
		{
			int nCells = nRows * nColumns;
			Random random = new Random();
			for (int index1 = 0; index1 < nCells; index1++)
			{
				int index2 = index1 + random.Next(nCells - index1);
				if (index1 != index2)
				{
					/* Get cell at index1. */
					int row1 = index1 / nColumns;
					int column1 = (index1 - row1 * nColumns) % nColumns;
					Cell cell1 = cells[row1,column1];

					/* Get cell at index2. */
					int row2 = index2 / nColumns;
					int column2 = (index2 - row2 * nColumns) % nColumns;
					Cell cell2 = cells[row2,column2];

					/* Swap. */
					cells[row1,column1] = cell2;
					cell2.setRowAndColumn(row1, column1);
					cells[row2,column2] = cell1;
					cell1.setRowAndColumn(row2, column2);
				}
			}
		}

		public bool inBounds(int row, int column)
		{
			return row >= 0 && row < nRows && column >= 0 && column < nColumns;
		}

		/* Set the cells around the bombs to the right number. Although 
		 * the bombs have been shuffled, the reference in the bombs array
		 * is still to same object. */
		public void setNumberedCells()
		{
			int[,] deltas = { // Offsets of 8 surrounding cells
				{-1, -1}, {-1, 0}, {-1, 1},
				{ 0, -1},          { 0, 1},
				{ 1, -1}, { 1, 0}, { 1, 1}
		};
			foreach (Cell bomb in bombs)
			{
				int row = bomb.getRow();
				int col = bomb.getColumn();
				for (int i=0; i< deltas.GetLength(0); i++)
				{
					int r = row + deltas[i,0];
					int c = col + deltas[i,1];
					if (inBounds(r, c))
					{
						cells[r,c].incrementNumber();
					}
				}
			}
		}

		public void printBoard(bool showUnderside)
		{
			Console.WriteLine();
			Console.WriteLine("   ");
			for (int i = 0; i < nColumns; i++)
			{
				Console.WriteLine(i + " ");
			}
			Console.WriteLine();
			for (int i = 0; i < nColumns; i++)
			{
				Console.WriteLine("--");
			}
			Console.WriteLine();
			for (int r = 0; r < nRows; r++)
			{
				Console.WriteLine(r + "| ");
				for (int c = 0; c < nColumns; c++)
				{
					if (showUnderside)
					{
						Console.WriteLine(cells[r,c].getUndersideState());
					}
					else
					{
						Console.WriteLine(cells[r,c].getSurfaceState());
					}
				}
				Console.WriteLine();
			}
		}

		public bool flipCell(Cell cell)
		{
			if (!cell.IsExposed() && !cell.IsGuess())
			{
				cell.flip();
				numUnexposedRemaining--;
				return true;
			}
			return false;
		}

		public void expandBlank(Cell cell)
		{
			int[,] deltas = {
				{-1, -1}, {-1, 0}, {-1, 1},
				{ 0, -1},          { 0, 1},
				{ 1, -1}, { 1, 0}, { 1, 1}
		};

			Queue<Cell> toExplore = new Queue<Cell>();
			toExplore.Enqueue(cell);

			while (! (toExplore.Count == 0))
			{
				Cell current = toExplore.Dequeue();

				for (int i=0; i< deltas.GetLength(0); i++)
				{
					int r = current.getRow() + deltas[i,0];
					int c = current.getColumn() + deltas[i,1];

					if (inBounds(r, c))
					{
						Cell neighbor = cells[r,c];
						if (flipCell(neighbor) && neighbor.isBlank())
						{
							toExplore.Enqueue(neighbor);
						}
					}
				}
			}
		}

		public UserPlayResult playFlip(UserPlay play)
		{
			Cell cell = getCellAtLocation(play);
			if (cell == null)
			{
				return new UserPlayResult(false, GameState.RUNNING);
			}

			if (play.IsGuess())
			{
				bool guessResult = cell.toggleGuess();
				return new UserPlayResult(guessResult, GameState.RUNNING);
			}

			bool result = flipCell(cell);

			if (cell.IsBomb())
			{
				return new UserPlayResult(result, GameState.LOST);
			}

			if (cell.isBlank())
			{
				expandBlank(cell);
			}

			if (numUnexposedRemaining == 0)
			{
				return new UserPlayResult(result, GameState.WON);
			}

			return new UserPlayResult(result, GameState.RUNNING);
		}

		public Cell getCellAtLocation(UserPlay play)
		{
			int row = play.getRow();
			int col = play.getColumn();
			if (!inBounds(row, col))
			{
				return null;
			}
			return cells[row,col];
		}

		public int getNumRemaining()
		{
			return numUnexposedRemaining;
		}
	}

	public class Cell
	{
		public int row;
		public int column;
		public bool isBomb;
		public int number;
		public bool isExposed = false;
		public bool isGuess = false;

		public Cell(int r, int c)
		{
			isBomb = false;
			number = 0;
			row = r;
			column = c;
		}

		public void setRowAndColumn(int r, int c)
		{
			row = r;
			column = c;
		}

		public void setBomb(bool bomb)
		{
			isBomb = bomb;
			number = -1;
		}

		public void incrementNumber()
		{
			number++;
		}

		public int getRow()
		{
			return row;
		}

		public int getColumn()
		{
			return column;
		}

		public bool IsBomb()
		{
			return isBomb;
		}

		public bool isBlank()
		{
			return number == 0;
		}

		public bool IsExposed()
		{
			return isExposed;
		}

		public bool flip()
		{
			isExposed = true;
			return !isBomb;
		}

		public bool toggleGuess()
		{
			if (!isExposed)
			{
				isGuess = !isGuess;
			}
			return isGuess;
		}

		public bool IsGuess()
		{
			return isGuess;
		}


	public String toString()
		{
			return getUndersideState();
		}

		public String getSurfaceState()
		{
			if (isExposed)
			{
				return getUndersideState();
			}
			else if (isGuess)
			{
				return "B ";
			}
			else
			{
				return "? ";
			}
		}

		public String getUndersideState()
		{
			if (isBomb)
			{
				return "* ";
			}
			else if (number > 0)
			{
				return Convert.ToString(number) + " ";
			}
			else
			{
				return "  ";
			}
		}
	}

	public enum GameState
	{
		WON, LOST, RUNNING
	}

	public class Game
	{

		private Board board;
		private int rows;
		private int columns;
		private int bombs;
		private GameState state;

		public Game(int r, int c, int b)
		{
			rows = r;
			columns = c;
			bombs = b;
			state = GameState.RUNNING;
		}

		public bool initialize()
		{
			if (board == null)
			{
				board = new Board(rows, columns, bombs);
				board.printBoard(true);
				return true;
			}
			else
			{
				Console.WriteLine("Game has already been initialized.");
				return false;
			}
		}

		public bool start()
		{
			if (board == null)
			{
				initialize();
			}
			return playGame();
		}

		public void printGameState()
		{
			if (state == GameState.LOST)
			{
				board.printBoard(true);
				Console.WriteLine("FAIL");
			}
			else if (state == GameState.WON)
			{
				board.printBoard(true);
				Console.WriteLine("WIN");
			}
			else
			{
				Console.WriteLine("Number remaining: " + board.getNumRemaining());
				board.printBoard(false);
			}
		}

		private bool playGame()
		{
			Scanner scanner = new Scanner();
			printGameState();

			while (state == GameState.RUNNING)
			{
				String input = ""; //scanner.nextLine();
				if (input.Equals("exit"))
				{
					//scanner.close();
					return false;
				}

				UserPlay play = UserPlay.fromString(input);
				if (play == null)
				{
					continue;
				}

				UserPlayResult result = board.playFlip(play);
				if (result.successfulMove())
				{
					state = result.getResultingState();
				}
				else
				{
					Console.WriteLine("Could not flip cell (" + play.getRow() + "," + play.getColumn() + ").");
				}
				printGameState();
			}
			//scanner.close();
			return true;
		}
		

	}
	public class UserPlay
	{
		private int row;
		private int column;
		private bool isGuess;

		private UserPlay(int r, int c, bool guess)
		{
			setRow(r);
			setColumn(c);
			isGuess = guess;
		}

		public static UserPlay fromString(String input)
		{
			bool isGuess = false;

			if (input.Length > 0 && input[0] == 'B')
			{
				isGuess = true;
				input = input.Substring(1);
			}

			//if (!input.maMatches("\\d* \\d+"))
			//{
			//	return null;
			//}

			String[] parts = input.Split(' ');
			try
			{
				int r = int.Parse(parts[0]);
				int c = int.Parse(parts[1]);
				return new UserPlay(r, c, isGuess);
			}
			catch (NumberFormatException e)
			{
				return null;
			}
		}

		public bool IsGuess()
		{
			return isGuess;
		}

		public bool isMove()
		{
			return !isMove();
		}

		public int getColumn()
		{
			return column;
		}

		public void setColumn(int column)
		{
			this.column = column;
		}

		public int getRow()
		{
			return row;
		}

		public void setRow(int row)
		{
			this.row = row;
		}
	}


	internal class Scanner
    {
        

        public Scanner()
        {
        }
    }
	public class UserPlayResult
	{
		private bool successful;
		private GameState resultingState;
		public UserPlayResult(bool success, GameState state)
		{
			successful = success;
			resultingState = state;
		}

		public bool successfulMove()
		{
			return successful;
		}

		public GameState getResultingState()
		{
			return resultingState;
		}
	}

}
