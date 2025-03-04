#pragma once

#include "Math.h"
#include <array>
#include <SDL3/SDL.h>

static constexpr unsigned char ISO_TILE_MAP_MAX_WIDTH = 200;
static constexpr unsigned char ISO_TILE_MAP_MAX_HEIGHT = 200;
static constexpr uint32_t ISO_TILE_WIDTH = 50;
static constexpr uint32_t ISO_TILE_WIDTH_HALF = ISO_TILE_WIDTH / 2;
static constexpr uint32_t ISO_TILE_HEIGHT = 50;
static constexpr uint32_t ISO_TILE_HEIGHT_HALF = ISO_TILE_HEIGHT / 2;
static constexpr uint32_t ISO_TILE_TOP_SURFACE_WIDTH = 42;
static constexpr uint32_t ISO_TILE_TOP_SURFACE_HEIGHT = 12;
static constexpr uint32_t ISO_TILE_HORIZONTAL_OFFSET = 21;

class IsoTile
{
private:
	uint32_t row;
	uint32_t col;
	uint32_t id;
	bool active;
	bool dead;
	std::array<float, 2> position;
	std::array<Triangle, 2> top_surface;
	SDL_FRect rect;
	SDL_Texture* texture;
	
	IsoTile* parent;
	// Parent, G, H, and F are used for A* pathfinding.
	int G, H, F;

public:
	IsoTile(uint32_t row, uint32_t col, uint32_t id,
		std::array<float, 2> pos,
		SDL_Texture* texture,
		bool active, bool dead);
	~IsoTile();
	const bool top_surface_contains_v2(const std::array<float, 2>& vec2);
	void render(SDL_Renderer* renderer);
};

class IsoTileMap
{

};