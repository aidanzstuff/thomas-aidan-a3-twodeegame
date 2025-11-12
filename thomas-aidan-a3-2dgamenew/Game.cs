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

        float playerY = 0;
        float velocityY = 0;
        float gravity = 600f;
        float jumpForce = -450f;
        bool isGrounded = true;
        bool collided = false;
        bool gameOver = false; // <-- moved here (outside Update)

        public void Setup()
        {
            Window.SetTitle("2D Game Assignment 3");
            Window.SetSize(720, 480);
            Window.TargetFPS = 60;
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

            // if game over, show message and wait for restart
            if (gameOver)
            {
                Text.Color = Color.Black;
                Text.Draw("GAME OVER", 250, 200);
                Text.Draw("Press R to Restart", 230, 240);

                if (Input.IsKeyboardKeyPressed(KeyboardInput.R))
                    RestartGame();

                return;
            }

            x += Time.DeltaTime * speed + spikespeed;

            Text.Color = Color.Black;
            Text.Draw($"Points: {points}", 480, 50);

            if (x > 480)
            {
                x = 0;
                spikespeed += 0.1f;
                points += 10;
            }

            if (Input.IsKeyboardKeyPressed(KeyboardInput.Space) && isGrounded)
            {
                velocityY = jumpForce;
                isGrounded = false;
            }

            velocityY += gravity * Time.DeltaTime;
            playerY += velocityY * Time.DeltaTime;

            if (playerY >= 0)
            {
                playerY = 0;
                velocityY = 0;
                isGrounded = true;
            }

            Vector2 triA = new Vector2(500 - x + 90, 370);
            Vector2 triB = new Vector2(500 - x + 140, 265);
            Vector2 triC = new Vector2(500 - x + 195, 370);

            float px = 300;
            float py = 270 + playerY;
            float size = 100;
            RectangleF playerRect = new RectangleF(px, py, size, size);

            collided = CheckTriangleRectangleCollision(triA, triB, triC, playerRect);

            if (collided)
                gameOver = true; // this now persists!

            DrawSpike(500 - x, 0);
            DrawGround(0, 0);
            DrawPlayer(0, 0);
        }

        void RestartGame()
        {
            x = 0;
            playerY = 0;
            velocityY = 0;
            isGrounded = true;
            collided = false;
            gameOver = false;
            points = 0;
            spikespeed = 0;
        }

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