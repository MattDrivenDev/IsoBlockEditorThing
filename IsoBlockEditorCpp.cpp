// IsoBlockEditorCpp.cpp : This file contains the 'main' function. Program execution begins and ends there.

#include <stdio.h>
#include <SDL3/SDL.h>
#include <SDL3_image/SDL_image.h>
#include "IsoTileMap.h"
#include "Dino.h"

static constexpr int SCREEN_WIDTH = 800;
static constexpr int SCREEN_HEIGHT = 600;

SDL_Window* window = nullptr;
SDL_Renderer* window_renderer = nullptr;
SDL_Texture* texture = nullptr;
IsoTile* test_tile = nullptr;
Dino* red = nullptr;

void render();
void update(double& deltatime);

int main(int argc, char* args[])
{
    if (!SDL_Init(SDL_INIT_VIDEO))
    {
        return 1;
    }

    window = SDL_CreateWindow("IsoBlockEditorThing", SCREEN_WIDTH, SCREEN_HEIGHT, 0);
    if (window == NULL)
    {
        return 2;
    }

    window_renderer = SDL_CreateRenderer(window, nullptr);
    if (!window_renderer)
    {
        SDL_Log("Couldn't create the renderer: %s", SDL_GetError());
        return 2;
    }

    texture = IMG_LoadTexture(window_renderer, "resource/blocks_50x50/isometric_pixel_0000.png");
    if (!texture)
    {
        SDL_Log("Couldn't create the texture: %s", SDL_GetError());
        return 3;
    }

    test_tile = new IsoTile(0, 0, 1, { SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 }, texture, true, false);
    red = new RedDino({ SCREEN_WIDTH / 2, ((SCREEN_HEIGHT / 2) - 16) }, window_renderer);;

    uint64_t lasttime = SDL_GetPerformanceCounter();
    double deltatime = 0.0;

    SDL_Event e;
    bool quit = false;
    while (quit == false)
    {
        uint64_t currenttime = SDL_GetPerformanceCounter();
        uint64_t frequency = SDL_GetPerformanceFrequency();
        deltatime = static_cast<double>(currenttime - lasttime) / static_cast<double>(frequency);
        lasttime = currenttime;

        while (SDL_PollEvent(&e))
        {
            if (e.type == SDL_EVENT_QUIT)
            {
                quit = true;
            }
        }

        update(deltatime);
        render();
    }

    delete test_tile;
    SDL_DestroyTexture(texture);
    SDL_DestroyRenderer(window_renderer);
    SDL_DestroyWindow(window);
    SDL_Quit();

    return 0;
}

void update(double& deltatime)
{
    red->update(deltatime);
}

void render()
{
    test_tile->render(window_renderer);
    red->render();
    SDL_RenderPresent(window_renderer);
}