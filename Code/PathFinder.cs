using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


/////////////////////////////////////////////////////////////////////////
/// Vous devez compléter les méthodes de ce fichier
/////////////////////////////////////////////////////////////////////////
namespace CMIYC
{

    public static class PathFinder
    {

        private static int[,] tabCosts = null;
        //<summary>
        //Détermine la direction dans laquelle un opposant doit aller pour 
        //atteindre le héros en utilisant le chemin de coût minimal
        //</summary>
        //<param name="aMaze">Le labyrinthe de jeu</param>
        //<param name="fromX">La position en X de l'opposant</param>
        //<param name="fromY">La position en Y de l'opposant</param>
        //<param name="toX">La position en X du héros</param>
        //<param name="toY">La position en Y du héros</param>
        //<returns>La direction dans laquelle l'opposant doit se déplacer ou Direction.None s'il est déjà
        //rendu à destination ou Direction.Undefined s'il n'existe pas de chemin.</returns>
        public static Direction FindShortestPath(Grid aMaze, int fromX, int fromY, int toX, int toY)
        {
            Direction retval = Direction.Undefined;
            //1) Allouer le tableau des coûts
            tabCosts = new int[aMaze.GetWidth(), aMaze.GetHeight()];

            //2) Initialiser le tableau des coûts
            for (int i = 0; i < aMaze.GetWidth(); i++)
            {
                for (int j = 0; j < aMaze.GetHeight(); j++)
                {
                    tabCosts[i, j] = int.MaxValue;
                }
            }
            //3) Calculer les coûts en lançant l'appel récursif
            tabCosts[fromX, fromY] = 0;
            ComputeCosts(aMaze, fromX, fromY, tabCosts);
            //4) Déterminer le premier déplacement
            retval = RecurseFindDirection(tabCosts, fromX, fromY, toX, toY);
            return retval;
        }

        /// <summary>
        /// Calcule le tableau des coûts
        /// </summary>
        /// <param name="aMaze">Le labyrinthe de jeu</param>
        /// <param name="fromX">La position en X de l'opposant</param>
        /// <param name="fromY">La position en Y de l'opposant</param>
        /// <param name="costs">Le tableau des coûts à remplir</param>
        public static void ComputeCosts(Grid aMaze, int fromX, int fromY, int[,] costs)
        {
            //Variable du coût de la case vérifiée.
            int origineCost = costs[fromX, fromY];
            //Vérifie si la case est dans le tableau de jeu.
            if (fromX >= 0 && fromX < costs.GetLength(0) && fromY >= 0 && fromY < costs.GetLength(1) && aMaze.GetMazeElementAt(fromX, fromY) == Element.None)
            {
                //Vérifie si la case à gauche est différent d'un mur et que sa valeur est plus petit que le cout de la case d'origine.
                if (aMaze.GetMazeElementAt(fromX, fromY - 1) != Element.Wall && costs[fromX, fromY - 1] > origineCost)
                {
                    costs[fromX, fromY - 1] = origineCost + 1;
                    ComputeCosts(aMaze, fromX, fromY - 1, costs);
                }
                if (aMaze.GetMazeElementAt(fromX, fromY + 1) != Element.Wall && costs[fromX, fromY + 1] > origineCost)
                {
                    costs[fromX, fromY + 1] = origineCost + 1;
                    ComputeCosts(aMaze, fromX, fromY + 1, costs);
                }
                if (aMaze.GetMazeElementAt(fromX - 1, fromY) != Element.Wall && costs[fromX - 1, fromY] > origineCost)
                {
                    costs[fromX - 1, fromY] = origineCost + 1;
                    ComputeCosts(aMaze, fromX - 1, fromY, costs);
                }
                if (aMaze.GetMazeElementAt(fromX + 1, fromY) != Element.Wall && costs[fromX + 1, fromY] > origineCost)
                {
                    costs[fromX + 1, fromY] = origineCost + 1;
                    ComputeCosts(aMaze, fromX + 1, fromY, costs);
                }
            }

        }
        /// <summary>
        /// Parcourt le tableau de couts pour trouver le premier deplacement a faire.
        /// </summary>
        /// <param name="costs">La tableau de couts, le tableau des couts contient toujours une bordure de valeur infini</param>
        /// <param name="fromX">La position en X ou se trouve le heros</param>
        /// <param name="fromY">La position en Y ou se trouve le heros</param>
        /// <param name="toX">La position en X ou se trouve l'opposant</param>
        /// <param name="toY">La position en Y ou se trouve l'opposant</param>
        /// <returns>La direction dans laquelle l'opposant doit se deplacer ou Direction.None s'il est deja
        /// rendu a destination ou Direction.Undefined s'il n'existe pas de chemin.</returns>
        private static Direction RecurseFindDirection(int[,] costs, int fromX, int fromY, int toX, int toY)
        {
            //Verifie si les valeurs de x et de y que l'on va utiliser dans les verification sont dans le tableau.
            if (toX >= costs.GetLength(0) && toY >= costs.GetLength(1))
            {
                return Direction.Undefined;
            }
            //Verifie si les valeurs de x et de y de l'opponent sont egale aux valeurs x et y du hero.
            if (fromX == toX && fromY == toY)
            {
                return Direction.None;
            }
            //Effectue d'autre instructions si l'algorithme est rendu a la case d'une valeur
            //de 1 (cela veut dire que l'on est proche d'une case 0 et de la destination).
            if (costs[toX, toY] == 1)
            {
                //Verifie si la case en haut est egale a 0 (case destination) et retourne la direction inverse si vrai.
                if (costs[toX - 1, toY] == 0)
                {
                    return Direction.South;
                }
                //Verifie si la case en bas est egale a 0 (case destination) et retourne la direction inverse si vrai.
                if (costs[toX + 1, toY] == 0)
                {
                    return Direction.North;
                }
                //Verifie si la case a gauche est egale a 0 (case destination) et retourne la direction inverse si vrai.
                if (costs[toX, toY - 1] == 0)
                {
                    return Direction.West;
                }
                //Verifie si la case a droite est egale a 0 (case destination) et retourne la direction inverse si vrai.
                if (costs[toX, toY + 1] == 0)
                {
                    return Direction.East;
                }
                //Si la case du moment egale a 1 mais aucune case la longent egale a 0 (ne devrait jamais arriver).
                else
                {
                    return Direction.Undefined;
                }
            }
            else
            {
                //Verifie si la case en haut possede une valeur plus basse que la case du moment.
                if (costs[toX - 1, toY] < costs[toX, toY])
                {
                    //Modification de la valeur toX pour qu'elle corresponde a la valeur de
                    //case superieur, ce qui va "bouger" l'algoritme vers le haut.
                    return RecurseFindDirection(costs, fromX, fromY, toX - 1, toY);
                }
                //Verifie si la case en bas possede une valeur plus basse que la case du moment.
                if (costs[toX + 1, toY] < costs[toX, toY])
                {
                    //Modification de la valeur toX pour qu'elle corresponde a la valeur de
                    //case inferieur, ce qui va "bouger" l'algoritme vers le bas.
                    return RecurseFindDirection(costs, fromX, fromY, toX + 1, toY);
                }
                //Verifie si la case de gauche possede une valeur plus basse que la case du moment.
                if (costs[toX, toY - 1] < costs[toX, toY])
                {
                    //Modification de la valeur toY pour qu'elle corresponde a la valeur de
                    //case de gauche, ce qui va "bouger" l'algoritme vers la gauche.
                    return RecurseFindDirection(costs, fromX, fromY, toX, toY - 1);
                }
                //Verifie si la case de droite possede une valeur plus basse que la case du moment.
                if (costs[toX, toY + 1] < costs[toX, toY])
                {
                    //Modification de la valeur toY pour qu'elle corresponde a la valeur de
                    //case de droite, ce qui va "bouger" l'algoritme vers la droite.
                    return RecurseFindDirection(costs, fromX, fromY, toX, toY + 1);
                }
                //Si aucun deplacement est possible mais que l'on a toujours pas atteint la destination.
                else
                {
                    return Direction.Undefined;
                }
            }
        }
    }
}
