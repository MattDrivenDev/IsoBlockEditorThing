#pragma once

#include <array>
#include "Animation.h"

class Dino
{
private:
	float direction = 1;
	std::array<float, 2> position = { 1.0f, 1.0f };
	SDL_Texture* spritesheet;
	Animation* anim_idle;
	Animation* anim_walk;
	Animation* anim_kick;
	Animation* anim_hurt;
	Animation* anim_crouch;
	Animation* anim_sneak;
	Animation* anim_current;
	SDL_Renderer* renderer;
	SDL_FRect* frect;

protected:
	static constexpr int FRAME_WIDTH = 24;
	static constexpr int FRAME_HEIGHT = 24;
	static constexpr int ANIM_IDLE_FRAME_START = 0;
	static constexpr int ANIM_IDLE_FRAME_COUNT = 4;
	static constexpr int ANIM_WALK_FRAME_START = 4;
	static constexpr int ANIM_WALK_FRAME_COUNT = 6;
	static constexpr int ANIM_KICK_FRAME_START = 10;
	static constexpr int ANIM_KICK_FRAME_COUNT = 3;
	static constexpr int ANIM_HURT_FRAME_START = 13;
	static constexpr int ANIM_HURT_FRAME_COUNT = 4;
	static constexpr int ANIM_CROUCH_FRAME_START = 17;
	static constexpr int ANIM_CROUCH_FRAME_COUNT = 1;
	static constexpr int ANIM_SNEAK_FRAME_START = 18;
	static constexpr int ANIM_SNEAK_FRAME_COUNT = 6;
	static constexpr float SPEED = 50.0f;
	static SDL_Texture* load_spritesheet(SDL_Renderer* renderer, const char* filename);

public:
	Dino(std::array<float, 2> position, SDL_Renderer* renderer, SDL_Texture* spritesheet);
	~Dino();
	virtual void update(double&deltatime);
	virtual void render();
};

class RedDino : public Dino
{
public:
	RedDino(std::array<float, 2> position, SDL_Renderer* renderer)
		:Dino(position, renderer, load_spritesheet(renderer, "resource/dinos/red.png")) { }
	void update(double& deltatime) override;
};

class GreenDino : public Dino
{
public:
	GreenDino(std::array<float, 2> position, SDL_Renderer* renderer)
		:Dino(position, renderer, load_spritesheet(renderer, "resource/dinos/green.png")) { }
	void update(double& deltatime) override;
};

class BlueDino : public Dino
{
public:
	BlueDino(std::array<float, 2> position, SDL_Renderer* renderer)
		:Dino(position, renderer, load_spritesheet(renderer, "resource/dinos/blue.png")) { }
	void update(double& deltatime) override;
};

class YellowDino : public Dino
{
public:
	YellowDino(std::array<float, 2> position, SDL_Renderer* renderer)
		:Dino(position, renderer, load_spritesheet(renderer, "resource/dinos/yellow.png")) { }
	void update(double& deltatime) override;
};