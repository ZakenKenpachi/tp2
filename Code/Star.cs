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
    class Star
    {
        //Duration de l'effet de l'étoile.
        public static int STAR_ACTIVATION_DURATION = 10;

        //Durée de vie d'une étoile dans le jeu.
        public static int STAR_VISIBILITY_DURATION = 10;
        //Variable qui connait si l'étoile a été ramassée.
        public bool heroHasPickedUpStar = true;
        //Création de la sprite pour l'étoile.
        private Sprite starSprite = null;
        //Création de la texture de l'étoile.
        private Texture starTexture = new Texture("Arts//Star.bmp");
        //Instance du jeu
        private Game game = Game.GetInstance();
        //Utilisation de la grille.
        private Grid aMaze = new Grid();
        //Chronomètre du temps d'invulnérabilité.
        private Clock invulnerabilityTimer = new Clock();

        /// <summary>
        /// Constructeur de la class Star.
        /// </summary>
        public Star()
        {
            starSprite = new Sprite(starTexture);
        }
        /// <summary>
        /// Fonction qui active le pouvoir de l'étoile et qui recommence le chronomètre.
        /// </summary>
        public void ActivateStar()
        {
            if (!heroHasPickedUpStar)
            {
                heroHasPickedUpStar = true;
                invulnerabilityTimer.Restart();
            }
        }
        /// <summary>
        /// Fonction qui décide si l'effet de l'étoile est activée ou pas.
        /// </summary>
        /// <returns>Retourne un booléen disant si oui ou non l'étoile est activée</returns>
        public bool IsStarActivated()
        {
            if(STAR_ACTIVATION_DURATION - (int)invulnerabilityTimer.ElapsedTime.AsSeconds() <= 0)
            {
                heroHasPickedUpStar = false;
            }
            return heroHasPickedUpStar;
        }
        /// <summary>
        /// Fonction qui donne la position de l'étoile.
        /// </summary>
        /// <returns></returns>
        public Vector2f GetPosition()
        {
            return starSprite.Position;
        }
        /// <summary>
        /// Fonction qui dit si l'étoile est visible sur le jeu ou pas.
        /// </summary>
        /// <returns>Retourne un booléen disant que l'étoile est visible ou pas.</returns>
        public bool IsStarVisible()
        {
            if (game.GetRemainingTime() <= 115 && game.GetRemainingTime() >=85 && heroHasPickedUpStar == false)
            {
                return true;
            }
            else 
            return false;
        }
        /// <summary>
        /// Fonction qui dessinne l'étoile sur l'écran de jeu.
        /// </summary>
        /// <param name="window">La fenêtre de jeu</param>
        /// <param name="posX">La position de l'étoile en X</param>
        /// <param name="posY">La position de l'étoile en Y</param>
        public void Draw(RenderWindow window, int posX, int posY)
        {
            if (aMaze.GetMazeElementAt(posX, posY) == Element.Star)
            {
                starSprite.Position = new Vector2f(posX * Game.DEFAULT_GAME_ELEMENT_WIDTH, posY * Game.DEFAULT_GAME_ELEMENT_HEIGHT);
                window.Draw(starSprite);
            }
        }
    }
}
