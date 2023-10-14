using Microsoft.Xna.Framework;
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
        Red _red;

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
            _map = new IsoBlockyMappy(Content, _camera, new Rectangle(0, 0, 400, 300));

            // We'll get a crash if we don't have a spawn point - good.
            var spawn = _map.ActiveTiles.Single();
            var spawnpoint = spawn.Position;
            _red = new Red(Content, _map, spawnpoint);

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
            _red.Update(gameTime);

            base.Update(gameTime);

            _previousKeyboardState = ks;
            _previousMouseState = ms;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var transformation = _camera.WorldToScreen();

            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                transformMatrix: transformation,
                rasterizerState: RasterizerState.CullCounterClockwise,
                samplerState: SamplerState.PointClamp);

            _map.Draw(_spriteBatch, gameTime);
            _red.Draw(_spriteBatch, gameTime);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}