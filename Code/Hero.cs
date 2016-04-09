using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CMIYC
{
    public class Hero
    {
        //Variable de la sprite du héro.
        private Sprite heroSprite = null;
        //Création de la texture du hero.
        private Texture heroTexture = new Texture("Arts/Hero.bmp");
        //Position du héro.
        private Vector2i position;
        

        public void Draw(RenderWindow window)
        {
            heroSprite.Position = new Vector2f((position.X * Game.DEFAULT_GAME_ELEMENT_WIDTH),(position.Y * Game.DEFAULT_GAME_ELEMENT_HEIGHT));
            window.Draw(heroSprite);
        }
        public Vector2i GetPosition()
        {
            return position;
        }
        public Hero(int posX, int posY)
        {
            heroSprite = new Sprite(heroTexture);
            position.X = posX;
            position.Y = posY;
        }
        public void Move(Grid2 maze, Direction direction)
        {
            
            if (direction == Direction.East)
            {
                if (maze.GetMazeElementAt(position.X + 1, position.Y) != Element.Wall)
                {
                    maze.SetElementAt(position.X + 1,position.Y, Element.Hero);
                    maze.SetElementAt(position.X, position.Y, Element.None);
                    position.X += 1;
                }
            }
            if (direction == Direction.North)
            {
                if (maze.GetMazeElementAt(position.X, position.Y-1 ) != Element.Wall)
                {
                    maze.SetElementAt(position.X, position.Y - 1, Element.Hero);
                    maze.SetElementAt(position.X, position.Y , Element.None);
                    position.Y -= 1;
                }
            }
            if (direction == Direction.West)
            {
                if (maze.GetMazeElementAt(position.X-1, position.Y) != Element.Wall)
                {
                    maze.SetElementAt(position.X - 1, position.Y, Element.Hero);
                    maze.SetElementAt(position.X, position.Y, Element.None);
                    position.X -= 1;
                }
            }
            if (direction == Direction.South) 
            {
                if (maze.GetMazeElementAt(position.X, position.Y + 1) != Element.Wall)
                {
                    maze.SetElementAt(position.X, position.Y + 1, Element.Hero);
                    maze.SetElementAt(position.X, position.Y, Element.None);
                    position.Y += 1;
                }
            }
            
        }
        public void SetX(int valeurX)
        {
            position.X = valeurX;
        }
        public void SetY(int valeurY)
        {
            position.Y = valeurY;
        }
    }
}
