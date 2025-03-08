#include "IsoTileMap.h"
#include <SDL3/SDL.h>
#include <SDL3/SDL_rect.h>

IsoTile::IsoTile(
    uint32_t row,
    uint32_t col,
    uint32_t id,
    std::array<float, 2> pos,
    SDL_Texture* texture,
    bool active, 
    bool dead)
{
    F = 0, G = 0, H = 0;
    parent = nullptr;

    IsoTile::row = row;
    IsoTile::col = col;
    IsoTile::id = id;
    IsoTile::position = pos;
    IsoTile::active = active;
    IsoTile::dead = dead;
    IsoTile::texture = texture;
    
    // Setup the bounding rectangle used for drawing the texture.
    rect.x = pos[0] - ISO_TILE_WIDTH_HALF;
    rect.y = pos[1] - ISO_TILE_HEIGHT_HALF;
    rect.w = ISO_TILE_WIDTH;
    rect.h = ISO_TILE_HEIGHT;

    // Setup the source rectangle used for drawing the texture.

    // The top surface is made of two triangles because isometric etc.
    // It's basically a diamond! We'll be using this for mouse-over 
    // effects etc. But, we calculate where they are based on the 
    // top-left corner of the rectangle.
    std::array<float, 2> top, right, bottom, left;
    top = { position[0] + 21 ,position[1] + 1 };
    right = { position[0] + 45, position[1] + 13 };
    bottom = { position[0] + 25, position[1] + 25 };
    left = { position[0] + 4,position[1] + 13 };
    Triangle upper, lower;
    upper = { left, top, right };
    lower = { left, bottom, right };
    top_surface = { upper, lower };
}

IsoTile::~IsoTile()
{

}

const bool IsoTile::top_surface_contains_v2(const std::array<float, 2>& vec2)
{
    return triangle_contains_v2(top_surface[0], vec2) || triangle_contains_v2(top_surface[1], vec2);
}

void IsoTile::render(SDL_Renderer* renderer)
{
    SDL_RenderTexture(renderer, texture, nullptr, &rect);
}