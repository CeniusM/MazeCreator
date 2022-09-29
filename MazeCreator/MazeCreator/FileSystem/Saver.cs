using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeCreator.FileSystem;
// needs a better name
internal unsafe class Saver
{
    public static void WriteSaveToFile(string path, byte[] bytes)
    {
        Console.WriteLine(path);
        Console.WriteLine(bytes.Length);

        //File.WriteAllBytes(path, bytes);

        using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            fs.Write(bytes, 0, bytes.Length);
        }
    }

    public static byte[] GetBytes(Maze maze)
    {
        byte[] bytes = new byte[(4 * 6) + maze.Height * maze.Width * 4];

        fixed (byte* bytesPtr = bytes)
        {
            int* intPtr = (int*)bytesPtr;

            intPtr[0] = maze.Width;
            intPtr[1] = maze.Height;
            intPtr[2] = maze.startX;
            intPtr[3] = maze.startY;
            intPtr[4] = maze.endX;
            intPtr[5] = maze.endY;

            // used for the map
            byte* ptr = &bytesPtr[24];
            for (int i = 0; i < maze.Width; i++)
            {
                for (int j = 0; j < maze.Height; j++)
                {
                    ptr[i * 4 + (j * maze.Width * 4) + 0] = Convert.ToByte(maze.maze[i, j].Up);
                    ptr[i * 4 + (j * maze.Width * 4) + 1] = Convert.ToByte(maze.maze[i, j].Down);
                    ptr[i * 4 + (j * maze.Width * 4) + 2] = Convert.ToByte(maze.maze[i, j].Left);
                    ptr[i * 4 + (j * maze.Width * 4) + 3] = Convert.ToByte(maze.maze[i, j].Right);
                }
            }
        }
        //for (int i = 0; i < 6; i++)
        //    Console.WriteLine(); // the first 6 ints
        //for (int i = 24; i < bytes.Length; i++)
        //{
        //    Console.WriteLine(Convert.ToBoolean(bytes[i]));
        //}

        return bytes;
    }
}
