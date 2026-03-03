
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
        // Luodaan attribuutti palasta
        private PhysicsObject pala;
        // Samoin lattiasta
        private PhysicsObject lattia;
        
        public override void Begin()
        {
            Level.Background.Color = Color.Black;
            Camera.ZoomToLevel();
            Gravity = new Vector(0, -50);
            
            LuoUusiPala();
            LuoLattiaJaSeinat();
            
            Keyboard.Listen(Key.Left, ButtonState.Down,  ()=> Lyonti(new Vector(-100, 0)), "Liikuta vasemmalle");
            Keyboard.Listen(Key.Right, ButtonState.Down, () => Lyonti(new Vector(100, 0)), "Liikuta vasemmalle");
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

            MessageDisplay.Add("Fysiikka‑Tetris");
        }

        /// <summary>
        /// Aliohjelma, jossa tapahtuu palikkaan kohdistuvan voiman siirto
        /// </summary>
        /// <param name="voima">Voiman suuruus ja suunta jolla palikkaa "lyödään".</param>
        public void Lyonti(Vector voima)
        {
            if (pala == null) return; // Jos ei ole aktiivista palaa
            pala.Hit(voima);
        }

        
        /// <summary>
        /// Luodaan funktio, jolla pystytään luomaan uusi pala, jota pystytään myöhemmin sitten ohjaamaan.
        /// </summary>
        public void LuoUusiPala()
        {
            pala = new PhysicsObject(40, 40, Shape.Rectangle)
            {
                Color = RandomGen.NextColor(),
                Position = new Vector(0, 300),
                Tag = "pala"
            };
            
            Add(pala);
        }


        /// <summary>
        /// Luodaan funktio, jolla pystytään luomaan tetriksen "ruudukko".
        /// Koska kuitenkin nyt sen ollessa fysiikkapeli, niin luodaan staattiset objektit
        /// ,jotka ovat ruudukon seinämät. 
        /// </summary>
        public void LuoLattiaJaSeinat()
        {
            lattia = PhysicsObject.CreateStaticObject(500, 40);
            lattia.Position = new Vector(0, -300);
            lattia.Color = Color.DarkGray;
            Add(lattia);
            
            PhysicsObject vasen = PhysicsObject.CreateStaticObject(40, 600);
            vasen.Position = new Vector(-200, 0);
            vasen.Color = Color.DarkGray;

            PhysicsObject oikea = PhysicsObject.CreateStaticObject(40, 600);
            oikea.Position = new Vector(200, 0);
            oikea.Color = Color.DarkGray;

            Add(vasen);
            Add(oikea);
        }
    }
}