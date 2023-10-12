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

        public Animation(Texture2D spritesheet, Rectangle[] frames)
        {
            _spritesheet = spritesheet;
            _frames = frames;
            _currentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            _frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameTime > 0.1f)
            {
                _currentFrame++;
                if (_currentFrame >= _frames.Length)
                {
                    _currentFrame = 0;
                }
                _frameTime = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destination)
        {
            spriteBatch.Draw(_spritesheet, destination, _frames[_currentFrame], Color.White);
        }
    }
}
