using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.OOP7
{
    public class Player
    {
        public String playerName;
        public int playerId;
        public String address;
        public int ranking;
        public char playerSymbol;
    }

    public class GameBoard
    {
        char[][] board;
        Scanner input;
        Queue<Player> nextTurn;
        bool gameOver;
        int count = 0;
        public GameBoard(Player[] players)
        {
            board = new char[][] { };
            //board = new char[][]{
        //{' ','|',' ','|',' '},
        //{'-','|','-','|','-'},
        //{' ','|',' ','|',' '},
        //{'-','|','-','|','-'},
        //{' ','|',' ','|',' '}
        //};
           // input = new Scanner();
            nextTurn = new Queue<Player>();
            nextTurn.Enqueue(players[0]);
            nextTurn.Enqueue(players[1]);
            gameOver = false;
        }

        public void start()
        {
            printBoard(board);
            while (!gameOver)
            {
                count++;
                if (count == 10)
                {
                    Console.WriteLine("Match draw");
                    break;
                }
                Player p = nextTurn.Dequeue();
                int val = nextMove();
                board[val / 10][val % 10] = p.playerSymbol;
                if (checkStatus(p))
                {
                    gameOver = true;
                    Console.WriteLine(p.playerName + " has won the game");
                }
                printBoard(board);
                nextTurn.Enqueue(p);
            }

        }

        public void printBoard(char[][] board)
        {
            //foreach (char[] row in board)
            //{
            //    for (char col: row)
            //    {
            //        System.out.print(col);
            //    }
            //    Console.WriteLine();
            //}
        }

        public bool checkStatus(Player p)
        {
            if (board[0][0] + board[0][2] + board[0][4] == p.playerSymbol + p.playerSymbol + p.playerSymbol) return true;
            if (board[2][0] + board[2][2] + board[2][4] == p.playerSymbol + p.playerSymbol + p.playerSymbol) return true;
            if (board[4][0] + board[4][2] + board[4][4] == p.playerSymbol + p.playerSymbol + p.playerSymbol) return true;
            if (board[0][0] + board[2][0] + board[4][0] == p.playerSymbol + p.playerSymbol + p.playerSymbol) return true;
            if (board[0][2] + board[2][2] + board[4][2] == p.playerSymbol + p.playerSymbol + p.playerSymbol) return true;
            if (board[0][4] + board[2][4] + board[4][4] == p.playerSymbol + p.playerSymbol + p.playerSymbol) return true;
            if (board[0][0] + board[2][2] + board[4][4] == p.playerSymbol + p.playerSymbol + p.playerSymbol) return true;
            if (board[0][4] + board[2][2] + board[4][0] == p.playerSymbol + p.playerSymbol + p.playerSymbol) return true;

            return false;
        }


        public int nextMove()
        {
            Console.WriteLine("Enter a number from 1-9");
            int position = input.nextInt();
            while (!validPosition(position))
            {
                Console.WriteLine("Wrong Position, try different position.Enter a number from 1-9");
                position = input.nextInt();
            }
            return getCoordinates(position);
        }

        public bool validPosition(int pos)
        {
            if (pos < 1 || pos > 9) return false;
            int val = getCoordinates(pos);
            if (board[val / 10][val % 10] == 'X' || board[val / 10][val % 10] == 'O') return false;
            return true;
        }

        public int getCoordinates(int pos)
        {
            switch (pos)
            {
                case 1:
                    return 0;
                case 2:
                    return 2;
                case 3:
                    return 4;
                case 4:
                    return 20;
                case 5:
                    return 22;
                case 6:
                    return 24;
                case 7:
                    return 40;
                case 8:
                    return 42;
                case 9:
                    return 44;
            }
            return -1;
        }
    }

    internal class Scanner
    {
        internal int nextInt()
        {
            throw new NotImplementedException();
        }
    }
}
