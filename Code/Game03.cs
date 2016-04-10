using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CMIYC
{
    /// <summary>
    /// Représente le concept de partie
    /// </summary>
    public class Game
    {

        // Taille en largeur de la fenêtre de jeu
        public const int GAME_WIDTH = 544;
        // Taille en hauteur de la fenêtre de jeu
        public const int GAME_HEIGHT = 608;
        // Nombre de mises à jour par seconde
        public const int FRAMES_PER_SECOND = 120;
        // Taille en hauteur d'une pièce de jeu.
        public const int DEFAULT_GAME_ELEMENT_HEIGHT = 32;
        // Taille en largeur d'une pièce de jeu.
        public const int DEFAULT_GAME_ELEMENT_WIDTH = 32;
        // Temps de durée d'une partie.
        public const int GAME_LENGTH_IN_SECONDS = 120;
        //Période d'invincibilité du joueur au début d'une partie.
        public const int PLAYER_INVINCIBILITY_PERIOD_AT_BEGINNING = 5;
        //Nombre d'ennemie.
        private const int NB_OPPONENT = 4;
        //Le temps quand la partie commence.
        //private DateTime gameBeginTime = new DateTime(0, 0, 0, 0, 1, 30);
        //Sprite d'une case vide.
        Sprite noneSprite = null;
        //Texture d'une case vide.
        Texture noneTexture = new Texture("Arts\\None.bmp");
        //Tableau contenant les ennemis.
        private Opponent[] tabOpponent = new Opponent[NB_OPPONENT];
        //Création de l'étoile du jeu.
        private Star theStar = null;
        //Création du héro.
        private Hero hero = new Hero(8, 9);
        //Création du nom de l'instance du singleton.
        static Game instance = null;
        //Création du font pour le temps affiché.
        //private Font timeFont = new Font("arial.ttf");
        //Création de la variable text pour le temps.
        private Text timeText = null;
        //Création de la sprite pour les murs.
        private Sprite wallSprite = null;
        //Création de la texture pour les murs.
        private Texture wallTexture = new Texture("Arts\\Wall.bmp");
        //Création de l'instance de la grille de jeu.
        private Grid maze = new Grid();
        //Creation du timer.
        private Clock timer;



        //À Completer...0
        private Game()
        {
            for (int i = 0; i < tabOpponent.Length; i++)
            {
                tabOpponent[i] = new Opponent(i, maze);
            }
            timer = new Clock();
            //timeText = new Text("aaaa", timeFont);
        }
        //public DateTime GetStartTime()
        //{
        //    return gameBeginTime;
        //}
        public static Game GetInstance()
        {
            if (instance == null)
            {
                instance = new Game();
                return instance;
            }
            else return instance;
        }
        public EndGameResult Update()
        {
            //timeText.DisplayedString = "Temps restant:" + GetRemainingTime().ToString();
            StarPopUp();
            return EndGameResult.NotFinished;
        }
        public int GetRemainingTime()
        {
            return 120 - (int)timer.ElapsedTime.AsSeconds();
        }
        private void StarPopUp()
        {
            theStar = new Star(8, 9);
            if (theStar.IsStarVisible() && !theStar.IsStarActivated())
            {
                maze.SetElementAt(8, 9, Element.Star);
            }
        }
        public void Draw(RenderWindow window)
        {


            for (int i = 0; i < maze.GetWidth(); i++)
            {
                for (int j = 0; j < maze.GetHeight(); j++)
                {
                    if (maze.GetMazeElementAt(i, j) == Element.Wall)
                    {
                        wallSprite = new Sprite(wallTexture);
                        wallSprite.Position = new Vector2f(i * DEFAULT_GAME_ELEMENT_WIDTH, j * DEFAULT_GAME_ELEMENT_HEIGHT);
                        window.Draw(wallSprite);
                    }
                    if(maze.GetMazeElementAt(i,j) == Element.Opponent) 
                    {
                       for (int k = 0; k < tabOpponent.Length; k++)
                       {
                           tabOpponent[k].Draw(window);
                       }
                    }
                    if (maze.GetMazeElementAt(i, j) == Element.Hero)
                    {
                        hero.Draw(window);
                    }
                    if (maze.GetMazeElementAt(i, j) == Element.None)
                    {
                        noneSprite = new Sprite(noneTexture);
                        noneSprite.Position = new Vector2f(i * DEFAULT_GAME_ELEMENT_WIDTH, j * DEFAULT_GAME_ELEMENT_HEIGHT);
                        window.Draw(noneSprite);
                    }
                }
            }
            theStar = new Star(8, 9);
            if (theStar.IsStarVisible())
            {
                maze.SetElementAt(8, 9, Element.Star);
                if (!theStar.IsStarActivated())
                {
                    theStar.Draw(window, 8, 9);
                }

            }
            //window.Draw(timeText);
        }
        public void MoveHero(Direction direction)
        {
            if (direction == Direction.East)
            {
                if (maze.GetMazeElementAt(hero.GetPosition().X + 1, hero.GetPosition().Y) == Element.Star)
                {
                    theStar.ActivateStar();
                }
            }
            if (direction == Direction.North)
            {
                if (maze.GetMazeElementAt(hero.GetPosition().X, hero.GetPosition().Y - 1) == Element.Star)
                {
                    theStar.ActivateStar();
                }
            }
            if (direction == Direction.West)
            {
                if (maze.GetMazeElementAt(hero.GetPosition().X - 1, hero.GetPosition().Y) == Element.Star)
                {
                    theStar.ActivateStar();
                }
            }
            if (direction == Direction.South)
            {
                if (maze.GetMazeElementAt(hero.GetPosition().X, hero.GetPosition().Y + 1) == Element.Star)
                {
                    theStar.ActivateStar();
                }
            }
            hero.Move(maze, direction);
            for (int i = 0; i < tabOpponent.Length; i++)
            {
                tabOpponent[i].Update(maze, hero, theStar.IsStarActivated());
            }
        }
    }
}
