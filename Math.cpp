#include "Math.h"

// I remember researching this one when I did the C# implementation. I didn't really
// understand the Math before, and I don't really understand it now. But, I might 
// whiteboard it in due course... 
const bool triangle_contains_v2(Triangle& triangle, const std::array<float, 2>& vec2)
{
	// Compute the edge vectors
	std::array<float, 2> v0 = { triangle.c[0] - triangle.a[0], triangle.c[1] - triangle.a[1] };
	std::array<float, 2> v1 = { triangle.b[0] - triangle.a[0], triangle.b[1] - triangle.a[1] };
	std::array<float, 2> v2 = { vec2[0] - triangle.a[0], vec2[1] - triangle.a[1] };
	
	// Compute the dot products
	float dot00 = v2_dot(v0, v0);
	float dot01 = v2_dot(v0, v1);
	float dot02 = v2_dot(v0, v2);
	float dot11 = v2_dot(v1, v1);
	float dot12 = v2_dot(v1, v2);

	// Compute the inverse denominator (account for no division by zero)
	float inv_denom = 1 / ((dot00 * dot11) - (dot01 * dot01));
	if (inv_denom == 0.0f)
	{
		// Degenerate triangle
		return false;
	}

	// Compute the barycentric coordinates, which determines the vec2's
	// position relative to the triangle.
	float u = ((dot11 * dot02) - (dot01 * dot12)) * inv_denom;
	float v = ((dot00 * dot12) - (dot01 * dot02)) * inv_denom;
	
	// Return true if the position is inside the triangle
	return (u >= 0.0f) && (v >= 0.0f) && ((u + v) < 1.0f);
}

// I did wonder if C++ has inlining... it does!? :)
inline const float v2_dot(const std::array<float, 2>& vec2_a, const std::array<float, 2>& vec2_b)
{
	return (vec2_a[0] * vec2_b[0]) + (vec2_a[1] * vec2_b[1]);
}