
using System;
using System.Collections.Generic;
using Jypeli;
using Timer = Jypeli.Timer;

namespace Harjoitustyo
{
    public static class Program
    {
        public static void Main()
        {
            new FysiikkaTetrisGame().Run();
        }
    }

    public class FysiikkaTetrisGame : PhysicsGame
    {
        public override void Begin()
        {
            Level.Background.Color = Color.Black;
            Camera.ZoomToLevel();
            Gravity = new Vector(0, -800);
            
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

            MessageDisplay.Add("Fysiikka‑Tetris");
        }
    }
}