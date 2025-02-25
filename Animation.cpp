#include "Animation.h"

void Animation::update(float &deltatime)
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

void Animation::render()
{

}
