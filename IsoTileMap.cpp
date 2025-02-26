#include "IsoTileMap.h"


IsoTile::IsoTile(uint32_t row, uint32_t col, uint32_t id, std::array<float, 2> pos, bool active, bool dead)
{
    
}

IsoTile::~IsoTile()
{

}

const bool IsoTile::top_surface_contains_v2(const std::array<float, 2>& vec2)
{
    return triangle_contains_v2(top_surface[0], vec2) || triangle_contains_v2(top_surface[1], vec2);
}
