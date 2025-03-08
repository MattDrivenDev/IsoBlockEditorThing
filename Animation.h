#pragma once
#include <vector>
#include <SDL3/SDL.h>

class Animation
{
private:
	bool loop;
	int frame;
	int startframe;
	int framecount;
	int framewidth;
	int frameheight;
	float frametime;
	SDL_Texture* spritesheet;
	std::vector<SDL_FRect*> frames;
	void create_frames();

public:
	Animation(
		SDL_Texture* spritesheet,
		int startframe,
		int framecount,
		int framewidth,
		int frameheight,
		bool loop);
	~Animation();
	void update(double &deltatime);
	void render(SDL_Renderer* renderer, SDL_FRect* destination);
};

