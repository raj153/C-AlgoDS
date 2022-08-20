using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.OOP5
{
    //https://github.com/TheTechGranth/thegranths/tree/master/src/main/java/SystemDesign/SnakeAndLadder
    //https://www.youtube.com/watch?v=zRz1GPSH50I&t=266s
    public class Dice
    {
        private int numberOfDice;

        public Dice(int numberOfDice)
        {
            this.numberOfDice = numberOfDice;
        }

       public  int rollDice()
        {
            return ((int)(new Random().Next(1,10) * (6 * numberOfDice - 1 * numberOfDice))) + 1;
        }
    }

    class GameBoard
    {
        private Dice dice;
        private Queue<Player> nextTurn;
        private List<Jumper> snakes;
        private List<Jumper> ladders;
        private Dictionary<String, int> playersCurrentPosition;
        int boardSize;

        public GameBoard(Dice dice, Queue<Player> nextTurn, List<Jumper> snakes, List<Jumper> ladders, Dictionary<String, int> playersCurrentPosition, int boardSize)
        {
            this.dice = dice;
            this.nextTurn = nextTurn;
            this.snakes = snakes;
            this.ladders = ladders;
            this.playersCurrentPosition = playersCurrentPosition;
            this.boardSize = boardSize;
        }

        public void startGame()
        {
            while (nextTurn.Count() > 1)
            {
                Player player = nextTurn.Peek();
                int currentPosition = playersCurrentPosition[player.playerName];
                int diceValue = dice.rollDice();
                int nextCell = currentPosition + diceValue;
                if (nextCell > boardSize) nextTurn.Enqueue(player);
                else if (nextCell == boardSize)
                {
                    Console.WriteLine(player.playerName + " won the game");
                }
                else
                {
                    int[] nextPosition = new int[1];
                    bool[] b = new bool[1];
                    nextPosition[0] = nextCell;
                    snakes.ForEach(v => {
                        if (v.startPoint == nextCell)
                        {
                            nextPosition[0] = v.endPoint;
                        }
                    } );
                    if (nextPosition[0] != nextCell) Console.WriteLine(player.playerName + " Bitten by Snake present at: " + nextCell);
                    ladders.ForEach(v=> {
                        if (v.startPoint == nextCell)
                        {
                            nextPosition[0] = v.endPoint;
                            b[0] = true;
                        }
                    } );
                    if (nextPosition[0] != nextCell && b[0]) Console.WriteLine(player.playerName + " Got ladder present at: " + nextCell);
                    if (nextPosition[0] == boardSize)
                    {
                        Console.WriteLine(player.playerName + " won the game");
                    }
                    else
                    {
                        playersCurrentPosition[player.playerName]= nextPosition[0];
                        Console.WriteLine(player.playerName + " is at position " + nextPosition[0]);
                        nextTurn.Enqueue(player);
                    }
                }
            }
        }
    }

    public class Jumper
    {
        public int startPoint;
        public int endPoint;

        public Jumper(int startPoint, int endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }
    }

    public class PlaySnakeAndLadder
    {
        public static void main(String[] args)
        {
            Dice dice = new Dice(1);
            Player p1 = new Player("Alberts", 1);
            Player p2 = new Player("Pintoss", 2);
            Queue<Player> allPlayers = new Queue<Player>();
            allPlayers.Enqueue(p1);
            allPlayers.Enqueue(p2);
            Jumper snake1 = new Jumper(10, 2);
            Jumper snake2 = new Jumper(99, 12);
            List<Jumper> snakes = new List<Jumper>();
            snakes.Add(snake1);
            snakes.Add(snake2);
            Jumper ladder1 = new Jumper(5, 25);
            Jumper ladder2 = new Jumper(40, 89);
            List<Jumper> ladders = new List<Jumper>();
            ladders.Add(ladder1);
            ladders.Add(ladder2);
            Dictionary<String, int> playersCurrentPosition = new Dictionary<string, int>();
            playersCurrentPosition.Add("Alberts", 0);
            playersCurrentPosition.Add("Pintoss", 0);
            GameBoard gb = new GameBoard(dice, allPlayers, snakes, ladders, playersCurrentPosition, 100);
            gb.startGame();
        }
    }
    public class Player
    {
        public String playerName;
        public int id;

       public Player(String playerName, int id)
        {
            this.playerName = playerName;
            this.id = id;
        }
    }


}
