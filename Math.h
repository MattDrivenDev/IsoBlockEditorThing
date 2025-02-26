#pragma once

#include <array>

struct Triangle
{
	std::array<float, 2> a;
	std::array<float, 2> b;
	std::array<float, 2> c;
};

const bool triangle_contains_v2(Triangle& triangle, const std::array<float, 2>& vec2);

inline const float v2_dot(const std::array<float, 2>& vec2_a, const std::array<float, 2>& vec2_b);