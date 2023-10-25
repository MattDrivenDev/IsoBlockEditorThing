using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IsoBlockEditor
{
    public class TextureWindow
    {
        public const int UI_TILE_TEXTURE_SPACING = 2;
        public const int UI_TILE_WIDTH = 16;
        public const int UI_TILE_HEIGHT = 16;
        public const int UI_TILE_SIZE_MULTIPLIER = 4;

        KeyboardState _previousKeyboardState;
        MouseState _previousMouseState;
        IsoBlockyMappy _map;
        Rectangle _innerRectangle;
        bool _active;

        // Source Rectangles
        Rectangle _inactiveSource;
        Rectangle _activeSourceTL;
        Rectangle _activeSourceT;
        Rectangle _activeSourceTR;
        Rectangle _activeSourceL;
        Rectangle _activeSourceC;
        Rectangle _activeSourceR;
        Rectangle _activeSourceBL;
        Rectangle _activeSourceB;
        Rectangle _activeSourceBR;

        // Destination Rectangles
        Rectangle _inactiveDestination;
        Rectangle _activeDestinationTL;
        Rectangle _activeDestinationT;
        Rectangle _activeDestinationTR;
        Rectangle _activeDestinationL;
        Rectangle _activeDestinationC;
        Rectangle _activeDestinationR;
        Rectangle _activeDestinationBL;
        Rectangle _activeDestinationB;
        Rectangle _activeDestinationBR;

        public Texture2D Texture;

        public TextureWindow(
            ContentManager content,
            Point position,
            IsoBlockyMappy map,
            bool active = false)
        {
            Texture = content.Load<Texture2D>("ui/transparent");

            _active = active;
            _map = map;
            _innerRectangle = new Rectangle(position.X + 16, position.Y + 16, 32, 32);

            var sourceSize = new Point(UI_TILE_WIDTH, UI_TILE_HEIGHT);
            var destinationSize = new Point(UI_TILE_WIDTH * UI_TILE_SIZE_MULTIPLIER, UI_TILE_HEIGHT * UI_TILE_SIZE_MULTIPLIER);

            _inactiveSource = new Rectangle(new Point(0 * 18, 22 * 18), sourceSize);
            _activeSourceTL = new Rectangle(new Point(0 * 18, 18 * 18), sourceSize);
            _activeSourceT = new Rectangle(new Point(1 * 18, 18 * 18), sourceSize);
            _activeSourceTR = new Rectangle(new Point(2 * 18, 18 * 18), sourceSize);
            _activeSourceL = new Rectangle(new Point(0 * 18, 19 * 18), sourceSize);
            _activeSourceC = new Rectangle(new Point(1 * 18, 19 * 18), sourceSize);
            _activeSourceR = new Rectangle(new Point(2 * 18, 19 * 18), sourceSize);
            _activeSourceBL = new Rectangle(new Point(0 * 18, 20 * 18), sourceSize);
            _activeSourceB = new Rectangle(new Point(1 * 18, 20 * 18), sourceSize);
            _activeSourceBR = new Rectangle(new Point(2 * 18, 20 * 18), sourceSize);

            _inactiveDestination = new Rectangle(position, destinationSize);
            _activeDestinationTL = new Rectangle(new Point(_inactiveDestination.X, _inactiveDestination.Y), destinationSize);
            _activeDestinationT = new Rectangle(new Point(_activeDestinationTL.X + destinationSize.X, _activeDestinationTL.Y), destinationSize);
            _activeDestinationTR = new Rectangle(new Point(_activeDestinationT.X + destinationSize.X, _activeDestinationT.Y), destinationSize);
            _activeDestinationL = new Rectangle(new Point(_activeDestinationTL.X, _activeDestinationTL.Y + destinationSize.Y), destinationSize);
            _activeDestinationC = new Rectangle(new Point(_activeDestinationL.X + destinationSize.X, _activeDestinationL.Y), destinationSize);
            _activeDestinationR = new Rectangle(new Point(_activeDestinationC.X + destinationSize.X, _activeDestinationC.Y), destinationSize);
            _activeDestinationBL = new Rectangle(new Point(_activeDestinationL.X, _activeDestinationL.Y + destinationSize.Y), destinationSize);
            _activeDestinationB = new Rectangle(new Point(_activeDestinationBL.X + destinationSize.X, _activeDestinationBL.Y), destinationSize);
            _activeDestinationBR = new Rectangle(new Point(_activeDestinationB.X + destinationSize.X, _activeDestinationB.Y), destinationSize);
        }

        public void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();
            var ms = Mouse.GetState();

            if (!_active && _inactiveDestination.Contains(ms.Position))
            {
                _active = true;
            }
            else if(_active && ActiveDestinationsContain(ms.Position))
            {
                _active = true;
            }
            else 
            {
                _active = false;
            }

            _previousKeyboardState = ks;
            _previousMouseState = ms;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_active)
            {
                spriteBatch.Draw(Texture, _inactiveDestination, _inactiveSource, Color.White);
                spriteBatch.Draw(_map.CurrentTexture, _innerRectangle, Color.White);
            }
            else
            {
                spriteBatch.Draw(Texture, _activeDestinationTL, _activeSourceTL, Color.White);
                spriteBatch.Draw(Texture, _activeDestinationT, _activeSourceT, Color.White);
                spriteBatch.Draw(Texture, _activeDestinationTR, _activeSourceTR, Color.White);
                spriteBatch.Draw(Texture, _activeDestinationL, _activeSourceL, Color.White);
                spriteBatch.Draw(Texture, _activeDestinationC, _activeSourceC, Color.White);
                spriteBatch.Draw(Texture, _activeDestinationR, _activeSourceR, Color.White);
                spriteBatch.Draw(Texture, _activeDestinationBL, _activeSourceBL, Color.White);
                spriteBatch.Draw(Texture, _activeDestinationB, _activeSourceB, Color.White);
                spriteBatch.Draw(Texture, _activeDestinationBR, _activeSourceBR, Color.White);
                spriteBatch.Draw(_map.CurrentTexture, _innerRectangle, Color.White);
            }
        }

        private bool ActiveDestinationsContain(Point position)
        {
            return _activeDestinationTL.Contains(position)
                || _activeDestinationT.Contains(position)
                || _activeDestinationTR.Contains(position)
                || _activeDestinationL.Contains(position)
                || _activeDestinationC.Contains(position)
                || _activeDestinationR.Contains(position)
                || _activeDestinationBL.Contains(position)
                || _activeDestinationB.Contains(position)
                || _activeDestinationBR.Contains(position);
        }
    }
}
