using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CMIYC
{
    class Star
    {
        //Duration de l'effet de l'étoile.
        public static int STAR_ACTIVATION_DURATION = 10;
        //Durée de vie d'une étoile dans le jeu.
        public static int STAR_VISIBILITY_DURATION = 10;
        //Variable qui connait si l'étoile a été ramassée.
        public bool heroHasPickedUpStar = false;
        //Début de l'effet de l'étoile.
        private DateTime starActivationTime = DateTime.Now;
        //Création de la sprite pour l'étoile.
        private Sprite starSprite = null;
        //Création de la texture de l'étoile.
        private Texture starTexture = new Texture("Arts/Star.bmp");
        //Instance du jeu
        private Game game = Game.GetInstance();
        //Utilisation de la grille.
        private Grid2 aMaze = new Grid2();
        private Clock invulnerabilityTimer;
        public Star(int posX, int posY)
        {
            starSprite = new Sprite(starTexture);
            aMaze.SetElementAt(posX, posY, Element.Star);
        }
        public void ActivateStar()
        {
            if (!heroHasPickedUpStar)
            {
                heroHasPickedUpStar = true;
                invulnerabilityTimer = new Clock();
            }
            if(STAR_ACTIVATION_DURATION - (int)invulnerabilityTimer.ElapsedTime.AsSeconds() < 0)
            {
                heroHasPickedUpStar = false;
            }
        }
        public bool IsStarActivated()
        {
            return heroHasPickedUpStar;
        }
        public Vector2f GetPosition()
        {
            return starSprite.Position;
        }
        public bool IsStarVisible()
        {
            if (game.GetRemainingTime() <= 115 && game.GetRemainingTime() >=85 && !heroHasPickedUpStar)
            {
                return true;
            }
            else return false;
        }
        public void Draw(RenderWindow window, int posX, int posY)
        {
            if (aMaze.GetMazeElementAt(8, 9) == Element.Star)
            {
                starSprite.Position = new Vector2f(posX * Game.DEFAULT_GAME_ELEMENT_WIDTH, posY * Game.DEFAULT_GAME_ELEMENT_HEIGHT);
                window.Draw(starSprite);
            }
        }
    }
}
