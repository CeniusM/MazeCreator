// problems*
// alot of artifacting at the end and begining of the sims
// maby make them spawn and despawn depending on some facktors that has to do with hpw much they are squishing
//
// alot if long kinda wires, fewer walkers the less of a problem
//
// one problem may be that the middle roads pretty much allways leads the the end,
// one way of fixing this is maby to slow down the closer they get to the end compared to how far the other workers are
// and the further away they are the faster they will get


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

internal class Walker
{
    private Random rand = new Random();

    public Stack<Coord> trail;
    public Coord pos;

    public int coordsTestetCount = 0;
    private List<Coord> coordsTestet = new List<Coord>(4);

    public Walker(Coord pos, int stackSize = 10)
    {
        this.pos = pos;
        trail = new Stack<Coord>(stackSize);
    }

    public void GoToNewPos(Coord coord)
    {
        trail.Push(pos);
        pos = coord;
    }

    public void GoToLastPos()
    {
        pos = trail.Pop();
    }

    public void ResetOffSet()
    {
        coordsTestetCount = 0;
        coordsTestet.Clear();
    }

    public Coord GetOffSet()
    {
        if (coordsTestetCount == 4)
            throw new Exception("gone over the max tested coords amount");

        int x;
        int y;

        do
        {
            x = rand.Next(-1, 2);
            y = rand.Next(-1, 2);
        } while (
        // both arent 0
        (x == 0 && y == 0) ||

        // diagonel
        (x != 0 && y != 0) ||

        // if it is allredy tested
        coordsTestet.Contains(new Coord(x, y))
        );

        return new Coord(x, y);
    }

    public void InvalidateOffSet(Coord offSet)
    {
        coordsTestetCount++;
        coordsTestet.Add(offSet);
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

    //public Stack<Coord> winingTrail;

    public Maze maze;
    public List<Walker> walkers;

    // for generation
    public bool[,] blockHasInit;
    // amount of blocks that have been init
    public int step;
    public int stepsNeeded;
    public int stepsPerRound;

    public MazeGenerator(int width, int height, int walkersCount = -1, int stepsPerRound = 1)
    {
        blockHasInit = new bool[width, height];
        maze = new Maze(width, height);
        step = 0;
        stepsNeeded = width * height;
        this.stepsPerRound = stepsPerRound;

        // currently starts in top right and ends in buttom left
        startX = 0;
        startY = 0;
        endX = width - 1;
        endY = height - 1;

        if (walkersCount == -1)
            walkersCount = width * height / 500;
        if (walkersCount < 2)
            walkersCount = 2;

        walkers = new List<Walker>(walkersCount);
        for (int i = 0; i < walkersCount; i++)
            walkers.Add(new Walker(new Coord(startX, startY)));
    }

    public void StepForward(int steps = -1)
    {
        if (done)
            return;
        if (steps == -1)
            steps = stepsPerRound;
        for (int i = 0; i < steps; i++)
        {
            // generation code

            // each walker
            for (int walkerNum = 0; walkerNum < walkers.Count(); walkerNum++)
            {
                Walker walker = walkers[walkerNum];

                // each offSet until succes
                while (true)
                {
                    // make sure the walker is still valid
                    bool JustSpawned = false;
                    if (walker.coordsTestetCount == 4)
                    {
                        if (walker.trail.Count() == 0)
                        {
                            // means it is dead and will spawn somewhere else
                            // at another walker, and we know if it dies it isent the only one
                            Random rand = new Random();
                            while (true)
                            {
                                int newWalker = rand.Next(0, walkers.Count());
                                if (newWalker == walkerNum)
                                    continue;
                                walker.pos = walkers[newWalker].pos;
                                JustSpawned = true;
                                break;
                            }

                        }
                        walker.ResetOffSet();
                        if (JustSpawned)
                            break;
                        walker.GoToLastPos();
                    }
                    if (JustSpawned)
                        break;

                    // get not tested offSet
                    Coord offSet = walker.GetOffSet();
                    Coord newPos = new Coord(walker.pos.x + offSet.x, walker.pos.y + offSet.y);

                    // check if valid offSet
                    if (!maze.InBound(newPos.x, newPos.y) || blockHasInit[newPos.x, newPos.y])
                    {
                        walker.InvalidateOffSet(offSet);
                        continue;
                    }

                    // got valid offSet
                    Maze.Direction dir = Maze.Direction.GetDirFromOffset(offSet.x, offSet.y);
                    maze.Set(walker.pos.x, walker.pos.y, dir, true);
                    dir.Reverse();
                    maze.Set(newPos.x, newPos.y, dir, true);
                    blockHasInit[newPos.x, newPos.y] = true;

                    walker.GoToNewPos(newPos);
                    walker.ResetOffSet();

                    step++;
                    if (step == stepsNeeded)
                    {
                        done = true;
                        return;
                    }

                    // check if landed on end
                    if (newPos.x == endX && newPos.y == endY)
                    {
                        
                    }

                    break;
                }
            }
        }
    }
}