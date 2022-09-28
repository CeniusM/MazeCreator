﻿// biggest problem
// the branches basicly comes from the end, two ways of fixing, just run the algorithm backwards/change start and end
// or make the path and go through the start to the end



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeCreator;

internal struct Coord
{
    public int x, y;

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

internal class MazeGenerator
{
    private bool done = false;
    public bool Done { get { return done; } }

    public int startX;
    public int startY;
    public int endX;
    public int endY;

    // for generation
    public bool[,] blockHasInit;
    public Maze maze;
    public int step;
    public int stepsNeeded;

    private Stack<Coord> trail;

    private bool FindingWinningTrail = true;
    private bool UsingWinningTrail = false;
    private bool UsingTrailStack = false;

    private int currentX;
    private int currentY;

    public MazeGenerator(int width, int height)
    {
        blockHasInit = new bool[width, height];
        maze = new Maze(width, height);
        step = 0;
        stepsNeeded = width * height - 1;
        trail = new(width * height);

        // currently starts in top right and ends in buttom left
        startX = 0;
        startY = 0;
        endX = width - 1;
        endY = height - 1;
        currentX = startX;
        currentY = startY;
    }

    public void StepForward(int steps = 1)
    {
        Random rand = new Random();
        if (!maze.InBound(currentX, currentY))
            return;
        if (done)
            return;

        for (int i = 0; i < steps; i++)
        {
            // generation code
            //maze.maze[currentX, currentY] = new Maze.Block();
            blockHasInit[currentX, currentY] = true;

            int x;
            int y;

            // crap way of checking all the ways
            List<Coord> coordsTested = new List<Coord>(4);
            while (true)
            {
                do
                {
                    x = rand.Next(-1, 2);
                    y = rand.Next(-1, 2);
                } while (
                // remove 0,0
                (x == 0 && y == 0) ||

                // remove diagonel by making sure only one is not set to 0
                !((x == 0 && y != 0) || (y == 0 && x != 0))
                );

                if (!coordsTested.Contains(new Coord(x, y)))
                    coordsTested.Add(new(x, y));
                else if (coordsTested.Count() != 4)
                    continue;

                if (maze.InBound(currentX + x, currentY + y))
                {
                    if (!blockHasInit[currentX + x, currentY + y])
                    {
                        trail.Push(new Coord(currentX, currentY));

                        // make the block
                        Maze.Direction dir = Maze.Direction.GetDirFromOffset(x, y);

                        maze.Set(currentX, currentY, dir, true);

                        dir.Reverse();
                        currentX += x;
                        currentY += y;

                        maze.Set(currentX, currentY, dir, true);

                        // this makes it so when we have reached the end, we will start from the start
                        // and make branches from there instead of from the end
                        if (currentX == endX && currentY == endY)
                        {
                            trail.Reverse();
                            currentX = startX;
                            currentY = startY;
                            step++;
                            blockHasInit[endX, endY] = true;
                        }

                        step++;
                        if (step == stepsNeeded)
                        {
                            done = true;
                            blockHasInit[currentX, currentY] = true;
                            return;
                        }
                        break;
                    }
                }
                else
                {
                    // make a wall becous its out goes outside the maze
                    // unless its the exit or the entrenss
                }

                if (coordsTested.Count == 4)
                {
                    Coord coord = trail.Pop();
                    currentX = coord.x;
                    currentY = coord.y;
                    break;
                }
            }
        }
    }
}