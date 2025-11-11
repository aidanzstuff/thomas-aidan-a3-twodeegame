// Include the namespaces (code libraries) you need below.
using System;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D
{
    /// <summary>
    ///     Your game code goes inside this class!
    /// </summary>
    public class Game
    {

        // Place your variables here:
        float x;
        float speed = 200;

        public static class Text;

        /// <summary>
        ///     Setup runs once before the game loop begins.
        /// </summary>
        public void Setup()
        {
            Window.SetTitle("2D Game Assignment 3");
        }
        void DrawGround(float x,float y)
        {
            // Set crosshair color
            Draw.LineColor = Color.Black;
            Draw.LineSize = 3;
            Draw.Line(x - 720, y + 370, x + 720, y + 370); // Continous line acting as the "ground"
        }

        void DrawSpike(float x, float y)
        {
            // Set crosshair color
            Draw.LineColor = Color.Black;
            Draw.FillColor = Color.Blue;
            Draw.LineSize = 3;
            Draw.Triangle(x + 90, y + 370, x + 140, y + 265, x + 195, y + 370); // Triangle acting as a "Spike"
        }

        void DrawPlayer(float x, float y)
        {
            // Set crosshair color
            Draw.LineColor = Color.Black;
            Draw.FillColor = Color.Green;
            Draw.LineSize = 3;
            Draw.Rectangle(x + 300, y + 265, x + 105, y + 105); // Square that jumps over the spikes, this is the player
        }

        /// <summary>
        ///     Update runs every frame.
        /// </summary>
        public void Update()
        {
            Window.ClearBackground(Color.OffWhite);
            // Set window size to 400x400
            Window.SetSize(720, 480);
            // 60fps for the super epic gaming PCs out there
            Window.TargetFPS = 60;

            // Move X coordinate over time at a rate of 'speed'
            x += Time.DeltaTime * speed;

            // Draw the circle
            Draw.LineSize = 3;
            Draw.FillColor = Color.Blue;
            DrawSpike(480 - x, 0);
            DrawGround(0, 0);
            DrawPlayer(0, 0);
            // Check to see if spacebar key is held down
            if (Input.IsKeyboardKeyDown(KeyboardInput.Space))
            {
                // If it is, make shape colors green
                Draw.FillColor = Color.Green;
            }
            else
            {
                // If not, make shape colors red
                Draw.FillColor = Color.Red;
            }
        }
    }

}
