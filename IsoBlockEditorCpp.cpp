// IsoBlockEditorCpp.cpp : This file contains the 'main' function. Program execution begins and ends there.

#include <stdio.h>
#include <SDL3/SDL.h>

static constexpr int SCREEN_WIDTH = 800;
static constexpr int SCREEN_HEIGHT = 600;

int main(int argc, char* args[])
{
    SDL_Window* window = NULL;
    SDL_Surface* screen_surface = NULL;

    if (SDL_Init(SDL_INIT_VIDEO) < 0)
    {
        return 1;
    }

    window = SDL_CreateWindow("IsoBlockEditorThing",
        SCREEN_WIDTH, SCREEN_HEIGHT, 0);

    if (window == NULL)
    {
        return 1;
    }

    screen_surface = SDL_GetWindowSurface(window);
    SDL_FillSurfaceRect(screen_surface, NULL, SDL_MapSurfaceRGB(screen_surface, 0xc2, 0xc2, 0xc2));
    SDL_UpdateWindowSurface(window);

    SDL_Event e;
    bool quit = false;
    while (quit == false)
    {
        while (SDL_PollEvent(&e))
        {
            if (e.type == SDL_EVENT_QUIT)
            {
                quit = true;
            }
        }
    }

    SDL_DestroyWindow(window);
    SDL_Quit();

    return 0;
}