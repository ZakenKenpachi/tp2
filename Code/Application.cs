using System;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CMIYC
{
    public class Application
    {
        private RenderWindow window = null;
        private Color backgroundColor = new Color(160, 160, 160);

        private void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
        void OnKeyPressed(object sender, KeyEventArgs e)
        {
            // A COMPLETER SELON LES BESOINS
            if (e.Code == Keyboard.Key.Left)
            {
                Game.GetInstance().MoveHero(Direction.West);
            }
            else if (e.Code == Keyboard.Key.Right)
            {
                Game.GetInstance().MoveHero(Direction.East);
            }
            else if (e.Code == Keyboard.Key.Up)
            {
                Game.GetInstance().MoveHero(Direction.North);
            }
            else if (e.Code == Keyboard.Key.Down)
            {
                Game.GetInstance().MoveHero(Direction.South);
            }
        }
        public Application(string windowTitle, uint width, uint height)
        {
            window = new RenderWindow(new SFML.Window.VideoMode(width, height), windowTitle);
            window.Closed += new EventHandler(OnClose);
            window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);
            window.SetFramerateLimit(Game.FRAMES_PER_SECOND);
        }



        string[] paths = new string[] { "Levels\\maze01.txt" };
        private int currentLevel = 0;
        public void Run()
        {
            window.SetActive();

            bool gameIsOver = false;
            // Game.GetInstance( ).InitializeGame( paths[currentLevel] );
            while (window.IsOpen && !gameIsOver)
            {

                EndGameResult result = Game.GetInstance().Update();
                if (result == EndGameResult.Win)
                {
                    // Signaler la victoire
                    Game.GetInstance().HandleEndOfGame(result);
                }
                else if (result == EndGameResult.Lost)
                {
                    // Signaler la défaite
                    Game.GetInstance().HandleEndOfGame(result);
                }
                else
                {
                    // Effectuer le rendu
                    window.Clear(backgroundColor);
                    window.DispatchEvents();
                    Game.GetInstance().Draw(window);
                    window.Display();
                }
            }
        }
    }
}
