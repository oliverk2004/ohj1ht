
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
        // Luodaan julkinen attribuutti palasta
        private PhysicsObject pala;
        
        public override void Begin()
        {
            Level.Background.Color = Color.Black;
            Camera.ZoomToLevel();
            Gravity = new Vector(0, -800);
            
            luoUusiPala();
            
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

            MessageDisplay.Add("Fysiikka‑Tetris");
        }

        /// <summary>
        /// Luodaan funktio, jolla pystytään luomaan uusi pala, jota pystytään myöhemmin sitten ohjaamaan.
        /// </summary>
        public void luoUusiPala()
        {
            pala = new PhysicsObject(40, 40, Shape.Rectangle)
            {
                Color = RandomGen.NextColor(),
                Position = new Vector(0, 300),
                Tag = "pala"
            };
            
            Add(pala);
        }
    }
}