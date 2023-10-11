using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IsoBlockEditor
{
    public abstract class Dino
    {
        Vector2 _position;
        Texture2D _spritesheet;
        Texture2D _shadow;

        public Dino(ContentManager content, string spritesheet, Vector2 position)
        {
            _spritesheet = content.Load<Texture2D>(spritesheet);
            _shadow = content.Load<Texture2D>("dinos/shadow");
            _position = position;
        }

        public void Update(GameTime gameTime)
        {

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

            spriteBatch.Draw(_shadow, destination, source, Color.White);
            spriteBatch.Draw(_spritesheet, destination, source, Color.White);
        }
    }

    public class Red : Dino
    {
        public Red(ContentManager content, Vector2 position) : base(content, "dinos/red", position)
        {

        }
    }
}
