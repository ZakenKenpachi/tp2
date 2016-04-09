using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CMIYC
{
    public class Opponent
    {
        //Position de recherche des enemmis sans vision sur le héro.
        public Vector2i DEFAULT_UNKNOWN_POSITION = new Vector2i(10, 10);
        //Début du temps où les enemmis se sauvent.
        private DateTime fleeBeginTime = DateTime.Now;
        //Position de la dernière fois que le héro a été vu.
        private static Vector2i lastKnownPosition;
        //Temps de congélation des enemmis lorsqu'ils sont touchés avec l'étoile.
        private int nbStillFronzenFrame = 10;
        //Nombre total d'update fait.
        private int nbTotalUpdates = 0;
        //Nombre d'update avant que les enemmis bougent.
        private int nbUpdatesBeforeMove = 10;
        //Création de la Sprite de l'ennemi.
        private Sprite opponentSprite = null;
        //Création de la texture de l'ennemi.
        private Texture opponentTexture = new Texture("Opponent.bmp");
        //Position actuelle de l'enemmi.
        private Vector2i position;
        //Variable pour les besoin allétoire.
        private Random rnd = new Random();
        //à faire
        private Vector2i targetPt;

    }
}
