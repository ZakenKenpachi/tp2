using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace CMIYC
{
    public class Grid
    {
        //Tableau 2D contenant l'identification des elements de jeu.
        private Element[,] mazeElement;
        //Valeur numerique entiere representant le nombre de colonne.
        private int mazeHeight = 19;
        //Valeur numerique entiere representant le nombre de ligne.
        private int mazeWidth = 17;

        /// <summary>
        /// Constructeur de la classe grid.
        /// </summary>
        public Grid()
        {
            InitForm();
        }

        /// <summary>
        /// Methode appelee lorsque l'on souhaite cree la grid
        /// </summary>
        public void InitForm()
        {
            // Exemple de lecture d'un fichier texte
            string path = "Levels/maze01.txt";

            // Si le fichier existe
            if (File.Exists(path))
            {
                //Tableau 2D de char contenant les chars du fichier texte et allant etre
                //utilise plus loin pour le tableau d'element.
                char[,] contenu = new char[mazeHeight, mazeWidth];
                //Tableau de string contenant les lines du fichier texte.
                string[] lines = File.ReadAllLines(path);
                //Boucle for inbrique parcourant le tableau de lines.
                for (int i = 0; i < mazeHeight; i++)
                {
                    for (int j = 0; j < mazeWidth * 2; j++)
                    {
                        //Verifie si la valeur du j est pair (correspont au valeur char que l'on souhaite recevoir)
                        if (j % 2 == 0)
                        {
                            contenu[i, j / 2] = lines[i][j];
                            lines[i].Split(',');
                        }
                    }
                }
                //Creation du tableau d'element.
                mazeElement = new Element[mazeHeight, mazeWidth];
                //Boucle for imbrique allant etre utilise pour comparer les cases du tableau contenu et mazeElement.
                for (int i = 0; i < mazeHeight; i++)
                {
                    for (int j = 0; j < mazeWidth; j++)
                    {
                        //Identifie la valeur des cases dans le tableau de char et asigne
                        //une valeur Element dans les cases jummelles de la grid dependament
                        //du char.
                        if (contenu[i, j] == '1')
                        {
                            mazeElement[i, j] = Element.Wall;
                        }
                        if (contenu[i, j] == '0')
                        {
                            mazeElement[i, j] = Element.None;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Methode appelee lorsque l'on souhaite savoir le nombre de ligne dans la grid.
        /// </summary>
        /// <returns>Valeur numerique representant le nombre de ligne.</returns>
        public int GetHeight()
        {
            return mazeHeight;
        }

        /// <summary>
        /// Methode que l'on appel lorsque l'on souhaite savoir la valeur d'une case dans la grid.
        /// </summary>
        /// <param name="row">Valeur entiere de la ligne que l'on va chercher.</param>
        /// <param name="colonne">Valeur entiere de la colonne que l'on va chercher.</param>
        /// <returns>La valeur Element de la case chercher.</returns>
        public Element GetMazeElementAt(int colonne, int row)
        {
            //Verifie si les valeurs de row et colonne sont inclus dans le tableau.
            if (row < mazeHeight && colonne < mazeWidth)
            {
                return mazeElement[row, colonne];
            }
            else
            {
                return Element.None;
            }
        }

        /// <summary>
        /// Methode qu'on appel lorsque l'on souhaite changer la valeur d'une case de la grid.
        /// </summary>
        /// <param name="row">Valeur numerique de la ligne de la case que l'on va modifier.</param>
        /// <param name="colonne">Valeur numerique de la colonne de la case que l'on va modifier.</param>
        /// <param name="ele">Valeur de l'element que l'on souhaite remplacer le contenue de la case.</param>
        public void SetElementAt(int colonne, int row, Element ele)
        {
            //Verifie si les valeurs de row et colonne sont inclus dans le tableau.
            if (row < mazeHeight && colonne < mazeWidth)
            {
                mazeElement[row, colonne] = ele;
            }

        }

        /// <summary>
        /// Methode appelee lorsque l'on souhaite savoir le nombre de colonne dans la grid.
        /// </summary>
        /// <returns>Valeur numerique representant le nombre de colonne.</returns>
        public int GetWidth()
        {
            return mazeWidth;
        }

    }
}

