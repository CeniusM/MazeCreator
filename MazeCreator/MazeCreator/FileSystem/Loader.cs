using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MazeCreator;

namespace MazeCreator.FileSystem;

internal class Loader
{
    public unsafe static Maze LoadMaze(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);

        int w = 0, h = 0, sX = 0, sY = 0, eX = 0, eY = 0;

        Maze maze;

        fixed (byte* bytesPtr = bytes)
        {
            int* intPtr = (int*)bytesPtr;

            w = intPtr[0];
            h = intPtr[1];
            sX = intPtr[2];
            sY = intPtr[3];
            eX = intPtr[4];
            eY = intPtr[5];

            maze = new Maze(w, h, sX, sY, eX, eY);

            // used for the map
            byte* ptr = &bytesPtr[24];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    maze.maze[i, j].Up = Convert.ToBoolean(ptr[i * 4 + (j * maze.Width * 4) + 0]);
                    maze.maze[i, j].Down = Convert.ToBoolean(ptr[i * 4 + (j * maze.Width * 4) + 1]);
                    maze.maze[i, j].Left = Convert.ToBoolean(ptr[i * 4 + (j * maze.Width * 4) + 2]);
                    maze.maze[i, j].Right = Convert.ToBoolean(ptr[i * 4 + (j * maze.Width * 4) + 3]);
                }
            }
        }

        return maze;
    }
}