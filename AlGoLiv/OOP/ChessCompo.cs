using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.OOP1
{
    public class Board
    {
        int boardSize;
        Cell[][] cells;

        public Board(int boardSize, Cell[][] cells)
        {
            this.boardSize = boardSize;
            this.cells = cells;
        }

        /**
         * Helper method to return cell to the left of current cell.
         */
        public Cell getLeftCell(Cell cell)
        {
            return getCellAtLocation(cell.x, cell.y - 1);
        }

        /**
         * Helper method to return cell to the right of current cell.
         */
        public Cell getRightCell(Cell cell)
        {
            return getCellAtLocation(cell.x, cell.y + 1);
        }

        /**
         * Helper method to return cell to the up of current cell.
         */
        public Cell getUpCell(Cell cell)
        {
            return getCellAtLocation(cell.x + 1, cell.y);
        }

        /**
         * Helper method to return cell to the down of current cell.
         */
        public Cell getDownCell(Cell cell)
        {
            return getCellAtLocation(cell.x - 1, cell.y);
        }

        /**
         * Helper method to return cell at a given location.
         */
        public Cell getCellAtLocation(int x, int y)
        {
            if (x >= boardSize || x < 0)
            {
                return null;
            }
            if (y >= boardSize || y < 0)
            {
                return null;
            }

            return cells[x][y];
        }



        /**
      * Helper method to determine whether the player is on check on the current board.
      */
        public bool isPlayerOnCheck(Player player)
        {
            return checkIfPieceCanBeKilled(player.getPiece(PieceType.KING), kingCheckEvaluationBlockers(), player);
        }

        private List<PieceCellOccupyBlocker> kingCheckEvaluationBlockers()
        {
            throw new NotImplementedException();
        }

        /**
         * Method to check if the piece can be killed currently by the opponent as per the current board configuration.
         *
         * @param targetPiece        Piece which is to be checked.
         * @param cellOccupyBlockers Blockers which make cell non occupiable.
         * @param player             Player whose piece has to be checked.
         * @return bool indicating whether the piece is in danger or not.
         */
        public bool checkIfPieceCanBeKilled(Piece targetPiece, List<PieceCellOccupyBlocker> cellOccupyBlockers, Player player)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Piece currentPiece = getCellAtLocation(i, j).currentPiece;
                    if (currentPiece != null && !currentPiece.isPieceFromSamePlayer(targetPiece))
                    {
                        List<Cell> nextPossibleCells = currentPiece.nextPossibleCells(this, cellOccupyBlockers, player);
                        if (nextPossibleCells.Contains(targetPiece.currentCell))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    public class Cell
    {

        public int x;
        public int y;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }


        public Piece currentPiece;

        public bool isFree()
        {
            return currentPiece == null;
        }
    }
    public enum Color
    {
        BLACK,
        WHITE
    }
    public class Piece
    {
        private bool isKilled = false;
        private Color _color;
        private  List<PossibleMovesProvider> movesProviders;
        private int numMoves = 0;
        public PieceType pieceType;

        
        
    public Cell currentCell;

        public Piece(Color color,  List<PossibleMovesProvider> movesProviders,  PieceType pieceType)
        {
            this._color = color;
            this.movesProviders = movesProviders;
            this.pieceType = pieceType;
        }

        public void killIt()
        {
            this.isKilled = true;
        }

        /**
         * Method to move piece from current cell to a given cell.
         */
        public void move(Player player, Cell toCell, Board board, List<PieceCellOccupyBlocker> additionalBlockers)
        {
            if (isKilled)
            {
                throw new InvalidMoveException();
            }
            List<Cell> nextPossibleCells1 = nextPossibleCells(board, additionalBlockers, player);
            if (!nextPossibleCells1.Contains(toCell))
            {
                throw new InvalidMoveException();
            }

            killPieceInCell(toCell);
            this.currentCell.currentPiece = null;
            this.currentCell = toCell;
            this.currentCell.currentPiece = this;
            this.numMoves++;
        }

        /**
         * Helper method to kill a piece in a given cell.
         */
        private void killPieceInCell(Cell targetCell)
        {
            if (targetCell.currentPiece != null)
            {
                targetCell.currentPiece.killIt();
            }
        }

        /**
         * Method which tells what are all next possible cells to which the current piece can move from the current cell.
         *
         * @param board              Board on which the piece is present.
         * @param additionalBlockers Blockers which make a cell non-occupiable for a piece.
         * @param player             Player who owns the piece.
         * @return List of all next possible cells.
         */
        public List<Cell> nextPossibleCells(Board board, List<PieceCellOccupyBlocker> additionalBlockers, Player player)
        {
            List<Cell> result = new List<Cell>();
            foreach (PossibleMovesProvider movesProvider in movesProviders)
            {
                List<Cell> cells = movesProvider.possibleMoves(this, board, additionalBlockers, player);
                if (cells != null)
                {
                    result.AddRange(cells);
                }
            }
            return removeDuplicates(result);
        }

        private List<Cell> removeDuplicates(List<Cell> result)
        {
            throw new NotImplementedException();
        }

        /**
         * Helper method to check if two pieces belong to same player.
         */
        public bool isPieceFromSamePlayer(Piece piece)
        {
            return piece._color.Equals(this._color);
        }
    }

    public class PieceCellOccupyBlocker
    {
    }

    public enum PieceType
    {
        KING,
        QUEEN,
        ROOK,
        KNIGHT,
        BISHOP,
        PAWN
    }



    public abstract class Player
    {
        List<Piece> pieces;

        public Player(List<Piece> pieces)
        {
            this.pieces = pieces;
        }

        public Piece getPiece(PieceType pieceType)
        {
            foreach (Piece piece in pieces)
            {
                if (piece.pieceType == pieceType)
                {
                    return piece;
                }
            }
            throw new PieceNotFoundException();
        }

        
    }

    /**
 * Given a cell, it will provide next cell which we can reach to.
 */
    public interface NextCellProvider
    {
        Cell nextCell(Cell cell);
    }

    /**
 * Provider class which returns all the possible cells for a given type of moves. For example, horizontal type of move
 * will give all the cells which can be reached by making only horizontal moves.
 */
    public abstract class PossibleMovesProvider
    {
        int maxSteps;
        MoveBaseCondition baseCondition;
       private PieceMoveFurtherCondition MoveFurtherCondition;
        private PieceCellOccupyBlocker baseBlocker;

        public PossibleMovesProvider(int maxSteps, MoveBaseCondition baseCondition, PieceMoveFurtherCondition moveFurtherCondition, PieceCellOccupyBlocker baseBlocker)
        {
            this.maxSteps = maxSteps;
            this.baseCondition = baseCondition;
            this.MoveFurtherCondition = moveFurtherCondition;
            this.baseBlocker = baseBlocker;
        }

        /**
         * Public method which actually gives all possible cells which can be reached via current type of move.
         */
        public List<Cell> possibleMoves(Piece piece, Board inBoard, List<PieceCellOccupyBlocker> additionalBlockers, Player player)
        {
            //if (baseCondition.isBaseConditionFullfilled(piece))
            //{
            //    return possibleMovesAsPerCurrentType(piece, inBoard, additionalBlockers, player);
            //}
            return null;
        }

        /**
         * Abstract method which needs to be implemented by each type of move to give possible moves as per their behaviour.
         */
        protected abstract List<Cell> possibleMovesAsPerCurrentType(Piece piece, Board board, List<PieceCellOccupyBlocker> additionalBlockers, Player player);

        /**
         * Helper method used by all the sub types to create the list of cells which can be reached.
         */
        protected List<Cell> findAllNextMoves(Piece piece, NextCellProvider nextCellProvider, Board board, List<PieceCellOccupyBlocker> cellOccupyBlockers, Player player)
        {
            List<Cell> result = new List<Cell>();
            //Cell nextCell = nextCellProvider.nextCell(piece.getCurrentCell());
            //int numSteps = 1;
            //while (nextCell != null && numSteps <= maxSteps)
            //{
            //    if (checkIfCellCanBeOccupied(piece, nextCell, board, cellOccupyBlockers, player))
            //    {
            //        result.add(nextCell);
            //    }
            //    if (!moveFurtherCondition.canPieceMoveFurtherFromCell(piece, nextCell, board))
            //    {
            //        break;
            //    }

            //    nextCell = nextCellProvider.nextCell(nextCell);
            //    numSteps++;
            //}
            return result;
        }

        /**
         * Helper method which checks if a given cell can be occupied by the piece or not. It makes use of list of
         * {@link PieceCellOccupyBlocker}s passed to it while checking. Also each move has one base blocker which it should
         * also check.
         */
        private bool checkIfCellCanBeOccupied(Piece piece, Cell cell, Board board, List<PieceCellOccupyBlocker> additionalBlockers, Player player)
        {
            //if (baseBlocker != null && baseBlocker.isCellNonOccupiableForPiece(cell, piece, board, player))
            //{
            //    return false;
            //}
            //for (PieceCellOccupyBlocker cellOccupyBlocker : additionalBlockers)
            //{
            //    if (cellOccupyBlocker.isCellNonOccupiableForPiece(cell, piece, board, player))
            //    {
            //        return false;
            //    }
            //}
            return true;
        }
    }

    public  class PieceMoveFurtherCondition
    {
    }

    public class PossibleMovesProviderDiagonal : PossibleMovesProvider
    {


    public PossibleMovesProviderDiagonal(int maxSteps, MoveBaseCondition baseCondition,
                                         PieceMoveFurtherCondition moveFurtherCondition, PieceCellOccupyBlocker baseBlocker)
            : base(maxSteps, baseCondition, moveFurtherCondition, baseBlocker)
    {
        
    }

    protected override List<Cell> possibleMovesAsPerCurrentType(Piece piece, Board board, List<PieceCellOccupyBlocker> additionalBlockers, Player player)
    {
        return null;
    }
}

public class PossibleMovesProviderHorizontal : PossibleMovesProvider
{

    public PossibleMovesProviderHorizontal(int maxSteps, MoveBaseCondition baseCondition,
                                           PieceMoveFurtherCondition moveFurtherCondition, PieceCellOccupyBlocker baseBlocker)
                        :base(maxSteps, baseCondition, moveFurtherCondition, baseBlocker)
{
    
}

protected override List<Cell> possibleMovesAsPerCurrentType(Piece piece,  Board board, List<PieceCellOccupyBlocker> additionalBlockers, Player player)
{
            List<Cell> result = new List<Cell>();
    //result.addAll(findAllNextMoves(piece, board::getLeftCell, board, additionalBlockers, player));
    //result.addAll(findAllNextMoves(piece, board::getRightCell, board, additionalBlockers, player));
    return result;
}
}

public class PossibleMovesProviderVertical : PossibleMovesProvider
{
    private VerticalMoveDirection verticalMoveDirection;

public PossibleMovesProviderVertical(int maxSteps, MoveBaseCondition baseCondition,
                                     PieceMoveFurtherCondition moveFurtherCondition, PieceCellOccupyBlocker baseBlocker,
                                     VerticalMoveDirection verticalMoveDirection) : base(maxSteps, baseCondition, moveFurtherCondition, baseBlocker)
{
    
    this.verticalMoveDirection = verticalMoveDirection;
}



    protected override List<Cell> possibleMovesAsPerCurrentType(Piece piece, Board board, List<PieceCellOccupyBlocker> additionalBlockers, Player player)
{
            List<Cell> result = new List<Cell>();
    //if (this.verticalMoveDirection == UP || this.verticalMoveDirection == BOTH)
    //{
    //    result.addAll(findAllNextMoves(piece, board::getUpCell, board, additionalBlockers, player));
    //}
    //if (this.verticalMoveDirection == DOWN || this.verticalMoveDirection == BOTH)
    //{
    //    result.addAll(findAllNextMoves(piece, board::getDownCell, board, additionalBlockers, player));
    //}
    return result;
}
}
public enum VerticalMoveDirection
{
    UP,
    DOWN,
    BOTH
}
public class PieceNotFoundException : Exception { }


public class InvalidMoveException : Exception
{
}
/**
 * It provides the base condition for a piece to make a move.The piece would only be allowed to move from its current
 * position if the condition fulfills.
 */
public interface MoveBaseCondition
{
    bool isBaseConditionFullfilled(Piece piece);
}

/**
 * This condition allows a move only if cell is making a move from its initial position. That is first move ever.
 */
public class MoveBaseConditionFirstMove : MoveBaseCondition
{

    public bool isBaseConditionFullfilled(Piece piece)
{
    return 0 == 0;
}
}





}