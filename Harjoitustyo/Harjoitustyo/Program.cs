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

    /// <summary>
    /// FysiikkaTetris -peli, joka muistuttaa normaalia tetristä, mutta on hieman erilainen
    /// </summary>
    public class FysiikkaTetrisGame : PhysicsGame
    {
        // Aktiivinen ohjattava pala
        private PhysicsObject? pala;

        // Lattia (staattinen)
        private PhysicsObject lattia;
        
        // Luodaan pistetaulu
        private Label pisteTaulu;

        public override void Begin()
        {
            Level.Background.Color = Color.Black;
            Camera.ZoomToLevel();
            Gravity = new Vector(0, -100);

            LuoLattiaJaSeinat();
            LuoPisteTaulu();
            LuoUusiPala();

            Keyboard.Listen(Key.Left,  ButtonState.Down, () => Lyonti(new Vector(-50, 0)), "Liikuta vasemmalle");
            Keyboard.Listen(Key.Right, ButtonState.Down, () => Lyonti(new Vector( 50, 0)), "Liikuta oikealle");
            Keyboard.Listen(Key.Space, ButtonState.Pressed, SuoraPudotus, "Pudota suoraan");
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

            MessageDisplay.Add("Fysiikka‑Tetris");
        }

        
        /// <summary>
        /// Palikkaan kohdistuvan voiman "lyönti"
        /// </summary>
        /// <param name="voima">Voiman suuruus ja suunta</param>
        public void Lyonti(Vector voima)
        {
            if (pala != null) pala.Hit(voima);
        }

        
        /// <summary>
        /// Luo uuden ohjattavan palikan ja asettaa törmäyskäsittelijät
        /// lattiaan sekä jo lukittuihin paloihin.
        /// </summary>
        public void LuoUusiPala()
        {
            pala = new PhysicsObject(70, 70, Shape.Rectangle)
            {
                Color = RandomGen.NextColor(),
                Position = new Vector(0, 300),
                Tag = "pala",
            };

            // Estetään palan kiertyminen
            pala.CanRotate = false;
            
            Add(pala);

            // Kun aktiivinen pala osuu lattiaan
            AddCollisionHandler(pala, "lattia", PalaOsuiLattiaan);

            // Kun aktiivinen pala osuu jo lukittuun palaan
            AddCollisionHandler(pala, "lukittu", PalaOsuiLukittuun);
        }

        
        /// <summary>
        /// Lattia ja seinät (staattisia objekteja).
        /// </summary>
        public void LuoLattiaJaSeinat()
        {
            lattia = PhysicsObject.CreateStaticObject(500, 40);
            lattia.Position = new Vector(0, -300);
            lattia.Color = Color.DarkGray;
            lattia.Tag = "lattia";
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
        
        
        /// <summary>
        /// Kutsutaan, kun aktiivinen pala osuu lattiaan.
        /// </summary>
        /// <param name="tormaaja">Palikka, joka osuu lattiaan.</param>
        /// <param name="lattiaObj">Staattinen lattiaobjekti.</param>
        private void PalaOsuiLattiaan(PhysicsObject tormaaja, PhysicsObject lattiaObj)
        {
            if (tormaaja != pala) return; // käsittele vain aktiivista palaa
            LukitseJaLuoUusi(tormaaja);
        }

        
        /// <summary>
        /// Kutsutaan, kun aktiivinen pala osuu jo lukittuun palaan
        /// </summary>
        /// <param name="tormaaja">Palikka, joka osuu toiseen jo törmänneeseen palikkaan.</param>
        /// <param name="lukittuObj">Jo törmännyt palikka</param>
        private void PalaOsuiLukittuun(PhysicsObject tormaaja, PhysicsObject lukittuObj)
        {
            if (tormaaja != pala) return; // käsittele vain aktiivista palaa
            LukitseJaLuoUusi(tormaaja);
        }

        
        /// <summary>
        /// Yhteinen lukituslogiikka: pysäytä, tee staattiseksi, merkitse "lukittu",
        /// nollaa viite ja luo uusi pala pienen viiveen jälkeen.
        /// </summary>
        /// <param name="tormaaja">palikka, joka osuu joko lattiaan tai toiseen palikkaan.</param>
        private void LukitseJaLuoUusi(PhysicsObject tormaaja)
        {
            // Jos aktiivinen pala on jo nollattu
            if (pala == null) return;

            // Pysäytä ja "lukitse" pala
            tormaaja.Velocity = Vector.Zero;
            tormaaja.AngularVelocity = 0;
            tormaaja.Restitution = 0.0;
            tormaaja.MakeStatic();
            tormaaja.Tag = "lukittu";

            // Nollataan viite aktiiviseen palaan ennen uuden luontia
            pala = null;

            Timer.SingleShot(0.05, LuoUusiPala);
        }

        /// <summary>
        /// Jos pelaaja painaa välilyöntiä niin palikka tippuu nopeasti alas.
        /// </summary>
        private void SuoraPudotus()
        {
            if (pala == null)
            {
                return;
            }

            pala.Velocity = new Vector(0, -500);
        }


        private void LuoPisteTaulu()
        {
            pisteTaulu = new Label
            {
                Text = "Pisteet: 0",
                TextColor = Color.White
            };

            pisteTaulu.X = Screen.Left + 100;
            pisteTaulu.Y = Screen.Top - 100;
            
            Add(pisteTaulu);
        }
    }
}