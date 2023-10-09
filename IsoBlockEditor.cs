using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IsoBlockEditor
{
    public class IsoBlockEditor : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Camera _camera;
        KeyboardState _previousKeyboardState;
        MouseState _previousMouseState;
        IsoBlockyMappy _map;

        public IsoBlockEditor()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _previousKeyboardState = Keyboard.GetState();
            _previousMouseState = Mouse.GetState();

            base.Initialize();
        }
            
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new Camera(GraphicsDevice.Viewport, new Vector2(100, 100), 1.5f);
            _map = new IsoBlockyMappy(Content, _camera, new Rectangle(0, 0, 100, 75));
            _map.DrawPlayArea = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Up))
            {
                _camera.Target.Y -= 1;
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                _camera.Target.Y += 1;
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                _camera.Target.X -= 1;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                _camera.Target.X += 1;
            }
            if (ks.IsKeyDown(Keys.Subtract))
            {
                _camera.Zoom -= 0.1f;
            }
            if (ks.IsKeyDown(Keys.Add))
            {
                _camera.Zoom += 0.1f;
            }

            var ms = Mouse.GetState();
            var mousePosition = ms.Position.ToVector2();
            var inverse = _camera.ScreenToWorld();
            mousePosition = Vector2.Transform(mousePosition, inverse);

            _map.Update(gameTime);

            _previousKeyboardState = ks;
            _previousMouseState = ms;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var transformation = _camera.WorldToScreen();

            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                transformMatrix: transformation,
                rasterizerState: RasterizerState.CullCounterClockwise);

            _map.Draw(_spriteBatch, gameTime);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}