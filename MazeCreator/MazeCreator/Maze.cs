using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeCreator;

/// <summary>
/// Top left is (0,0)
/// </summary>
internal class Maze
{
    // represents the way you can take from this Block to the next
    // true means you can go that way
    public struct Block
    {
        public bool Up;
        public bool Down;
        public bool Right;
        public bool Left;

        public Block(bool up, bool down, bool right, bool left)
        {
            Up = up;
            Down = down;
            Right = right;
            Left = left;
        }

        public void Set(Direction dir, bool value = true)
        {
            if (dir.IsDirSet(Direction.Up))
                Up = value;
            if (dir.IsDirSet(Direction.Down))
                Down = value;
            if (dir.IsDirSet(Direction.Right))
                Right = value;
            if (dir.IsDirSet(Direction.Left))
                Left = value;
        }
    }

    public struct Direction
    {
        // 1,2,4,8 so you can have multible directions with || operator, and (num & dir) == dir to see what is set
        public const int Up = 1;
        public const int Down = 2;
        public const int Right = 4;
        public const int Left = 8;
        public const int All = Up | Down | Right | Left;

        // if a dir is set to true, means you can go that way
        // 1 means its a valid dir, 0 means its closed
        public int Val;

        public Direction(bool up, bool down, bool right, bool left)
        {
            Val = 0;
            if (up)
                Val |= Up;
            if (down)
                Val |= Down;
            if (right)
                Val |= Right;
            if (left)
                Val |= Left;
        }

        public bool IsDirSet(int dir)
        {
            return (Val & dir) == dir;
        }

        // true means you can go that dir
        public void Set(int dir, bool open = true)
        {
            if ((dir & Up) == Up)
            {
                if (open)
                    Val |= Up;
                else
                    Val &= All - Up;
            }
            if ((dir & Down) == Down)
            {
                if (open)
                    Val |= Down;
                else
                    Val &= All - Down;
            }
            if ((dir & Right) == Right)
            {
                if (open)
                    Val |= Right;
                else
                    Val &= All - Right;
            }
            if ((dir & Left) == Left)
            {
                if (open)
                    Val |= Left;
                else
                    Val &= All - Left;
            }
        }

        public bool IsDirOpen(int dir)
        {
            if ((dir & Val) == dir)
                return true;
            return false;
        }

        /// <summary>
        /// Just switches up and down, left and right
        /// </summary>
        public void Reverse()
        {
            int newVal = 0;
            if (IsDirOpen(Up))
                newVal |= Down;
            if (IsDirOpen(Down))
                newVal |= Up;
            if (IsDirOpen(Right))
                newVal |= Left;
            if (IsDirOpen(Left))
                newVal |= Right;
            Val = newVal;
        }

        public static Direction GetDirFromOffset(int x, int y)
        {
            return new Direction((y == -1), (y == 1), (x == 1), (x == -1));
        }

        //public static explicit operator Direction(int obj)
        //{
        //    return new Direction((obj & Up) == Up, (obj & Down) == Down, (obj & Right) == Right, (obj & Left) == Left);
        //}
        public static implicit operator Direction(int obj)
        {
            return new Direction((obj & Up) == Up, (obj & Down) == Down, (obj & Right) == Right, (obj & Left) == Left);
        }
    }

    private int height;
    public int Height { get { return height; } }
    private int width;
    public int Width { get { return width; } }
    public Block[,] maze;

    public int startX;
    public int startY;
    public int endX;
    public int endY;

    public Maze(int w, int h)
    {
        width = w;
        height = h;
        maze = new Block[width, height];
    }

    public bool IsMoveValid(int x, int y, Direction dir)
    {
        ref Block block = ref maze[x, y];
        if (dir.IsDirSet(Direction.Up))
            return block.Up;
        if (dir.IsDirSet(Direction.Down))
            return block.Down;
        if (dir.IsDirSet(Direction.Right))
            return block.Right;
        if (dir.IsDirSet(Direction.Left))
            return block.Left;

        throw new Exception("Invald direction");
    }

    public void Set(int x, int y, Direction dir, bool value = true)
    {
        maze[x, y].Set(dir, value);
    }

    //public void SetBothSides(int x, int y, Direction dir, bool value = true)
    //{
    //    if (!InBound(x, y))
    //        return;

    //    if (dir.IsDirSet(Direction.Up))
    //        SetBlock(x, y, 0, -1, Direction.Up, value);
    //    if (dir.IsDirSet(Direction.Down))
    //        SetBlock(x, y, 0, 1, Direction.Down, value);
    //    if (dir.IsDirSet(Direction.Right))
    //        SetBlock(x, y, 1, 0, Direction.Right, value);
    //    if (dir.IsDirSet(Direction.Left))
    //        SetBlock(x, y, -1, 0, Direction.Left, value);
    //}

    //private void SetBlock(int x, int y, int xOffSet, int yOffSet, Direction dir, bool value = true)
    //{
    //    // set block
    //    maze[x, y].Set(dir, value);

    //    // if block on the other side is in bounds, also set that
    //    dir.Reverse();
    //    if (InBound(xOffSet, yOffSet))
    //        maze[xOffSet, yOffSet].Set(dir, value);
    //}

    public bool InBound(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
            return false;
        else
            return true;
    }

    public void LoadLevel(byte[] bytes)
    {
        throw new NotImplementedException();
    }

    public byte[] GetLevel()
    {
        throw new NotImplementedException();
    }
}
