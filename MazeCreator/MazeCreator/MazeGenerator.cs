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

    private int startX;
    private int startY;
    private int endX;
    private int endY;

    // for generation
    public bool[,] blockHasInit;
    public Maze maze;
    public int step;
    public int stepsNeeded;

    private Stack<Coord> trail;
    private List<Coord> winningTrail;

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
        winningTrail = new(width + height);

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
                        if (FindingWinningTrail)
                            winningTrail.Add(new Coord(currentX, currentY));

                        // make the block
                        Maze.Direction dir = Maze.Direction.GetDirFromOffset(x, y);

                        maze.Set(currentX, currentY, dir, true);

                        dir.Reverse();
                        currentX += x;
                        currentY += y;

                        maze.Set(currentX, currentY, dir, true);

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
                    // instead of Just going back, it will slowly go through the winning trail so there is more paths in the begining
                    // when you have gone through the whole trail, and its not done, then use the trail stack

                    if (FindingWinningTrail)
                    {
                        Coord coord = trail.Pop();
                        currentX = coord.x;
                        currentY = coord.y;
                        break;
                    }
                    else if (UsingWinningTrail)
                    {

                    }
                    else if (UsingTrailStack)
                    {

                    }
                    else
                        throw new NotImplementedException();

                    //Coord coord = trail.Pop();
                    //currentX = coord.x;
                    //currentY = coord.y;
                    //break;
                }
            }
        }
    }
}

// debuging
//maze.maze[currentX, currentY] = new Maze.Block((rand.Next(0,6) != 1), (rand.Next(0, 6) != 1), (rand.Next(0, 6) != 1), (rand.Next(0, 6) != 1));
//maze.maze[currentX, currentY] = new Maze.Block(false, false, false, true);
//blockHasInit[currentX, currentY] = true;
//currentX++;
//currentY++;
//if (!maze.InBound(currentX, currentY))
//{
//    currentX = 0;
//    currentY++;
//}




//int dir = rand.Next(0, 5); // [0 , 4]
//if (dir == 0)
//    if (maze.InBound(currentX + 1, currentY))
//        currentX++;
//else if (dir == 1)
//    if (maze.InBound(currentX  -1, currentY))
//        currentX--;
//else if (dir == 2)
//    if (maze.InBound(currentX, currentY + 1))
//        currentY++;
//else if (dir == 3)
//    if (maze.InBound(currentX++, currentY - 1))
//        currentY--;





//maze.Set(currentX, currentY, Maze.Direction.Up, !dir.IsDirSet(Maze.Direction.Up));
//maze.Set(currentX, currentY, Maze.Direction.Down, !dir.IsDirSet(Maze.Direction.Down));
//maze.Set(currentX, currentY, Maze.Direction.Right, !dir.IsDirSet(Maze.Direction.Right));
//maze.Set(currentX, currentY, Maze.Direction.Left, !dir.IsDirSet(Maze.Direction.Left));