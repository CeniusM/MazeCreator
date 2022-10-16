using MazeCreator.MazeCreator;
using MazeCreator;
using System.Diagnostics;

namespace Benchy;

internal class Benchmarking
{
    // test the difrent solvers at difrent size mazes and i guess the speed of the generation
    public static void Start()
    {
        Stopwatch sw = new Stopwatch();
        int sampleSize = 200;
        sw.Restart();
        MazeGenerator mazeGenerator;
        List<Maze> mazes100x100 = new List<Maze>(sampleSize);
        for (int i = 0; i < sampleSize; i++)
        {
            mazeGenerator = new MazeGenerator(100, 100);
            mazeGenerator.StepForward(0);
            mazes100x100.Add(mazeGenerator.maze);
        }

        List<Maze> mazes200x200 = new List<Maze>(sampleSize);
        for (int i = 0; i < sampleSize; i++)
        {
            mazeGenerator = new MazeGenerator(200, 200);
            mazeGenerator.StepForward(0);
            mazes200x200.Add(mazeGenerator.maze);
        }
        long time = sw.ElapsedMilliseconds;
        Console.WriteLine($"Generating Mazes took {time}ms");
        //Console.ReadLine();

        Console.WriteLine("testing the solvers");
        MazeSolver.CheckAllPathsRandom solver;
        sw.Restart();
        for (int i = 0; i < sampleSize; i++)
        {
            solver = new MazeSolver.CheckAllPathsRandom(mazes100x100[i]);
            solver.Step(0);
        }
        long time1 = sw.ElapsedMilliseconds;
        sw.Restart();
        for (int i = 0; i < sampleSize; i++)
        {
            solver = new MazeSolver.CheckAllPathsRandom(mazes200x200[i]);
            solver.Step(0);
        }
        long time2 = sw.ElapsedMilliseconds;

        Console.WriteLine($"time1(100x100): {time1}ms, avg: {time1/sampleSize}ms");
        Console.WriteLine($"time2(200x200): {time2}ms, avg: {time2/sampleSize}ms");
        Console.ReadLine();
    }
}
