﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IsoBlockEditor
{
    public class IsoBlockyMappy
    {
        IsoBlockyTile[,] _tiles;
        MouseState _previousMouseState;
        KeyboardState _previousKeyboardState;
        Texture2D _landTexture;
        Texture2D _waterTexture;
        Texture2D _currentTexture;
        Camera _camera;
        Rectangle _playArea;
        public bool DrawPlayArea = false;

        public IsoBlockyMappy(ContentManager content, Camera camera, Rectangle playArea)
        {
            _landTexture = content.Load<Texture2D>("blocks_50x50/isometric_pixel_0014");
            _waterTexture = content.Load<Texture2D>("blocks_50x50/isometric_pixel_0064");
            _currentTexture = _landTexture;
            _camera = camera;
            _playArea = playArea;

            var (width, height) = CalculateMapDimensionsToFitPlayArea();            
            _tiles = new IsoBlockyTile[height, width];

            for (var i = 0; i < height; i++)
            {
                var offset = i % 2 == 0 ? 0 : IsoBlockyTile.ISO_HORIZONTAL_OFFSET;
                for (var j = 0; j < width; j++)
                {
                    var x = j * IsoBlockyTile.TOP_SURFACE_WIDTH + offset;
                    var y = i * IsoBlockyTile.TOP_SURFACE_HEIGHT;
                    _tiles[i, j] = new IsoBlockyTile(new Vector2(x, y));

                    // Even rows are aesthetically displeasing when they extend
                    // beyond the odd rows. But, arrays are not jagged, so we just
                    // put a null in the even rows. We can safely break out since
                    // we're on the final iteration of the row loop.
                    if (offset > 0 && j == width - 1) _tiles[i, j].IsDeadTile = true;
                }
            }
        }

        public (int, int) CalculateMapDimensionsToFitPlayArea()
        {
            // Our play area is a rectangle. Our tiles are positioned so that the 
            // position is the center of the 50x50 texture. So, if our first tile 
            // is at 0,0 on the play area, then the top left corner of the tile
            // is at -25, -25. Tiles are positioned so that the top surface is flush
            // together. The top surface is 45 wide and 12 high. To fit, we offset
            // every other row with 21 pixels too.#
            var width = _playArea.Width / IsoBlockyTile.TOP_SURFACE_WIDTH;
            var height = _playArea.Height / IsoBlockyTile.TOP_SURFACE_HEIGHT;

            // If there is a remainder, then we need to add one more tile to the
            // width or height to make sure we cover the entire play area.
            if (_playArea.Width % IsoBlockyTile.TOP_SURFACE_WIDTH != 0) width++;
            if (_playArea.Height % IsoBlockyTile.TOP_SURFACE_HEIGHT != 0) height++;

            return (width, height);
        }

        public void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();
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
            var height = _tiles.GetLength(0);
            var width = _tiles.GetLength(1);
            for (var i = 0; i < height; i++)
            {
                var offset = i % 2 == 0 ? 0 : IsoBlockyTile.ISO_HORIZONTAL_OFFSET;
                for (var j = 0; j < width; j++)
                {
                    // Even rows are aesthetically displeasing when they extend
                    // beyond the odd rows. But, arrays are not jagged, so we just
                    // put a null in the even rows. We can safely break out since
                    // we're on the final iteration of the row loop.
                    if (_tiles[i, j].IsDeadTile) continue;

                    _tiles[i, j].IsHighlighted = _tiles[i, j].TopSurfaceContains(mousePosition);

                    if (_tiles[i, j].IsActive)
                    {
                        if (_tiles[i, j].IsHighlighted)
                        {
                            if (_previousMouseState.LeftButton == ButtonState.Pressed
                            && Mouse.GetState().LeftButton == ButtonState.Released)
                            {
                                _tiles[i, j].IsSelected = !_tiles[i, j].IsSelected;
                            }
                            else if (_previousMouseState.RightButton == ButtonState.Pressed
                                && Mouse.GetState().RightButton == ButtonState.Released)
                            {
                                _tiles[i, j].IsActive = false;
                                _tiles[i, j].Texture = null;
                            }
                        }
                    }
                    else
                    {
                        if (_tiles[i, j].IsHighlighted
                            && _previousMouseState.LeftButton == ButtonState.Pressed
                            && Mouse.GetState().LeftButton == ButtonState.Released)
                        {
                            _tiles[i, j].IsActive = true;
                            _tiles[i, j].Texture = _currentTexture;
                        }
                    }
                }
            }

            _previousKeyboardState = ks;
            _previousMouseState = ms;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (DrawPlayArea)
            {
                spriteBatch.FillRectangle(_playArea, Color.Red);
            }

            var height = _tiles.GetLength(0);
            var width = _tiles.GetLength(1);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (_tiles[i, j].IsDeadTile) continue;

                    if (_tiles[i, j].IsActive)
                    {
                        if (_tiles[i, j].IsSelected)
                        {
                            spriteBatch.Draw(_tiles[i, j].Texture, _tiles[i, j].Rectangle, Color.Blue);
                        }
                        else if (_tiles[i, j].IsHighlighted)
                        {
                            spriteBatch.Draw(_tiles[i, j].Texture, _tiles[i, j].Rectangle, Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(_tiles[i, j].Texture, _tiles[i, j].Rectangle, Color.White);
                        }
                    }
                    else
                    {
                        if (_tiles[i, j].IsHighlighted)
                        {
                            spriteBatch.Draw(_currentTexture, _tiles[i, j].Rectangle, Color.White * 0.5f);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// A single "tile" in the blocky mappy. It is isometric in shape
    /// so has a diamond-shaped top surface for selecting/highlighting
    /// etc. 
    /// </summary>
    public struct IsoBlockyTile
    {
        public const int TEXTURE_WIDTH = 50;
        public const int TEXTURE_HALF_WIDTH = 25;
        public const int TEXTURE_HEIGHT = 50;
        public const int TEXTURE_HALF_HEIGHT = 25;
        public const int TOP_SURFACE_WIDTH = 42;
        public const int TOP_SURFACE_HEIGHT = 12;
        public const int ISO_HORIZONTAL_OFFSET = 21;

        public bool IsActive;
        public bool IsSelected;
        public bool IsHighlighted;
        public bool IsDeadTile;
        public Texture2D Texture;
        public Vector2 Position;
        public Rectangle Rectangle;
        (Triangle, Triangle) TopSurface;

        public IsoBlockyTile(
            Vector2 position,
            Texture2D texture = null,
            bool isActive = false,
            bool isSelected = false,
            bool isHighlighted = false,
            bool isDeadTile = false)
        {
            Position = position;
            Texture = texture;
            IsActive = isActive;
            IsSelected = isSelected;
            IsHighlighted = isHighlighted;
            IsDeadTile = isDeadTile;

            // RECTANGLE:
            // The rectangle is used for drawing the texture and potentially
            // for a more broadphase collision detection system. We'll position
            // so that the position is in the center of the rectangle.
            Rectangle = new Rectangle(
                (int)position.X - TEXTURE_HALF_WIDTH, 
                (int)position.Y - TEXTURE_HALF_HEIGHT, 
                TEXTURE_WIDTH, 
                TEXTURE_HEIGHT);

            // BOUNDS:
            // Since the top surface of the blocky tile is a diamond,
            // we need to create the bounds by creating two triangles that
            // sit on top of each other. 
            
            // First, calculate the different nodes of the full diamond.
            // These coordinates are relative to the top left corner of 
            // rectangle and are measured in pixels based on the texture.
            var top = new Vector2(Rectangle.X + 21, Rectangle.Y + 1);
            var right = new Vector2(Rectangle.X + 45, Rectangle.Y + 13);
            var bottom = new Vector2(Rectangle.X + 25, Rectangle.Y + 25);
            var left = new Vector2(Rectangle.X + 4, Rectangle.Y + 13);

            // Second, create two triangles that sit on top of each other.
            var upperTriangle = new Triangle(left, top, right);
            var lowerTriangle = new Triangle(left, bottom, right);

            // Finally, assign the two triangles to the top surface.
            TopSurface = (upperTriangle, lowerTriangle);
        }

        public bool TopSurfaceContains(Point p) =>
            TopSurface.Item1.PointInTriangle(p) || TopSurface.Item2.PointInTriangle(p);

        public bool TopSurfaceContains(Vector2 p) =>
            TopSurface.Item1.PointInTriangle(p) || TopSurface.Item2.PointInTriangle(p);
    }
}
