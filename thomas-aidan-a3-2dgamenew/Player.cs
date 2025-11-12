using MohawkGame2D;
using System;
using System.Numerics;

namespace MohawkGame2D;

public class Player
{
    // Variables local to this class
    public Color skin;
    public Color cheek;
    public float x;
    public float y;

    public void Render()
    {
        // Set crosshair color
        Draw.LineColor = Color.Black;
        Draw.FillColor = Color.Yellow;
        Draw.LineSize = 3;
        Draw.Rectangle(300, 265, 105, 105); // Square that jumps over the spikes, this is the player
    }
}