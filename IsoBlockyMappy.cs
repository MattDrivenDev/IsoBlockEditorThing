using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoBlockEditor
{
    public class IsoBlockyMappy
    {
        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }
    }

    public struct IsoBlockyTile
    {
        public const int TEXTURE_WIDTH = 50;
        public const int TEXTURE_HEIGHT = 50;
        public const int TOP_SURFACE_WIDTH = 42;
        public const int TOP_SURFACE_HEIGHT = 12;
        public const int ISO_HORIZONTAL_OFFSET = 21;

        public bool IsActive;
        public bool IsSelected;
        public bool IsHighlighted;
        public (Triangle, Triangle) TopSurface;
        public Texture2D Texture;
        public Rectangle Rectangle;

        public IsoBlockyTile(
            Rectangle rectangle, 
            Texture2D texture = null,
            bool isActive = false, 
            bool isSelected = false, 
            bool isHighlighted = false)
        {
            Rectangle = rectangle;
            Texture = texture;
            IsActive = isActive;
            IsSelected = isSelected;
            IsHighlighted = isHighlighted;

            // BOUNDS:
            // Since the top surface of the blocky tile is a diamond,
            // we need to create the bounds by creating two triangles that
            // sit on top of each other. 
            
            // First, calculate the different nodes of the full diamond.
            // These coordinates are relative to the top left corner of 
            // rectangle and are measured in pixels based on the texture.
            var top = new Vector2(rectangle.X + 21, rectangle.Y + 1);
            var right = new Vector2(rectangle.X + 45, rectangle.Y + 13);
            var bottom = new Vector2(rectangle.X + 25, rectangle.Y + 25);
            var left = new Vector2(rectangle.X + 4, rectangle.Y + 13);

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
