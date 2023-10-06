using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IsoBlockEditor
{
    public class Camera
    {
        public Viewport Viewport;
        public Vector2 Target;
        public float Zoom;

        public Camera(Viewport viewport, Vector2 target, float zoom = 1)
        {
            Viewport = viewport;
            Target = target;
            Zoom = zoom;
        }

        public Matrix WorldToScreen()
        {
            return Matrix.CreateTranslation(new Vector3(-Target, 0f))
                * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1f))
                * Matrix.CreateTranslation(new Vector3(new Vector2(Viewport.Width / 2, Viewport.Height / 2), 0f));
        }

        public Matrix ScreenToWorld()
        {
            return Matrix.Invert(WorldToScreen());
        }
    }
}
