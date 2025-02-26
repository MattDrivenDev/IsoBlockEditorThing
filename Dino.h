#pragma once

#include <array>
#include "Animation.h"

class Dino
{
private:
	float direction = 1;
	std::array<float, 2> position = { 1.0f, 1.0f };
	Animation anim_idle;
	Animation anim_walk;
	Animation anim_kick;
	Animation anim_hurt;
	Animation anim_crouch;
	Animation anim_sneak;
	Animation* anim_current;

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

public:
	Dino(std::array<float, 2> position) 
		:position(position),
		anim_idle(ANIM_IDLE_FRAME_COUNT, true),
		anim_walk(ANIM_WALK_FRAME_COUNT, true),
		anim_kick(ANIM_KICK_FRAME_COUNT, true),
		anim_hurt(ANIM_HURT_FRAME_COUNT, true),
		anim_crouch(ANIM_CROUCH_FRAME_COUNT, true),
		anim_sneak(ANIM_SNEAK_FRAME_COUNT, true),
		anim_current(& anim_idle) {}
	virtual void update(float &deltatime);
	virtual void render();
};

class RedDino : public Dino
{
public:
	RedDino(std::array<float, 2> position) :Dino(position) {}
	void update(float& deltatime) override;
	void render() override;
};

class GreenDino : public Dino
{
public:
	GreenDino(std::array<float, 2> position) :Dino(position) {}
	void update(float& deltatime) override;
	void render() override;
};

class BlueDino : public Dino
{
public:
	BlueDino(std::array<float, 2> position) :Dino(position) {}
	void update(float& deltatime) override;
	void render() override;
};

class YellowDino : public Dino
{
public:
	YellowDino(std::array<float, 2> position) :Dino(position) {}
	void update(float& deltatime) override;
	void render() override;
};