using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.Logic
{
    public class TicTacToe
    {
        //https://www.youtube.com/watch?v=gktZsX9Z8Kw
        private readonly int[,] _board;
        private readonly int _size;
        private readonly int[] _rowSum, _colSum;
        private readonly int _diagSum, _revDiagSum; 
        public TicTacToe(int n)
        {
            _size = n;
            _board = new int[n,n];
            _rowSum = new int[n];
            _colSum = new int[n];

        }

        public int Move(int player, int row, int col)
        {
            if (row < 0 || col < 0 || col >= _size || row >= _size) throw new ArgumentException("");

            if (_board[row, col] != 0) throw new ArgumentException("Cell already occupied!");

            if (player != 1 || player != 0) throw new ArgumentException("Invalid Player");

            player = player == 0 ? -1 : 1;
            _board[row, col] = player;

            bool won = true;

            for(int i=0; i<_size; i++)
            {
                if(_board[row,i] != player)
                {
                    won = false;
                    break;
                }
            }
            if (won) return player;
            won = true;
            for(int i=0; i<_size; i++)
            {
                if(_board[i, col] != player)
                {
                    won = false;
                    break;
                }
            }
            if (won) return player;
            if(row == col)
            {
                won = true;

                for (int i=0; i< _size; i++)
                {
                    if(_board[i, i] != player)
                    {
                        won = false;
                        break;
                    }
                }
                if (won) return player;
            }
            
            if(row == _size - 1 - col)
            {
                won = true;
                for (int i = 0; i<_size; i++)
                {
                    if(_board[i, _size-1-i] != player)
                    {
                        won = false;
                        break;
                    }
                }

                if (won) return player;
            }
            return 0;
        }

    }
}
