//Jeu de Catch me if you can, créé par Alexandre Bélisle-Huard et David Aupin.
//ABH = le code pour le nom d'Alexandre et DA = le code pour David.
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

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
        Texture noneTexture = new Texture("None.bmp");
        //Tableau contenant les ennemis.
        private Opponent[] tabOpponents = null;
        //Création de l'étoile du jeu.
        private Star theStar = null;
        //Création du héro.
        private Hero hero = new Hero(8, 9);
        //Création du nom de l'instance du singleton.
        static Game instance = null;
        //Création du font pour le temps affiché.
        private Font timeFont = new Font("comic.ttf");
        //Création de la variable text pour le temps.
        private Text timeText = null;
        //Création de la sprite pour les murs.
        private Sprite wallSprite = null;
        //Création de la texture pour les murs.
        private Texture wallTexture = new Texture("Wall.bmp");
        //Création de l'instance de la grille de jeu.
        private Grid maze = new Grid();
        //Creation du timer.
        private Clock timer;

        /// <summary>
        /// Constructeur de la classe Game.
        /// </summary>
        private Game ()
        {
            timer = new Clock();
            timeText = new Text("", timeFont);
            tabOpponents = new Opponent[NB_OPPONENT] { new Opponent(1, maze), new Opponent(1, maze), new Opponent(1, maze), new Opponent(1, maze) };
        }
        /// <summary>
        /// Singleton qui crée une instance s'il n'y en a pas ou retourne l'instance existante.
        /// </summary>
        /// <returns>Retourne l'instance de la partie.</returns>
        public static Game GetInstance ()
        {
            //Regarde s'il y a déjà une instance de créée
            if (instance == null)
            {
                instance = new Game(); //Sinon crée une nouvelle instance.
                return instance; //Retourne l'instance créée.
            }
            else return instance; //Retourne l'instance existante.
        }
        /// <summary>
        /// Fonction qui update le jeu et qui décide si la partie doit continuer ou finir.
        /// </summary>
        /// <returns>Retourne le résultat de la partie, soit gagné, perdu ou encore en jeu.</returns>
        public EndGameResult Update ()
        {
            theStar = new Star();
            timeText.DisplayedString = "Temps restant:" + GetRemainingTime().ToString();
            //Vérifie si l'étoile est visible et si elle est déjà activée.
            if (theStar.IsStarVisible() && !theStar.IsStarActivated())
            {
                maze.SetElementAt(8, 9, Element.Star);
            }
            for (int i = 0; i < tabOpponents.Length; i++)
            {
                tabOpponents[i].Update(maze, hero, theStar.IsStarActivated());
            }
            if (GetRemainingTime() == 0)
            {
                return EndGameResult.Win;
            }
            for (int i = 0; i < tabOpponents.Length; i++)
            {
                if (hero.GetPosition() == tabOpponents[i].GetPosition())
                {
                    return EndGameResult.Lost;
                }
            }
            return EndGameResult.NotFinished;
        }
        /// <summary>
        /// Fonction qui donne le temps restant à la partie.
        /// </summary>
        /// <returns>La valeur du temps restant à la partie</returns>
        public int GetRemainingTime ()
        {
            return 120 - (int)timer.ElapsedTime.AsSeconds();
        }
        /// <summary>
        /// Fonction qui gère la fin de partie.
        /// </summary>
        public void HandleEndOfGame (EndGameResult result)
        {
            string message = "";
            string title = "Partie Terminée";
            if (result == EndGameResult.Win)
            {
                message = "Bravo, vous avez survécu! Voulez-vous rejouer?";
            }
            else if (result == EndGameResult.Lost) 
            {
                message = "Vous avez été mangé! Voulez-vous rejouer?";
            }
            DialogResult question = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.No == question)
            {
                // Quitter le jeu
                System.Environment.Exit(0);
            }
            else
            {
                // Redémarrer
                InitializeNewGame();
            }
        }
        /// <summary>
        /// Initialise une nouvelle partie.
        /// </summary>
        public void InitializeNewGame() 
        {
            maze.InitFrom("Levels/maze01.txt");
            timer.Restart();
            maze.SetElementAt(8,9,Element.Hero);
            hero.SetX(8);
            hero.SetY(9);
            theStar.ActivateStar();
            maze.SetElementAt(8,10,Element.Opponent);
            maze.SetElementAt(8,8,Element.Opponent);
            maze.SetElementAt(9,9,Element.Opponent);
            maze.SetElementAt(7,9,Element.Opponent);
        }
        /// <summary>
        /// Fonction qui dessine les objets à l'écran.
        /// </summary>
        /// <param name="window">La fenètre de jeu</param>
        public void Draw (RenderWindow window)
        {
            for (int i = 0; i < maze.GetWidth(); i++)   
            {
                for (int j = 0; j < maze.GetHeight(); j++) //Deux for inbriqués pour parcourir le tableau entier.
                {
                    //Dessine les murs.
                    if (maze.GetMazeElementAt(i, j) == Element.Wall)
                    {
                        wallSprite = new Sprite(wallTexture);
                        wallSprite.Position = new Vector2f(i * DEFAULT_GAME_ELEMENT_WIDTH, j * DEFAULT_GAME_ELEMENT_HEIGHT);
                        window.Draw(wallSprite);
                    }
                    //Dessine le héros.
                    if (maze.GetMazeElementAt(i, j) == Element.Hero)
                    {
                        hero.Draw(window);
                    }
                    //Dessine les cases vides.
                    if (maze.GetMazeElementAt(i, j) == Element.None)
                    {
                        noneSprite = new Sprite(noneTexture);
                        noneSprite.Position = new Vector2f(i * DEFAULT_GAME_ELEMENT_WIDTH, j * DEFAULT_GAME_ELEMENT_HEIGHT);
                        window.Draw(noneSprite);
                    }
                    //Dessine les ennemis.
                    if (maze.GetMazeElementAt(i, j) == Element.Opponent)
                    {
                        for (int k = 0; k < tabOpponents.Length; k++)
                        {
                            tabOpponents[k].Draw(window);
                        }
                    }
                    //Dessine l'étoile.
                    if (maze.GetMazeElementAt(i,j) == Element.Star) 
                    {
                        theStar.Draw(window, i, j);
                    }
                }
            }
            window.Draw(timeText);   //Dessine le temps de la partie.
        }
        /// <summary>
        /// Fonction qui appelle la fonction pour faire bouger le héros et qui active l'étoile si le héros passe dessus.
        /// </summary>
        /// <param name="direction"></param>
        public void MoveHero (Direction direction)
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
        }
    }
}
