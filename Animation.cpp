#include "Animation.h"



Animation::Animation(SDL_Texture* spritesheet, int startframe, int framecount, int framewidth, int frameheight, bool loop)
{
	Animation::spritesheet = spritesheet;
	Animation::frame = 0;
	Animation::startframe = startframe;
	Animation::framecount = framecount;
	Animation::loop = loop;
	Animation::frametime = 0.0f;
	Animation::framewidth = framewidth;
	Animation::frameheight = frameheight;

	create_frames();
}

Animation::~Animation()
{
	Animation::spritesheet = nullptr;
	for (int i = 0;i < framecount;i++)
	{
		delete (frames[i]);
	}
}

void Animation::create_frames()
{
	for (int i = 0;i < framecount;i++)
	{
		SDL_FRect* f = new SDL_FRect();
		f->w = (float)framewidth;
		f->h = (float)frameheight;
		f->x = (float)((startframe + i) * framewidth);
		f->y = 0.0f;
		frames.push_back(f);
	}
}

void Animation::update(double &deltatime)
{
	Animation::frametime += deltatime;
	if (frametime > 0.1f)
	{
		Animation::frame++;
		if (Animation::frame >= Animation::framecount)
		{
			frame = Animation::loop ? 0 : Animation::framecount - 1;
		}
		Animation::frametime = 0;
	}
}

void Animation::render(SDL_Renderer* renderer, SDL_FRect* destination)
{
	SDL_RenderTexture(renderer, spritesheet, frames[frame], destination);
}
