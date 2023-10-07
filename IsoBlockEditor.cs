using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IsoBlockEditor
{

    public class IsoBlockEditor : Game
    {
        GraphicsDeviceManager _graphics;
        Rectangle _playArea;
        SpriteBatch _spriteBatch;
        Texture2D _landTexture;
        Texture2D _waterTexture;
        IsoBlockyTile[,] _map;
        Camera _camera;
        KeyboardState _previousKeyboardState;
        MouseState _previousMouseState;
        Texture2D _currentTexture;

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

            _landTexture = Content.Load<Texture2D>("blocks_50x50/isometric_pixel_0014");
            _waterTexture = Content.Load<Texture2D>("blocks_50x50/isometric_pixel_0064");
            _currentTexture = _landTexture;

            _playArea = new Rectangle(0, 0, 100, 75);
            var widthInTiles = (_playArea.Width / IsoBlockyTile.TOP_SURFACE_WIDTH);
            var heightIntTiles = (_playArea.Height / IsoBlockyTile.TOP_SURFACE_HEIGHT);
            _map = new IsoBlockyTile[heightIntTiles, widthInTiles];

            _camera = new Camera(GraphicsDevice.Viewport, new Vector2(100, 100), 1.5f);

            var height = _map.GetLength(0);
            var width = _map.GetLength(1);

            for (var i = 0; i < height; i++)
            {
                var offset = i % 2 == 0 ? 0 : IsoBlockyTile.ISO_HORIZONTAL_OFFSET;
                for(var j = 0; j < width; j++)
                {
                    var x = j * IsoBlockyTile.TOP_SURFACE_WIDTH + offset;
                    var y = i * IsoBlockyTile.TOP_SURFACE_HEIGHT;
                    _map[i, j] = new IsoBlockyTile(new Rectangle(x, y, IsoBlockyTile.TEXTURE_WIDTH, IsoBlockyTile.TEXTURE_HEIGHT));
                }
            }
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

            if (ms.ScrollWheelValue > _previousMouseState.ScrollWheelValue
                || ms.ScrollWheelValue < _previousMouseState.ScrollWheelValue)
            {
                _currentTexture = _currentTexture == _landTexture ? _waterTexture : _landTexture;
            }

            // Get the tile that the mouse is currently over
            var height = _map.GetLength(0);
            var width = _map.GetLength(1);
            for (var i = 0; i < height; i++)
            {
                var offset = i % 2 == 0 ? 0 : IsoBlockyTile.ISO_HORIZONTAL_OFFSET;
                for (var j = 0; j < width; j++)
                {
                    var x = j * IsoBlockyTile.TOP_SURFACE_WIDTH + offset;
                    var y = i * IsoBlockyTile.TOP_SURFACE_HEIGHT;
                    _map[i, j].IsHighlighted = _map[i, j].TopSurfaceContains(mousePosition);

                    if (_map[i, j].IsActive)
                    {
                        if (_map[i, j].IsHighlighted)
                        {
                            if (_previousMouseState.LeftButton == ButtonState.Pressed
                            && Mouse.GetState().LeftButton == ButtonState.Released)
                            {
                                _map[i, j].IsSelected = !_map[i, j].IsSelected;
                            }
                            else if (_previousMouseState.RightButton == ButtonState.Pressed
                                && Mouse.GetState().RightButton == ButtonState.Released)
                            {
                                _map[i, j].IsActive = false;
                                _map[i, j].Texture = null;
                            }
                        }
                    }
                    else
                    {
                        if (_map[i, j].IsHighlighted
                            && _previousMouseState.LeftButton == ButtonState.Pressed
                            && Mouse.GetState().LeftButton == ButtonState.Released)
                        {
                            _map[i, j].IsActive = true;
                            _map[i, j].Texture = _currentTexture;
                        }
                    }
                }
            }

            _previousKeyboardState = ks;
            _previousMouseState = ms;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var height = _map.GetLength(0);
            var width = _map.GetLength(1);
            var transformation = _camera.WorldToScreen();

            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                transformMatrix: transformation,
                rasterizerState: RasterizerState.CullCounterClockwise);

            _spriteBatch.FillRectangle(_playArea, Color.Red);

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (_map[i, j].IsActive)
                    {
                        if (_map[i, j].IsSelected)
                        {
                            _spriteBatch.Draw(_map[i, j].Texture, _map[i, j].Rectangle, Color.Blue);
                        }
                        else if (_map[i, j].IsHighlighted)
                        {
                            _spriteBatch.Draw(_map[i, j].Texture, _map[i, j].Rectangle, Color.Red);
                        }
                        else
                        {
                            _spriteBatch.Draw(_map[i, j].Texture, _map[i, j].Rectangle, Color.White);
                        }
                    }
                    else
                    {
                        if (_map[i, j].IsHighlighted)
                        {
                            _spriteBatch.Draw(_currentTexture, _map[i, j].Rectangle, Color.White * 0.5f);
                        }
                    }
                }
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}