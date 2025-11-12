using System;
using System.Drawing;
using System.Numerics;

namespace MohawkGame2D
{
    public class Game
    {
        float x;
        float speed = 200;
        float spikespeed = 0;
        int points = 0;

        // --- Jump and physics variables ---
        float playerY = 0;          // Player vertical position offset
        float velocityY = 0;        // Vertical speed
        float gravity = 600f;       // Pulls player back down
        float jumpForce = -450f;    // Negative because up is decreasing Y
        bool isGrounded = true;     // True when on the ground

        bool collided = false;

        public void Setup()
        {
            Window.SetTitle("2D Game Assignment 3");
            Window.SetSize(720, 480);
            Window.TargetFPS = 60;

            // initialize the text system (loads default fonts)
            Text.Initialize();

            // optional default styling
            Text.Size = 24;            // text size
            Text.Color = Color.Black;  // color for text
                                       // Load your font
            Font myFont = Text.LoadFont("assets/fonts/Coolveticarg.otf", 32);

            // Set as the default font for drawing text
            Text.Font = myFont;
        }

        void DrawGround(float x, float y)
        {
            Draw.LineColor = Color.Black;
            Draw.LineSize = 3;
            Draw.Line(x - 720, y + 370, x + 720, y + 370);
        }

        void DrawSpike(float x, float y)
        {
            Draw.LineColor = Color.Black;
            Draw.FillColor = Color.Blue;
            Draw.LineSize = 3;
            Draw.Triangle(x + 90, y + 370, x + 140, y + 265, x + 195, y + 370);
        }

        void DrawPlayer(float x, float y)
        {
            Draw.LineColor = Color.Black;
            Draw.FillColor = collided ? Color.Red : Color.Green;
            Draw.LineSize = 3;
            Draw.Square(x + 300, y + 270 + playerY, 100);
        }

        public void Update()
        {
            Window.ClearBackground(Color.OffWhite);
            bool gameOver = false;
            x += Time.DeltaTime * speed + spikespeed;

            // If game is over, show "Game Over" text and stop the game
            if (gameOver)
            {
                Text.Draw("GAME OVER", 250, 200, Text.Font);
                Text.Draw("Press R to Restart", 230, 240, Text.Font);

                // Optionally restart
                if (Input.IsKeyboardKeyPressed(KeyboardInput.R))
                {
                    RestartGame();
                }

                return; // skip the rest of Update
            }

            // When spike moves off-screen, reset
            if (x > 480) // adjust this number based on when it fully leaves the screen
            {
                x = 0;
                spikespeed += (float)0.1;
                points += 10;
            }

            // --- Handle jumping ---
            if (Input.IsKeyboardKeyPressed(KeyboardInput.Space) && isGrounded)
            {
                velocityY = jumpForce;
                isGrounded = false;
            }

            // Apply gravity
            velocityY += gravity * Time.DeltaTime;
            playerY += velocityY * Time.DeltaTime;

            // Ground collision (stop falling)
            if (playerY >= 0)
            {
                playerY = 0;
                velocityY = 0;
                isGrounded = true;
            }

            // --- Define spike triangle positions ---
            Vector2 triA = new Vector2(500 - x + 90, 370);
            Vector2 triB = new Vector2(500 - x + 140, 265);
            Vector2 triC = new Vector2(500 - x + 195, 370);

            // --- Player rectangle ---
            float px = 300;
            float py = 270 + playerY;
            float size = 100;
            RectangleF playerRect = new RectangleF(px, py, size, size);

            // --- Collision check ---
            collided = CheckTriangleRectangleCollision(triA, triB, triC, playerRect);

            // If collision happens → end game
            if (collided)
            {
                gameOver = true;
            }

            // --- Draw all objects ---
            DrawSpike(500 - x, 0);
            DrawGround(0, 0);
            DrawPlayer(0, 0);

            void RestartGame()
            {
                x = 0;
                playerY = 0;
                velocityY = 0;
                isGrounded = true;
                collided = false;
                gameOver = false;
            }
        }

        // === Collision Functions ===
        bool CheckTriangleRectangleCollision(Vector2 a, Vector2 b, Vector2 c, RectangleF rect)
        {
            if (rect.Contains(a.X, a.Y) || rect.Contains(b.X, b.Y) || rect.Contains(c.X, c.Y))
                return true;

            Vector2[] corners =
            {
                new Vector2(rect.Left, rect.Top),
                new Vector2(rect.Right, rect.Top),
                new Vector2(rect.Right, rect.Bottom),
                new Vector2(rect.Left, rect.Bottom)
            };

            foreach (var p in corners)
                if (PointInTriangle(p, a, b, c))
                    return true;

            return false;
        }

        bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            float denominator = ((b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y));
            float alpha = ((b.Y - c.Y) * (p.X - c.X) + (c.X - b.X) * (p.Y - c.Y)) / denominator;
            float beta = ((c.Y - a.Y) * (p.X - c.X) + (a.X - c.X) * (p.Y - c.Y)) / denominator;
            float gamma = 1.0f - alpha - beta;
            return alpha >= 0 && beta >= 0 && gamma >= 0;
        }
}
}