using System.Diagnostics;

GUI.MazeCreatorGUI game = new();

Stopwatch sw = new Stopwatch();
float lastTime = 1;

game.Init();

while (game.IsRunning()) // should probely make ít event bassed
{
    sw.Restart();

    game.HandleInputs();
    game.Update(lastTime);
    game.Render(lastTime);

    //Thread.Sleep(50);
    lastTime = (float)sw.Elapsed.TotalMilliseconds;
}

game.CleanUp();