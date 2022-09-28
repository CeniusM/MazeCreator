using SDL2Lib;

namespace Interfaces;

internal class MazeCreatorGUI
{
    public const int MazeHeight = 20; // the maze's dimensions
    public const int MazeWidth = 20;
    public const int BlockHeight = 20; // each block in maze dimensions in pixels
    public const int BlockWidth = 20;

    private World.World world;
    private Renderer.WorldRenderer worldRender;

    private bool _isRunning = true;

    private static IntPtr window;
    private static IntPtr renderer;

    public MazeCreatorGUI()
    { }

    public void Init()
    {
        // Initilizes SDL.
        if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
        {
            Console.WriteLine($"There was an issue initilizing SDL. {SDL.SDL_GetError()}");
        }

        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        window = SDL.SDL_CreateWindow("Platformer", 
            SDL.SDL_WINDOWPOS_UNDEFINED,
            SDL.SDL_WINDOWPOS_UNDEFINED,
            Settings.ScreenWidthInPixels,
            Settings.ScreenHeightInPixels,
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

        if (window == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");
        }

        // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
        renderer = SDL.SDL_CreateRenderer(window,
                                                -1,
                                                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
                                                SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        if (renderer == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
        }

        // Initilizes SDL_image for use with png files.
        if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) == 0)
        {
            Console.WriteLine($"There was an issue initilizing SDL2_Image {SDL_image.IMG_GetError()}");
        }

        world = new World.World();
        worldRender = new Renderer.WorldRenderer(world, renderer, Settings.ScreenWidthInPixels, Settings.ScreenHeightInPixels, Settings.BlockLength);
    }

    public void HandleInputs()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
        {
            if (e.type == SDL.SDL_EventType.SDL_QUIT)
                _isRunning = false;
            else
                world.Event(ref e);
        }
    }

    public void Update(float delta = 17f)
    {
        world.Update(delta);
    }

    public void Render(float delta)
    {
        worldRender.RenderWorldToRenderer(delta);
        SDL.SDL_RenderPresent(renderer);
    }

    public void CleanUp()
    {
        // Clean up the resources that were created.
        SDL.SDL_DestroyRenderer(renderer);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
    }

    public bool IsRunning()
    {
        return _isRunning;
    }
}
