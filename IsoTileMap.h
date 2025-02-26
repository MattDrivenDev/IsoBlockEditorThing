#pragma once

#include "Math.h"
#include <array>

class IsoTile
{
private:
	uint32_t row;
	uint32_t col;
	uint32_t id;
	bool active;
	bool dead;
	std::array<float, 2> render_position = { 1.0f, 1.0f };
	std::array<Triangle, 2> top_surface;

public:
	IsoTile(uint32_t row, uint32_t col, uint32_t id,
		std::array<float, 2> pos,
		bool active, bool dead);
	~IsoTile();

	static constexpr uint32_t ISO_TILE_WIDTH = 50;
	static constexpr uint32_t ISO_TILE_WIDTH_HALF = ISO_TILE_WIDTH / 2;
	static constexpr uint32_t ISO_TILE_HEIGHT = 50;
	static constexpr uint32_t ISO_TILE_HEIGHT_HALF = ISO_TILE_HEIGHT / 2;
	static constexpr uint32_t ISO_TILE_TOP_SURFACE_WIDTH = 42;
	static constexpr uint32_t ISO_TILE_TOP_SURFACE_HEIGHT = 12;
	static constexpr uint32_t ISO_TILE_HORIZONTAL_OFFSET = 21;
	const bool top_surface_contains_v2(const std::array<float, 2>& vec2);
};