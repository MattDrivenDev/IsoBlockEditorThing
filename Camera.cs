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
        KeyboardState _previousKeyboardState;
        MouseState _previousMouseState;

        public Camera(Viewport viewport, Vector2 target, float zoom = 1)
        {
            Viewport = viewport;
            Target = target;
            Zoom = zoom;
        }

        public void Update(GameTime gameTime)
        {
            var ms = Mouse.GetState();
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Up)) Target.Y -= 1;
            if (ks.IsKeyDown(Keys.Down)) Target.Y += 1;
            if (ks.IsKeyDown(Keys.Left)) Target.X -= 1;
            if (ks.IsKeyDown(Keys.Right)) Target.X += 1;
            if (ms.ScrollWheelValue > _previousMouseState.ScrollWheelValue) Zoom += 0.1f;
            if (ms.ScrollWheelValue < _previousMouseState.ScrollWheelValue) Zoom -= 0.1f;
            Zoom = MathHelper.Clamp(Zoom, 1f, 5f);

            _previousKeyboardState = ks;
            _previousMouseState = ms;
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
