
// Mika Spoelstra 500826255 IG-101
// GPA-tentamenopdracht Canabalt

using System;
using System.Collections.Generic;
using System.Threading;
/* not working...
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
*/

namespace GPA_tentamenopdracht
{
    static class Globals
    {
        public static int lives = 3;
        public static bool fall;

    }
    class Program
    {
        struct Player
        {
            public int x;
            public int y;
            public char symbol;
            public ConsoleColor color;
        }

        struct tu // tube unit
        {
            public int x;
            public int y;
        }

        static void Print(int x, int y, char symbol, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(symbol);
        }

        static void PrintCharcter(int x, int y, char symbol, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(symbol);
        }

        static void PrintString(int x, int y, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);
        }

        static void PrintTube(int x, int y, int num1, string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            for (int i = 0; i < num1 + 1; i++)
            {
                Console.SetCursorPosition(x + 1, y);
                Console.Write(str);
            }
        }

        static void Main(string[] args)
        {
            //intialize game
            Console.CursorVisible = false;
            Console.Title = "Runner";
            const int WINDOW_HEIGHT = 30;
            const int WINDOW_WIDTH = 70;
            Console.BackgroundColor = ConsoleColor.Black;
            //unfortunately I did not managed to get a working background image, this code below used to work in previous assignments...
            //Texture2d background;
            //...
            //background = Content.Load<Texture2D>("cityBackground");

            bool isGamePlay = true;
            
            List<tu> TubeList = new List<tu>();
            Random randomGenerator = new Random();
            
            int score = 0;
            int t = 0;
            tu sTube = new tu();
            sTube.x = 0;
            sTube.y = 27;
            int l = WINDOW_WIDTH - 20;

            //initialize scene
            Console.BufferHeight = Console.WindowHeight = WINDOW_HEIGHT;
            Console.BufferWidth = Console.WindowWidth = WINDOW_WIDTH;


            Console.Clear();

            //initialize Player
            Player runner;
            runner.x = 15;
            runner.y = 24;
            runner.symbol = 'R';
            runner.color = ConsoleColor.Yellow;

            //game loop
            while (isGamePlay)
            {
                // Update logic
                if (t % 20 == 0)
                {
                    tu newTube = new tu();
                    newTube.x = WINDOW_WIDTH - 10;
                    newTube.y = randomGenerator.Next(24, 27);

                    TubeList.Add(newTube);
                }

                if (runner.y < WINDOW_HEIGHT - 1)
                {
                    Globals.fall = true;
                    if (TubeList.Count >= 2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (runner.y + 1 == TubeList[i].y && runner.x + 1 >= TubeList[i].x && runner.x + 1 <= TubeList[i].x + 10)
                            {
                                Globals.fall = false;// player is on platform
                                score = score + 1;
                            }
                        }
                    }

                    if (Globals.fall)
                    {
                        runner.y += 1;
                    }
                }

                //damage, gameover & restart
                else
                {
                    Globals.lives = Globals.lives - 1;
                    runner.x = 15;
                    runner.y = 10;

                    if (Globals.lives == 0)
                    {
                        PrintString(30, 12, "*Game Over*", ConsoleColor.Red);
                        PrintString(24, 14, "Press any key to restart", ConsoleColor.Yellow);
                        score = 0;
                        Globals.lives = 3;
                        runner.x = 15;
                        runner.y = 10;
                        Console.ReadKey();

                    }
                }

                if (l > 10)
                {
                    if (runner.y == sTube.y) 
                    { 
                        runner.y = sTube.y - 1; 
                    }
                }

                //spacebar to jump
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyPressed = Console.ReadKey(true);

                    while (Console.KeyAvailable) { Console.ReadKey(true); }

                    if (keyPressed.Key == ConsoleKey.Spacebar)
                    { 
                        if (runner.y > 0 || runner.y < -5)
                        {
                            runner.y = runner.y - 5;
                            runner.x = runner.x + 1;
                        }

                        if (runner.y >= WINDOW_HEIGHT /2)
                        {
                            runner.y = WINDOW_HEIGHT /2;
                        }

                        if (runner.y <= WINDOW_HEIGHT /2)
                        {
                            runner.y = WINDOW_HEIGHT /2;
                        }
                    }
                }
                
               
                //Update Tube
                List<tu> newList = new List<tu>();
                for (int i = 0; i < TubeList.Count; i++)
                {
                    tu oldTube = TubeList[i];
                    tu newTube = new tu();
                    newTube.x = oldTube.x - 1;
                    newTube.y = oldTube.y;

                    if (newTube.x >= 1)
                    {
                        newList.Add(newTube);
                    }
                }

                TubeList = newList;
                t++;
                l--;

                //draw runner
                Console.Clear();
                PrintCharcter(runner.x, runner.y, runner.symbol, runner.color);
               

                //display text
                PrintString(2, 1, "Lives: " + Globals.lives, ConsoleColor.Yellow);
                PrintString(WINDOW_WIDTH / 2 - 5, 1, "Score: " + score, ConsoleColor.Yellow);
                PrintString(47, 1, "Press spacebar to jump", ConsoleColor.Yellow);

                //draw white dotted line under scores
                for (int i = 1; i < WINDOW_WIDTH - 1; i++)
                {
                    Print(i, 3, '-', ConsoleColor.Red);
                }

                for (int i = 1; i < WINDOW_WIDTH - 1; i++)
                {
                    Print(i, 29, '^', ConsoleColor.Red);
                }


                //draw tube platforms
                for (int z = 0; z < TubeList.Count; z++)
                {
                    PrintTube(TubeList[z].x, TubeList[z].y, 10, "::::::::::", ConsoleColor.Red);
                    PrintTube(TubeList[z].x, TubeList[z].y + 1, 10, "||||||||||", ConsoleColor.Red);
                    PrintTube(TubeList[z].x, TubeList[z].y + 2, 10, "||||||||||", ConsoleColor.Red);
                    PrintTube(TubeList[z].x, TubeList[z].y + 3, 10, "::::::::::", ConsoleColor.Red);
                }


                Thread.Sleep(20); //game speed delay 


            }
        }
    }
}