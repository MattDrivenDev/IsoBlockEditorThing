#include "Dino.h"
#include <SDL3_image/SDL_image.h>

SDL_Texture* Dino::load_spritesheet(SDL_Renderer* renderer, const char* filename)
{
	SDL_Texture* texture;
	texture = IMG_LoadTexture(renderer, filename);
	if (!texture)
	{
		SDL_Log("Couldn't create the texture: %s", SDL_GetError());
		throw;
	}

	return texture;
}

Dino::Dino(std::array<float, 2> position, SDL_Renderer* renderer, SDL_Texture* spritesheet)
{
	Dino::renderer = renderer;
	Dino::spritesheet = spritesheet;
	Dino::position = position;
	Dino::anim_idle = new Animation(spritesheet, ANIM_IDLE_FRAME_START, ANIM_IDLE_FRAME_COUNT, FRAME_WIDTH, FRAME_HEIGHT, true);
	Dino::anim_walk = new Animation(spritesheet, ANIM_WALK_FRAME_START, ANIM_WALK_FRAME_COUNT, FRAME_WIDTH, FRAME_HEIGHT, true);
	Dino::anim_kick = new Animation(spritesheet, ANIM_KICK_FRAME_START, ANIM_KICK_FRAME_COUNT, FRAME_WIDTH, FRAME_HEIGHT, true);
	Dino::anim_hurt = new Animation(spritesheet, ANIM_HURT_FRAME_START, ANIM_HURT_FRAME_COUNT, FRAME_WIDTH, FRAME_HEIGHT, true);
	Dino::anim_crouch = new Animation(spritesheet, ANIM_CROUCH_FRAME_START, ANIM_CROUCH_FRAME_COUNT, FRAME_WIDTH, FRAME_HEIGHT, true);
	Dino::anim_sneak = new Animation(spritesheet, ANIM_SNEAK_FRAME_START, ANIM_SNEAK_FRAME_COUNT, FRAME_WIDTH, FRAME_HEIGHT, true);
	Dino::anim_current = Dino::anim_idle;

	Dino::frect = new SDL_FRect();
	Dino::frect->x = (float)(position[0] - (FRAME_WIDTH / 2));
	Dino::frect->y = (float)(position[1] - (FRAME_HEIGHT / 2));
	Dino::frect->w = (float)FRAME_WIDTH;
	Dino::frect->h = (float)FRAME_HEIGHT;
}

Dino::~Dino()
{
	delete Dino::renderer;
	delete Dino::spritesheet;
	delete Dino::anim_idle;
	delete Dino::anim_walk;
	delete Dino::anim_kick;
	delete Dino::anim_hurt;
	delete Dino::anim_crouch;
	delete Dino::anim_sneak;
	delete Dino::anim_current;
	delete Dino::frect;
}

void Dino::update(double &deltatime)
{
	Dino::anim_current->update(deltatime);
}

void Dino::render()
{
	Dino::anim_current->render(Dino::renderer, Dino::frect);
}

void RedDino::update(double& deltatime)
{
	Dino::update(deltatime);
}

void GreenDino::update(double& deltatime)
{
	Dino::update(deltatime);
}

void BlueDino::update(double& deltatime)
{
	Dino::update(deltatime);
}

void YellowDino::update(double& deltatime)
{
	Dino::update(deltatime);
}