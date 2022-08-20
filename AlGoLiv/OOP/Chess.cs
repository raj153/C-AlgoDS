using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.OOP
{
	public class Chess
	{
		ChessBoard chessBoard;
		Player[] player;
		Player currentPlayer;
		List<Move> movesList;
		GameStatus gameStatus;
		public bool playerMove(CellPosition fromPosition, CellPosition toPosition, Piece piece) { return true; }
		public bool endGame() { return true; }
		private void changeTurn() { }
	}
	public class Player
	{
		Account account;
		Color color;
		Time timeLeft;
	}
	public class Time
	{
		int mins;
		int secs;
	}
	public enum Color
	{
		BLACK, WHITE

	}
	public class Account
	{
		String username;
		String password;
		String name;
		String email;
		String phone;
	}
	public enum GameStatus
	{
		ACTIVE, PAUSED, FORTFEIGHT, BLACK_WIN, WHITE_WIN
	}
	public class ChessBoard
	{
		List<List<Cell>> board;
		public void resetBoard() { }
		public void updateBoard(Move move) { }
	}
	public class Cell
	{
		Color color;
		Piece piece;
		CellPosition position;
	}
	public class CellPosition
	{
		Char ch;
		int i;
	}
	public class Move
	{
		Player turn;
		Piece piece;
		Piece killedPiece;
		CellPosition startPosition;
		CellPosition endPosition;
	}
	public abstract class Piece
	{
		Color color;
		public abstract bool Move(CellPosition fromPosition, CellPosition toPosition);
		public abstract List<CellPosition> PossibleMoves(CellPosition fromPosition);
		public abstract bool Validate(CellPosition fromPosition, CellPosition toPosition);
	}
	public class Knight : Piece
	{
		public override bool Move(CellPosition fromPosition, CellPosition toPosition) { return true; }
		public override List<CellPosition> PossibleMoves(CellPosition fromPosition) { return new List<CellPosition>(); }
		public override bool Validate(CellPosition fromPosition, CellPosition toPosition) { return true; }
	}
public class Bishop : Piece
{
		public override bool Move(CellPosition fromPosition, CellPosition toPosition) { return true; }
		public override List<CellPosition> PossibleMoves(CellPosition fromPosition) { return new List<CellPosition>(); }
		public override bool Validate(CellPosition fromPosition, CellPosition toPosition) { return true; }

}
	public class Rook : Piece
	{


		public override bool Move(CellPosition fromPosition, CellPosition toPosition) { return true; }
		public override List<CellPosition> PossibleMoves(CellPosition fromPosition) { return new List<CellPosition>(); }
		public override bool Validate(CellPosition fromPosition, CellPosition toPosition) { return true; }
	}
	public class King : Piece
{


		public override bool Move(CellPosition fromPosition, CellPosition toPosition) { return true; }
		public override List<CellPosition> PossibleMoves(CellPosition fromPosition) { return new List<CellPosition>(); }
		public override bool Validate(CellPosition fromPosition, CellPosition toPosition) { return true; }

	}
	public class Queen : Piece
{


		public override bool Move(CellPosition fromPosition, CellPosition toPosition) { return true; }
		public override List<CellPosition> PossibleMoves(CellPosition fromPosition) { return new List<CellPosition>(); }
		public override bool Validate(CellPosition fromPosition, CellPosition toPosition) { return true; }

	}
	public class Pawn : Piece
{


		public override bool Move(CellPosition fromPosition, CellPosition toPosition) { return true; }
		public override List<CellPosition> PossibleMoves(CellPosition fromPosition) { return new List<CellPosition>(); }
		public override bool Validate(CellPosition fromPosition, CellPosition toPosition) { return true; }

	}

}
