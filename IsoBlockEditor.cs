using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

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
        Yellow _yellow;
        TextureWindow _textureWindow;
        Texture2D _uiTexture;
        Rectangle _cursorSource;

        public IsoBlockEditor()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
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
            _map = new IsoBlockyMappy(Content, _camera, new Rectangle(0, 0, 400, 300));
            _textureWindow = new TextureWindow(Content, new Point(16, 16), _map);
            _uiTexture = Content.Load<Texture2D>("ui/transparent");
            _cursorSource = new Rectangle(27 * 18, 19 * 18, 16, 16);
            
            // We'll get a crash if we don't have a spawn point - good.
            var spawn = _map.ActiveTiles.Single();
            var spawnpoint = spawn.Position;
            _yellow = new Yellow(Content, _map, spawnpoint);

            // Focus the camera on the spawn point.
            _camera.Target = spawnpoint;
        }

        protected override void Update(GameTime gameTime)
        {
            var ms = Mouse.GetState();
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Escape)) Exit();

            _camera.Update(gameTime);
            _map.Update(gameTime);
            _yellow.Update(gameTime);
            _textureWindow.Update(gameTime);

            base.Update(gameTime);

            _previousKeyboardState = ks;
            _previousMouseState = ms;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            DrawGameWorld(gameTime);
            DrawUI(gameTime);

            base.Draw(gameTime);
        }

        private void DrawGameWorld(GameTime gameTime)
        {
            var transformation = _camera.WorldToScreen();
            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                transformMatrix: transformation,
                rasterizerState: RasterizerState.CullCounterClockwise,
                samplerState: SamplerState.PointClamp);
            _map.Draw(_spriteBatch, gameTime);
            _yellow.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
        }

        private void DrawUI(GameTime gameTime)
        {
            var cursorDestination = new Rectangle(_previousMouseState.Position, new Point(24, 24));

            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                rasterizerState: RasterizerState.CullCounterClockwise,
                samplerState: SamplerState.PointClamp);
            _textureWindow.Draw(_spriteBatch);
            _spriteBatch.Draw(_uiTexture, cursorDestination, _cursorSource, Color.Gray);
            _spriteBatch.End();
        }
    }
}