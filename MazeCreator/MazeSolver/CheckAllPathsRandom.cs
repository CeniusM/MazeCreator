using MazeCreator;

namespace MazeSolver;

internal class CheckAllPathsRandom
{
    public Maze maze;
    public Walker walker;
    public Coord end;
    public bool[,] Checked;
    public bool Done = false;

    //public Stack<Coord> trail;

    public CheckAllPathsRandom(Maze maze)
    {
        this.maze = maze;
        walker = new(new(maze.startX, maze.startY), Math.Abs(maze.startX - end.x) + Math.Abs(maze.startY - end.y));
        end = new(maze.endX, maze.endY);
        Checked = new bool[maze.Width, maze.Height];
        Checked[walker.pos.x, walker.pos.y] = true;
        //trail = new Stack<Coord>(Math.Abs(maze.startX - end.x) + Math.Abs(maze.startY - end.y));
    }

    // -1 = defult step, 0 = finish, 0 < num means just finish
    public void Step(int stepSize = -1)
    {
        if (Done)
            return;
        if (stepSize == 0)
            stepSize = maze.Width * maze.Height;
        else if (stepSize < 0)
            stepSize = 1;

        for (int i = 0; i < stepSize; i++)
        {

            // generation code

            // each walker
            //for (int walkerNum = 0; walkerNum < walkers.Count(); walkerNum++)
            //{
            //    Walker walker = walkers[walkerNum];

            // each offSet until succes
            while (true)
            {
                if (walker.coordsTestetCount == 4)
                {
                    walker.GoToLastPos();
                    walker.ResetOffSet();
                }

                // make sure the walker is still valid
                //bool JustSpawned = false;
                //if (walker.coordsTestetCount == 4)
                //{
                //    if (walker.trail.Count() == 0)
                //    {
                //        // means it is dead and will spawn somewhere else
                //        // at another walker, and we know if it dies it isent the only one
                //        Random rand = new Random();
                //        while (true)
                //        {
                //            int newWalker = rand.Next(0, walkers.Count());
                //            if (newWalker == walkerNum)
                //                continue;
                //            walker.pos = walkers[newWalker].pos;
                //            JustSpawned = true;
                //            break;
                //        }

                //    }
                //walker.ResetOffSet();
                //if (JustSpawned)
                //    break;
                //    walker.GoToLastPos();
                //}
                //if (JustSpawned)
                //    break;

                // get not tested offSet
                Coord offSet = walker.GetOffSet();
                Coord newPos = new Coord(walker.pos.x + offSet.x, walker.pos.y + offSet.y);

                // check if valid offSet
                Maze.Direction dir = Maze.Direction.GetDirFromOffset(offSet.x, offSet.y);
                if (!maze.InBound(newPos.x, newPos.y) ||
                    Checked[newPos.x, newPos.y] ||
                    !maze.IsMoveValid(walker.pos.x, walker.pos.y, dir))
                {
                    walker.InvalidateOffSet(offSet);
                    continue;
                }
                Checked[newPos.x, newPos.y] = true;

                walker.GoToNewPos(newPos);
                walker.ResetOffSet();

                if (walker.pos == end)
                {
                    Done = true;
                    return;
                }

                break;
                //}
            }
        }
    }
}
