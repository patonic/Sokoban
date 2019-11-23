using System;

namespace Sokoban
{
    enum Direction { Up, Right, Down, Left }

    class Program
    {
        static char[,] str = {
                            {'v','v','W','W','W','W','v','v','v','v','v'},
                            {'v','v','W','v','v','W','v','v','v','v','v'},
                            {'v','v','W','B','v','W','W','W','v','v','v'},
                            {'v','v','W','v','v','v','v','W','v','v','v'},
                            {'W','W','W','v','W','W','v','W','W','W','W'},
                            {'W','v','v','B','v','v','B','v','B','v','W'},
                            {'W','v','W','v','W','W','v','v','v','v','W'},
                            {'W','v','W','v','W','W','B','B','v','v','W'},
                            {'W','v','W','v','v','v','v','W','W','W','W'},
                            {'W','v','v','v','W','W','W','W','v','v','v'},
                            {'W','W','W','v','W','v','v','v','v','v','v'},
                            {'v','W','P','v','W','v','v','v','v','v','v'},
                            {'v','W','W','v','W','v','v','v','v','v','v'},
                            {'v','W','W','v','W','W','v','v','v','v','v'},
                            {'v','W','v','v','v','W','v','v','v','v','v'},
                            {'v','W','v','v','v','W','v','v','v','v','v'},
                            {'v','W','M','M','M','W','v','v','v','v','v'},
                            {'v','W','M','M','M','W','v','v','v','v','v'},
                            {'v','W','W','W','W','W','v','v','v','v','v'}
                            };

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

        startGame:
            World world = new World(str);
            world.paint();
            bool win = false;
            for (; ; )
            {
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        win = world.move(Direction.Left);
                        break;
                    case ConsoleKey.RightArrow:
                        win = world.move(Direction.Up);
                        break;
                    case ConsoleKey.DownArrow:
                        win = world.move(Direction.Right);
                        break;
                    case ConsoleKey.LeftArrow:
                        win = world.move(Direction.Down);
                        break;
                    case ConsoleKey.Escape:
                        return;
                    case ConsoleKey.R:
                        goto startGame;
                }

                if (win)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("WINNER!");
                    Console.ReadKey();
                    goto startGame;
                }

                //world.paint(); Теперь перерисовка в функции движения
            }
        }
        public class World
        {
            bool[,] arrayWall;
            bool[,] arrayBox;
            bool[,] arrayMark;
            ItemXY player;
            public World(char[,] field)
            {
                arrayWall = new bool[field.GetLength(0), field.GetLength(1)];
                for (int x = 0; x < arrayWall.GetLength(0); x++)
                    for (int y = 0; y < arrayWall.GetLength(1); y++)
                        if (field[x, y] == 'W')
                            arrayWall[x, y] = true;
                        else
                            arrayWall[x, y] = false;

                arrayBox = new bool[field.GetLength(0), field.GetLength(1)];
                for (int x = 0; x < arrayBox.GetLength(0); x++)
                    for (int y = 0; y < arrayBox.GetLength(1); y++)
                        if (field[x, y] == 'B')
                            arrayBox[x, y] = true;
                        else
                            arrayBox[x, y] = false;

                for (int x = 0; x < field.GetLength(0); x++)
                    for (int y = 0; y < field.GetLength(1); y++)
                        if (field[x, y] == 'P')
                            player = new ItemXY(x, y);

                arrayMark = new bool[field.GetLength(0), field.GetLength(1)];
                for (int x = 0; x < arrayMark.GetLength(0); x++)
                    for (int y = 0; y < arrayMark.GetLength(1); y++)
                        if (field[x, y] == 'M')
                            arrayMark[x, y] = true;
                        else
                            arrayMark[x, y] = false;
            }

            public void paint()
            {
                Console.Clear();
                for (int y = 0; y < arrayMark.GetLength(1); y++)
                {
                    for (int x = 0; x < arrayMark.GetLength(0); x++)
                    {
                        paintXY(x, y);
                    }

                }

            }

            public void paintXY(int x, int y)
            {
                if (arrayWall[x, y])    //Wall
                {
                    Console.CursorLeft = y;
                    Console.CursorTop = x;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write('▓');
                }
                if (arrayMark[x, y])    //Mark
                {
                    Console.CursorLeft = y;
                    Console.CursorTop = x;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write('X');
                }
                if (arrayBox[x, y])    //Box
                {
                    Console.CursorLeft = y;
                    Console.CursorTop = x;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write('█');
                }
                if (arrayMark[x, y] && arrayBox[x, y])    //BoxAndMark
                {
                    Console.CursorLeft = y;
                    Console.CursorTop = x;
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write('█');
                }
                if (player.x == x && player.y == y)    //Player
                {
                    Console.CursorLeft = y;
                    Console.CursorTop = x;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write('☻');
                }
                if (!arrayWall[x, y] && !arrayMark[x, y] && !arrayBox[x, y] && !(player.x == x && player.y == y))
                {
                    Console.CursorLeft = y;
                    Console.CursorTop = x;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(' ');
                }
                Console.CursorLeft = 0;
                Console.CursorTop = arrayWall.GetLength(0);
            }

            public bool move(Direction dir)
            {
                ItemXY newPositionRelative;
                switch (dir)
                {
                    case Direction.Up:
                        newPositionRelative = new ItemXY(0, 1);
                        break;
                    case Direction.Right:
                        newPositionRelative = new ItemXY(1, 0);
                        break;
                    case Direction.Down:
                        newPositionRelative = new ItemXY(0, -1);
                        break;
                    case Direction.Left:
                        newPositionRelative = new ItemXY(-1, 0);
                        break;
                    default:
                        newPositionRelative = new ItemXY(0, 0);
                        break;
                }

                if (!arrayBox[player.x + newPositionRelative.x, player.y + newPositionRelative.y] && !arrayWall[player.x + newPositionRelative.x, player.y + newPositionRelative.y])
                {
                    player.x += newPositionRelative.x;
                    player.y += newPositionRelative.y;
                    paintXY(player.x - newPositionRelative.x, player.y - newPositionRelative.y);    //Старое местоположение игрока
                    paintXY(player.x, player.y);    //Новое местоположение игрока
                }
                else
                if (arrayBox[player.x + newPositionRelative.x, player.y + newPositionRelative.y] &&
                    !(arrayBox[player.x + newPositionRelative.x * 2, player.y + newPositionRelative.y * 2] ||
                      arrayWall[player.x + newPositionRelative.x * 2, player.y + newPositionRelative.y * 2]))
                {
                    arrayBox[player.x + newPositionRelative.x, player.y + newPositionRelative.y] = false;
                    arrayBox[player.x + newPositionRelative.x * 2, player.y + newPositionRelative.y * 2] = true;
                    player.x += newPositionRelative.x;
                    player.y += newPositionRelative.y;
                    paintXY(player.x - newPositionRelative.x, player.y - newPositionRelative.y);    //Старое местоположение игрока
                    paintXY(player.x, player.y);    //Новое местоположение игрока
                    paintXY(player.x + newPositionRelative.x, player.y + newPositionRelative.y);    //Новое местоположение ящика
                }

                bool win = true;
                for (int y = 0; y < arrayMark.GetLength(1); y++)
                    for (int x = 0; x < arrayMark.GetLength(0); x++)
                        if (arrayMark[x, y] && !arrayBox[x, y])
                            win = false;
                return win;
            }

            class ItemXY
            {
                public int x;
                public int y;
                public ItemXY(int x, int y)
                {
                    this.x = x;
                    this.y = y;
                }
            }
        }

    }
}