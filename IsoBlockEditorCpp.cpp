// IsoBlockEditorCpp.cpp : This file contains the 'main' function. Program execution begins and ends there.

#include <stdio.h>
#include "SDL.h"

static constexpr int SCREEN_WIDTH = 640;
static constexpr int SCREEN_HEIGHT = 480;

int main(int argc, char* args[])
{
    SDL_Window* window = NULL;
    SDL_Surface* screen_surface = NULL;

    if (SDL_Init(SDL_INIT_VIDEO) < 0)
    {
        return 1;
    }

    window = SDL_CreateWindow("IsoBlockEditorThing", 
        SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 
        SCREEN_WIDTH, SCREEN_HEIGHT, SDL_WINDOW_SHOWN);

    if (window == NULL)
    {
        return 1;
    }

    screen_surface = SDL_GetWindowSurface(window);
    SDL_FillRect(screen_surface, NULL, SDL_MapRGB(screen_surface->format, 0xff, 0xff, 0xff));
    SDL_UpdateWindowSurface(window);

    SDL_Event e;
    bool quit = false;
    while (quit == false)
    {
        while (SDL_PollEvent(&e))
        {
            if (e.type == SDL_QUIT)
            {
                quit = true;
            }
        }
    }

    SDL_DestroyWindow(window);
    SDL_Quit();

    return 0;
}