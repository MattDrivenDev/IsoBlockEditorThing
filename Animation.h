#pragma once

class Animation
{
private:
	bool loop;
	int frame;
	int framecount;
	float frametime;

public:
	Animation(int framecount, bool loop) 
		:framecount(framecount), 
		loop(loop), 
		frame(0), 
		frametime(0.0f) {}

	void update(float &deltatime);
	void render();
};

