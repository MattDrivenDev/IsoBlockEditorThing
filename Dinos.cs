using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace IsoBlockEditor
{
    public abstract class Dino
    {
        const int FRAME_WIDTH = 24;
        const int FRAME_HEIGHT = 24;
        const int FIRST_FRAME_IDLE = 0;
        const int FRAME_COUNT_IDLE = 4;
        const int FIRST_FRAME_WALK = 4;
        const int FRAME_COUNT_WALK = 6;
        const int FIRST_FRAME_KICK = 10;
        const int FRAME_COUNT_KICK = 3;
        const int FIRST_FRAME_HURT = 13;
        const int FRAME_COUNT_HURT = 4;
        const int FIRST_FRAME_CROUCH = 17;
        const int FRAME_COUNT_CROUCH = 1;
        const int FIRST_FRAME_SNEAK = 18;
        const int FRAME_COUNT_SNEAK = 6;

        float _direction;
        Vector2 _position;
        Texture2D _spritesheet;
        Texture2D _shadow;
        Animation _idle;
        Animation _walk;
        Animation _kick;
        Animation _hurt;
        Animation _crouch;
        Animation _sneak;
        Animation _currentAnimation;
        IsoBlockyMappy _map;
        IsoBlockyTile _currentTile;

        public Dino(ContentManager content, IsoBlockyMappy map, string spritesheet, Vector2 position)
        {
            _map = map;
            _spritesheet = content.Load<Texture2D>(spritesheet);
            _shadow = content.Load<Texture2D>("dinos/shadow");
            _position = position;
            _idle = new Animation(_spritesheet, CreateFrames(FIRST_FRAME_IDLE, FRAME_COUNT_IDLE));
            _walk = new Animation(_spritesheet, CreateFrames(FIRST_FRAME_WALK, FRAME_COUNT_WALK));
            _kick = new Animation(_spritesheet, CreateFrames(FIRST_FRAME_KICK, FRAME_COUNT_KICK));
            _hurt = new Animation(_spritesheet, CreateFrames(FIRST_FRAME_HURT, FRAME_COUNT_HURT));
            _crouch = new Animation(_spritesheet, CreateFrames(FIRST_FRAME_CROUCH, FRAME_COUNT_CROUCH));
            _sneak = new Animation(_spritesheet, CreateFrames(FIRST_FRAME_SNEAK, FRAME_COUNT_SNEAK));
            _currentAnimation = _idle;
            _currentTile = _map.GetTileFromPosition(position) ?? throw new Exception("No tile found at dino spawn point.");
        }

        private Rectangle[] CreateFrames(int firstFrame, int frameCount)
        {
            var frames = new Rectangle[frameCount];
            for (var i = 0; i < frameCount; i++)
            {
                var x = (firstFrame + i) * FRAME_WIDTH;
                frames[i] = new Rectangle(x, 0, FRAME_WIDTH, FRAME_HEIGHT);
            }
            return frames;
        }

        public void Update(GameTime gameTime)
        {
            if (_map.SelectedTile != null && _currentTile != _map.SelectedTile)
            {
                // Move to the selected tile!
                _currentTile = _map.SelectedTile;
                _currentAnimation = _walk;
                _direction = _currentTile.Position.X - _position.X;
            }

            if (_position != _currentTile.Position)
            {
                // When you're at the selected tile, move into the centre of the top surface
                _position = _currentTile.Position;
            }

            _currentAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // -12 to position so the sprite is centered on the position
            // -13 to offser the sprite so that it is half-way between
            // the center of the tile and the top of the tile.
            // -8 to offset the sprite so that it looks like it's standing
            // on the tile surface (roughly half the height of the actual sprite)
            var y = (int)_position.Y - 12 - 13 - 7;
            var destination = new Rectangle((int)_position.X - 12, y, 24, 24);
            var source = new Rectangle(0, 0, 24, 24);
            var effects = _direction >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(_shadow, destination, source, Color.White);
            _currentAnimation.Draw(spriteBatch, destination, effects);
        }
    }

    public class Red : Dino
    {
        public Red(ContentManager content, IsoBlockyMappy map, Vector2 position) 
            : base(content, map, "dinos/red", position) { }
    }

    public class Blue : Dino
    {
        public Blue(ContentManager content, IsoBlockyMappy map, Vector2 position) 
            : base(content, map, "dinos/blue", position) { }
    }

    public class Green : Dino
    {
        public Green(ContentManager content, IsoBlockyMappy map, Vector2 position) 
            : base(content, map, "dinos/green", position) { }
    }

    public class Yellow : Dino
    {
        public Yellow(ContentManager content, IsoBlockyMappy map, Vector2 position) 
            : base(content, map, "dinos/yellow", position) { }
    }
}
