using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace CMIYC
{
    class Opponent
    {
        //Début du temps de jeu où les enemmis se sauvent.
        private DateTime fleeBeginTime = DateTime.Now;
        //Position de la dernière fois que le héro a été vu.
        private static Vector2i lastKnownPosition = new Vector2i();
        //Temps de congelation des enemmis lorsqu'ils sont touchés avec l'étoile.
        private int nbStillFronzenFrame = 0;
        //Nombre total d'update fait.
        private int nbTotalUpdates = 0;
        //Nombre d'update avant que les enemmis bougent.
        private int nbUpdatesBeforeMove = 10;
        //Création de la Sprite de l'ennemi.
        private Sprite opponentSprite = null;
        //Création de la texture de l'ennemi.
        private Texture opponentTexture = new Texture("Opponent.bmp");
        //Position actuelle de l'enemmi.
        private Vector2i position = new Vector2i();
        //Variable pour les besoin alléatoire.
        private Random rnd = new Random();
        //Target lorsque la fuite est engagé.
        private Vector2i targetPt = new Vector2i();

        /// <summary>
        /// Méthode appelée lorsque l'on souhaite faire afficher l'opponent en utilisation.
        /// </summary>
        /// <param name="window">Le renderwindow où les éléments seront afficher.</param>
        public void Draw(RenderWindow window)
	    {
            opponentSprite.Position = new Vector2f(GetPosition().X * Game.DEFAULT_GAME_ELEMENT_WIDTH, GetPosition().Y * Game.DEFAULT_GAME_ELEMENT_HEIGHT);
            window.Draw(opponentSprite);
	    }

        /// <summary>
        /// Méthode appelée lorsque l'on souhaite geler l'opponent.
        /// </summary>
        public void Freeze()
	    {
            nbStillFronzenFrame = 10;
	    }

        /// <summary>
        /// Méthode appelée lorsque l'on souhaite savoir la position de l'opponent.
        /// </summary>
        /// <returns>Vector2i représentant la position de l'opponent.</returns>
        public Vector2i GetPosition()
        {
            return position;
        }

        /// <summary>
        /// Méthode appelée lorsque l'on souhaite faire bouger l'opponent en utilisation.
        /// </summary>
        /// <param name="dir">La direction que l'on va bouger l'opponent.</param>
        /// <param name="maze">La grid de jeu contenant les éléments allant être modifier.</param>
        public void Move(Direction dir, Grid maze)
	    {
            //Vérifie la direction et effectué le déplacement correspondant.
            if (dir == Direction.North)
            {
                //Modification de la grille et du vecter.
                //Pas besoin de vérifier si la case est un wall puisque l'algorithme qui donne la direction le fait.
                maze.SetElementAt(position.X, position.Y, Element.None);
                position.X = position.X - 1;
                maze.SetElementAt(position.X, position.Y, Element.Opponent);
            }
            if (dir == Direction.South)
            {
                maze.SetElementAt(position.X, position.Y, Element.None);
                position.X = position.X + 1;
                maze.SetElementAt(position.X, position.Y, Element.Opponent);
            }
            if (dir == Direction.East)
            {
                maze.SetElementAt(position.X, position.Y, Element.None);
                position.Y = position.Y - 1;
                maze.SetElementAt(position.X, position.Y, Element.Opponent);
            }
            if (dir == Direction.West)
            {
                maze.SetElementAt(position.X, position.Y, Element.None);
                position.Y = position.Y + 1;
                maze.SetElementAt(position.X, position.Y, Element.Opponent);
            }
	    }

        /// <summary>
        /// Constructeur de la classe.
        /// </summary>
        /// <param name="numero">Valeur numérique entière de 1 à 4</param>
        /// <param name="maze">La grid de jeu contennant les élements.</param>
        public void Opponnent(int numero, Grid maze)
	    {
            if (numero == 1)
            {
                position.Y = 9;
                position.X = 9;
                maze.SetElementAt(position.X, position.Y, Element.Opponent);
            }
            if (numero == 2)
            {
                position.Y = 10;
                position.X = 8;
                maze.SetElementAt(position.X, position.Y, Element.Opponent);
            }
            if (numero == 3)
            {
                position.Y = 10;
                position.X = 10;
                maze.SetElementAt(position.X, position.Y, Element.Opponent);
            }
            if (numero == 4)
            {
                position.Y = 9;
                position.X = 11;
                maze.SetElementAt(position.X, position.Y, Element.Opponent);
            }
            opponentSprite = new Sprite(opponentTexture);
	    }

        /// <summary>
        /// Méthode qui modifie le vecteur position avec le paramètre entré.
        /// </summary>
        /// <param name="pos">Le nouveau vecteur2i.</param>
        public void SetPosition(Vector2i pos)
	    {
            position = pos;
	    }

        /// <summary>
        /// Méthode appelée lorsque l'on souhaite updater l'opponent.
        /// </summary>
        /// <param name="maze">La grid de jeu contenant les éléments.</param>
        /// <param name="hero">Le hero ayant été généré.</param>
        /// <param name="isStarActivated">Valeur boolène qui détermine si la star est activée.</param>
        public void Update(Grid maze, Hero hero, bool isStarActivated)
	    {
            //Verifie si le nombre d'update avant que l'on puisse bouger est plus grand que 0.
            if (nbUpdatesBeforeMove > 0)
            {
                //Enlève 1 au nbUpdatesBeforeMove.
                nbUpdatesBeforeMove = nbUpdatesBeforeMove - 1;
            }
            //Si le nombre d'update avant que l'on puisse bouger est égale à 0.
            else
            {
                //Vérifie si le nbStillFronzenFrame est plus grand que 0, cela veut dire que l'on ne bouge pas.
                if (nbStillFronzenFrame > 0)
                {
                    //Enlève 1 au nbStillFronzenFrame
                    nbStillFronzenFrame = nbStillFronzenFrame - 1;
                }
                //Si l'opponent n'est pas geler.
                else
                {
                    //Activation du mode fuite si la star est activée.
                    if (isStarActivated == true)
                    {
                        fleeBeginTime = DateTime.Now;
                        //Nouvelle coordonnée x de la fuite.
                        int newX = rnd.Next(0, 17);
                        //Nouvelle coordonnée y de la fuite.
                        int newY = rnd.Next(0, 19);
                        //New vector targetPt représentant le mode fuite.
                        targetPt = new Vector2i(newX, newY);
                        Direction dir = PathFinder.FindShortestPath(maze, position.X, position.Y, targetPt.X, targetPt.Y);
                        Move(dir, maze);
                    }
                    //Si la star n'est pas activée.
                    else
                    {
                        //Direction selon la fonction HaveADirectViewOnTarget, si la fonction retourne autre chose que undefined.
                        Direction dir = HaveADirectViewOnTarget(hero.GetPosition(), maze);
                        //Si la fonction retourne autre chose que undefined.
                        if (dir != Direction.Undefined)
                        {
                            //lastKnowPosition devien la position du hero puisque l'on a une vue direct sur le hero.
                            lastKnownPosition = hero.GetPosition();
                            //Déplacement en utilisant la direction de la view.
                            Move(dir, maze);
                        }
                        //Si la fonction n'a pas une vue direct sur le hero.
                        else
                        {
                            //Utilisation du lastKnowPosition pour trouver la direction.
                            dir = PathFinder.FindShortestPath(maze, position.X, position.Y, targetPt.X, targetPt.Y);
                            //Déplacement en utilisant la direction de l'algorithme récursif.
                            Move(dir, maze);
                        }
                       
                    }
                }
            }
            //Ajoute 1 au nombre d'update totale.
            nbTotalUpdates++;
	    }

        /// <summary>
        /// Fonction appelée lorsque l'on souhaite obtenir la direction d'une vue direct sur le hero par l'opponent.
        /// </summary>
        /// <param name="target">Le verteur allant être utiliser par les vérifications de ligne et colonne.</param>
        /// <param name="maze">La grid de jeu.</param>
        /// <returns>Direction: La direction du déplacement à faire si on a une vue direct sur le hero.</returns>
        private Direction HaveADirectViewOnTarget(Vector2i target, Grid maze)
	    {
            //Si le hero ce trouve sur une ligne plus haute mais dans la même colonne.
            if (target.X < position.X && target.Y == position.Y)
            {
                int wallPresent = 0;
                //i = target.X + 1 parce que l'on cherche les cases entre le hero et l'opponnet.
                //Boucle qui inspecte les case
                for (int i = target.X + 1; i < position.X; i++)
                {
                    //Inspect les éléments des lignes i à la colonne position.Y
                    if (maze.GetMazeElementAt(i, position.Y) == Element.Wall)
                        {
                            //Ajoute 1 s'il presence d'une wall dans la ligne.
                            wallPresent++;
                        }
                }
                //Retourne la direction s'il n'y a pas eu de wall détecter entre les deux cases.
                if (wallPresent ==0)
                {
                    return Direction.North;
                }
                //Retourne undefined s'il y a 1 ou des walls détecter entre les deux cases.
                else
                {
                    return Direction.Undefined;
                }
            }
            if (target.X > position.X && target.Y == position.Y)
            {
                int wallPresent = 0;
                for (int i = position.X + 1; i < target.X; i++)
                {
                    if (maze.GetMazeElementAt(i, position.Y) == Element.Wall)
                    {
                        wallPresent++;
                    }
                }
                if (wallPresent == 0)
                {
                    return Direction.South;
                }
                else
                {
                    return Direction.Undefined;
                }
            }
            if (target.X == position.X && target.Y < position.Y)
            {
                int wallPresent = 0;
                for (int i = target.Y + 1; i < position.Y; i++)
                {
                    if (maze.GetMazeElementAt(position.X, i) == Element.Wall)
                    {
                        wallPresent++;
                    }
                }
                if (wallPresent == 0)
                {
                    return Direction.West;
                }
                else
                {
                    return Direction.Undefined;
                }
            }
            if (target.X == position.X && target.Y > position.Y)
            {
                int wallPresent = 0;
                for (int i = position.Y + 1; i < target.Y; i++)
                {
                    if (maze.GetMazeElementAt(position.X, i) == Element.Wall)
                    {
                        wallPresent++;
                    }
                }
                if (wallPresent == 0)
                {
                    return Direction.East;
                }
                else
                {
                    return Direction.Undefined;
                }
            }
            else
            {
                return Direction.Undefined;
            }
	    }
    }
}
