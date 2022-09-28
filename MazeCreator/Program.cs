using System.Diagnostics;



Stopwatch sw = new Stopwatch();
float lastTime = 1;

game.Init();

while (game.IsRunning()) // ~100 cycles a second
{
    sw.Restart();

    game.HandleInputs();
    game.Update(lastTime);
    game.Render(lastTime);
    SDL.SDL_Delay(17);

    lastTime = (float)sw.Elapsed.TotalMilliseconds;
}

game.CleanUp();