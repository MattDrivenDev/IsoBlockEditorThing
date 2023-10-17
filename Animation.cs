using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoBlockEditor
{
    public class Animation
    {
        Texture2D _spritesheet;
        Rectangle[] _frames;
        int _currentFrame;
        float _frameTime;
        bool _loop;

        public Animation(Texture2D spritesheet, Rectangle[] frames, bool loop = true)
        {
            _spritesheet = spritesheet;
            _frames = frames;
            _currentFrame = 0;
            _loop = loop;
        }

        public void Update(GameTime gameTime)
        {
            _frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameTime > 0.1f)
            {
                _currentFrame++;
                if (_loop)
                {
                    if (_currentFrame >= _frames.Length) _currentFrame = 0;
                }
                else
                {
                    if (_currentFrame >= _frames.Length) _currentFrame = _frames.Length - 1;
                }
                _frameTime = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destination, SpriteEffects effects)
        {
            spriteBatch.Draw(
                texture: _spritesheet,
                destinationRectangle: destination,
                sourceRectangle: _frames[_currentFrame],
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                effects: effects,
                layerDepth: 0);
        }
    }
}
