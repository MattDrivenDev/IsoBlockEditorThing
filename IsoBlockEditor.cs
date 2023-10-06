﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IsoBlockEditor
{
    public struct Triangle
    {
        public Vector2 A;
        public Vector2 B;
        public Vector2 C;

        public Triangle(Vector2 a, Vector2 b, Vector2 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public bool PointInTriangle(Point p) => PointInTriangle(p.ToVector2());

        public bool PointInTriangle(Vector2 p)
        {
            var v0 = C - A;
            var v1 = B - A;
            var v2 = p - A;
            var dot00 = Vector2.Dot(v0, v0);
            var dot01 = Vector2.Dot(v0, v1);
            var dot02 = Vector2.Dot(v0, v2);
            var dot11 = Vector2.Dot(v1, v1);
            var dot12 = Vector2.Dot(v1, v2);
            var invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            var u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            var v = (dot00 * dot12 - dot01 * dot02) * invDenom;
            return (u >= 0) && (v >= 0) && (u + v < 1);
        }
    }

    public struct Tile
    {
        public bool IsHighlighted;
        public (Triangle, Triangle) Bounds;
        public Rectangle Rectangle;

        public bool PointInTile(Point p) => PointInTile(p.ToVector2());

        public bool PointInTile(Vector2 p)
        {
            return Bounds.Item1.PointInTriangle(p) || Bounds.Item2.PointInTriangle(p);
        }
    }

    public class IsoBlockEditor : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Texture2D _tileTexture;
        Tile[,] _map;
        Matrix _transformation = Matrix.Identity * Matrix.CreateScale(2.5f);

        public IsoBlockEditor()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _map = new Tile[30, 10];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var height = _map.GetLength(0);
            var width = _map.GetLength(1);

            for (var i = 0; i < height; i++)
            {
                var offset = i % 2 == 0 ? 0 : 21;
                for(var j = 0; j < width; j++)
                {
                    var x = j * 42 + offset;
                    var y = i * 12;

                    _map[i, j] = new Tile
                    {
                        IsHighlighted = false,
                        Rectangle = new Rectangle(x, y, 50, 50),
                        Bounds = (
                            new Triangle(new Vector2(x + 4, y + 13), new Vector2(x + 24, y + 1), new Vector2(x + 45, y + 13)),
                            new Triangle(new Vector2(x + 4, y + 13), new Vector2(x + 25, y + 25), new Vector2(x + 45, y + 13)))
                    };
                }
            }

            _tileTexture = Content.Load<Texture2D>("blocks_50x50/isometric_pixel_0000");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var mousePosition = Mouse.GetState().Position.ToVector2();
            var inverse = Matrix.Invert(_transformation);
            mousePosition = Vector2.Transform(mousePosition, inverse);

            // Get the tile that the mouse is currently over
            var height = _map.GetLength(0);
            var width = _map.GetLength(1);
            for (var i = 0; i < height; i++)
            {
                var offset = i % 2 == 0 ? 0 : 21;
                for (var j = 0; j < width; j++)
                {
                    var x = j * 42 + offset;
                    var y = i * 12;
                    _map[i, j].IsHighlighted = _map[i, j].PointInTile(mousePosition);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var height = _map.GetLength(0);
            var width = _map.GetLength(1);

            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                transformMatrix: _transformation,
                rasterizerState: RasterizerState.CullCounterClockwise);

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (_map[i, j].IsHighlighted)
                    {
                        _spriteBatch.Draw(_tileTexture, _map[i, j].Rectangle, Color.Green);
                    }
                    else
                    {
                        _spriteBatch.Draw(_tileTexture, _map[i, j].Rectangle, Color.White);
                    }
                }
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}