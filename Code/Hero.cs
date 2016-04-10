using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CMIYC
{
    public class Hero
    {
        //Variable de la sprite du héro.
        private Sprite heroSprite = null;
        //Création de la texture du hero.
        private Texture heroTexture = new Texture("Hero.bmp");
        //Position du héro.
        private Vector2i position;
        
        /// <summary>
        /// Fonction qui dessine le héros dans la fenêtre de jeu.
        /// </summary>
        /// <param name="window">La fenêtre de jeu</param>
        public void Draw(RenderWindow window)
        {
            heroSprite.Position = new Vector2f((position.X * Game.DEFAULT_GAME_ELEMENT_WIDTH),(position.Y * Game.DEFAULT_GAME_ELEMENT_HEIGHT));
            window.Draw(heroSprite);
        }
        public Vector2i GetPosition()
        {
            return position;
        }
        /// <summary>
        /// Constructeur de la classe Hero
        /// </summary>
        /// <param name="posX">Position en X du héros</param>
        /// <param name="posY">Position en Y du héros</param>
        public Hero(int posX, int posY)
        {
            heroSprite = new Sprite(heroTexture);
            position.X = posX;
            position.Y = posY;
        }
        /// <summary>
        /// Fonction qui fait bouger le héros dans la direction donnée.
        /// </summary>
        /// <param name="maze">Le tableau logique du jeu</param>
        /// <param name="direction">La direction que le héros doit bouger</param>
        public void Move(Grid maze, Direction direction)
        {
            if (direction == Direction.East) //Si la direction est vers l'est.
            {
                if (maze.GetMazeElementAt(position.X + 1, position.Y) != Element.Wall)
                {
                    maze.SetElementAt(position.X + 1,position.Y, Element.Hero);
                    maze.SetElementAt(position.X, position.Y, Element.None);
                    position.X += 1;
                }
            }
            if (direction == Direction.North) //Si la direction est vers le nord.
            {
                if (maze.GetMazeElementAt(position.X, position.Y-1 ) != Element.Wall)
                {
                    maze.SetElementAt(position.X, position.Y - 1, Element.Hero);
                    maze.SetElementAt(position.X, position.Y , Element.None);
                    position.Y -= 1;
                }
            }
            if (direction == Direction.West) //Si la direction est vers l'ouest.
            {
                if (maze.GetMazeElementAt(position.X-1, position.Y) != Element.Wall)
                {
                    maze.SetElementAt(position.X - 1, position.Y, Element.Hero);
                    maze.SetElementAt(position.X, position.Y, Element.None);
                    position.X -= 1;
                }
            }
            if (direction == Direction.South) //Si la direction est vers le sud.
            {
                if (maze.GetMazeElementAt(position.X, position.Y + 1) != Element.Wall)
                {
                    maze.SetElementAt(position.X, position.Y + 1, Element.Hero);
                    maze.SetElementAt(position.X, position.Y, Element.None);
                    position.Y += 1;
                }
            }
        }
        /// <summary>
        /// Donne la position du héros en X.
        /// </summary>
        /// <returns>Un entier avec la valeur de la position en X.</returns>
        public int GetX() 
        {
            return position.X;
        }
        /// <summary>
        /// Donne la position du héros en Y.
        /// </summary>
        /// <returns>Un entier avec la valeur dela position en Y</returns>
        public int GetY() 
        {
            return position.Y;
        }
        /// <summary>
        /// Fonction qui change la position en X du héros.
        /// </summary>
        /// <param name="valeurX">Nouvelle position en X</param>
        public void SetX(int valeurX)
        {
            position.X = valeurX;
        }
        /// <summary>
        /// Fonction qui change la position en Y du héros.
        /// </summary>
        /// <param name="valeurX">Nouvelle position en Y</param>
        public void SetY(int valeurY)
        {
            position.Y = valeurY;
        }
    }
}
