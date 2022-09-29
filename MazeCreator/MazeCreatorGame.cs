﻿using SDL2Lib;
using MazeCreator;

namespace GUI;

internal class MazeCreatorGUI
{
    public /*const*/static int MazeHeight = 80; // the maze's dimensions
    public /*const*/static int MazeWidth = 80;
    public /*const*/static int BlockHeight = 10; //4 each block in maze dimensions in pixels
    public /*const*/static int BlockWidth = 10;
    public /*const*/static int BlockWallCelingWidth = 1; // the width of the wall and celling/floor lines *needs better name
    public /*const*/static int HalfBlockWallCelingWidth = BlockWallCelingWidth >> 1; // used to remove jaget cornors at wider walls -
    // only problem is that you also have to move every wall this amount up(rectVer) or the the left(rectHor), and make it BlockWallCelingWidth taller and wider

    private bool _isRunning = true;
    private bool _paused = false;

    private static IntPtr window;
    private static IntPtr renderer;

    SDL.SDL_Rect rect = new SDL.SDL_Rect();
    SDL.SDL_Rect rectHor = new SDL.SDL_Rect();
    SDL.SDL_Rect rectVer = new SDL.SDL_Rect();

    private MazeCreator.MazeGenerator mazeGenerator = new MazeCreator.MazeGenerator(MazeWidth, MazeHeight);

    public MazeCreatorGUI()
    { }

    private void ResizeWindow(int mazeX, int mazeY, int blockX, int blockY, int lineWidth)
    {
        MazeHeight += mazeY;
        MazeWidth += mazeX;
        BlockHeight += blockY;
        BlockWidth += blockX;
        BlockWallCelingWidth += lineWidth;

        rect.h = BlockHeight;
        rectVer.h = BlockHeight;

        rect.w = BlockWidth;
        rectHor.w = BlockWidth;
        
        rectHor.h = BlockWallCelingWidth;
        rectVer.w = BlockWallCelingWidth;

        SDL.SDL_SetWindowSize(window, BlockWidth * MazeWidth, BlockHeight * MazeHeight);
        //renderer = SDL.SDL_CreateRenderer(window,
        //                                        -1,
        //                                        SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
        //                                        SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        mazeGenerator = new MazeCreator.MazeGenerator(MazeWidth, MazeHeight);
    }

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
            MazeWidth * BlockWidth,
            MazeHeight * BlockHeight,
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

        //world = new World.World();
        //worldRender = new Renderer.WorldRenderer(world, renderer, Settings.ScreenWidthInPixels, Settings.ScreenHeightInPixels, Settings.BlockLength);

        // square
        rect.h = BlockHeight;
        rect.w = BlockWidth;
        rect.x = 0;
        rect.y = 0;

        // horizantal wall
        rectHor.h = BlockWallCelingWidth;
        rectHor.w = BlockWidth;
        rectHor.x = 0;
        rectHor.y = 0;

        // vertical celling/floor
        rectVer.h = BlockHeight;
        rectVer.w = BlockWallCelingWidth;
        rectVer.x = 0;
        rectVer.y = 0;
    }

    public void HandleInputs()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
        {
            if (e.type == SDL.SDL_EventType.SDL_QUIT)
                _isRunning = false;
            else if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                _paused = !_paused;
            else if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
            {
                if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_DOWN)
                {
                    ResizeWindow(0, 1, 0, 0, 0);
                }
                else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_UP)
                {
                    ResizeWindow(0, -1, 0, 0, 0);
                }
                else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_RIGHT)
                {
                    ResizeWindow(1, 0, 0, 0, 0);
                }
                else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_LEFT)
                {
                    ResizeWindow(-1, 0, 0, 0, 0);
                }
                else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_PLUS)
                {
                    ResizeWindow(0, 0, 1, 1, 0);
                }
                else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_MINUS)
                {
                    ResizeWindow(0, 0, -1, -1, 0);
                }
                else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_SPACE)
                {
                    mazeGenerator = new MazeCreator.MazeGenerator(MazeWidth, MazeHeight);
                }


            }
        }
    }

    public void Update(float delta = 17f)
    {
        if (!_paused)
            mazeGenerator.StepForward();
    }

    public void Render(float delta)
    {
        SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
        SDL.SDL_RenderClear(renderer);

        // draw maze
        void DrawSquare(int x, int y, ref Maze.Block block, int xDrawPos, int yDrawPos)
        {
            if (mazeGenerator.blockHasInit[x, y])
                SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 255, 255);
            else
                SDL.SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);

            // draw iner square
            rect.x = xDrawPos;
            rect.y = yDrawPos;
            SDL.SDL_RenderFillRect(renderer, ref rect);

            // draw walls based of block
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
            // top*
            if (!block.Up)
            {
                rectHor.x = xDrawPos;
                rectHor.y = yDrawPos;
                SDL.SDL_RenderFillRect(renderer, ref rectHor);
            }
            if (!block.Down)
            {
                rectHor.x = xDrawPos;
                rectHor.y = yDrawPos + BlockHeight - BlockWallCelingWidth;
                SDL.SDL_RenderFillRect(renderer, ref rectHor);
            }
            if (!block.Left)
            {
                rectVer.x = xDrawPos;
                rectVer.y = yDrawPos;
                SDL.SDL_RenderFillRect(renderer, ref rectVer);
            }
            if (!block.Right)
            {
                rectVer.x = xDrawPos + BlockWidth - BlockWallCelingWidth;
                rectVer.y = yDrawPos;
                SDL.SDL_RenderFillRect(renderer, ref rectVer);
            }
        }

        for (int x = 0; x < mazeGenerator.maze.Width; x++)
        {
            for (int y = 0; y < mazeGenerator.maze.Height; y++)
            {
                DrawSquare(x, y, ref mazeGenerator.maze.maze[x, y], x * BlockWidth, y * BlockHeight);
            }
        }

        //  +- BlockWallCelingWidth so it dosent block the walls
        rect.x = mazeGenerator.startX * BlockWidth - BlockWallCelingWidth;
        rect.y = mazeGenerator.startY * BlockWidth - BlockWallCelingWidth;
        SDL.SDL_SetRenderDrawColor(renderer, 0, 255, 0, 255);
        SDL.SDL_RenderFillRect(renderer, ref rect);
        rect.x = mazeGenerator.endX * BlockWidth + BlockWallCelingWidth;
        rect.y = mazeGenerator.endY * BlockWidth + BlockWallCelingWidth;
        SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
        SDL.SDL_RenderFillRect(renderer, ref rect);


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
