using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CMIYC
{
    public class Grid
    {
        private Element[,] mazeElement;
        //Valeur numerique entiere representant le nombre de colonne.
        private int mazeHeight = 19;
        //Valeur numerique entiere representant le nombre de ligne.
        private int mazeWidth = 17;
        //Chemin du fichier text.
        string path = "Levels/maze01.txt";
         public Grid()
        {
            InitFrom(path);
        }
        public void InitFrom(string path)
        {
            
            // Si le fichier existe
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                char[,] tab2d = new char[17, 19];
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] splitTemp = lines[i].Split(',');
                    for (int j = 0; j < splitTemp.Length; j++)
                    {
                        tab2d[j,i] = splitTemp[j][0]; 
                    }
                }
                mazeElement = new Element[mazeWidth, mazeHeight];
                //Boucle for imbrique allant etre utilise pour comparer les cases du tableau contenu et mazeElement.
                for (int i = 0; i < mazeWidth; i++)
                {
                    for (int j = 0; j < mazeHeight; j++)
                    {
                        //Identifie la valeur des cases dans le tableau de char et asigne
                        //une valeur Element dans les cases jummelles de la grid dependament
                        //du char.
                        if (tab2d[i, j] == '1')
                        {
                            mazeElement[i, j] = Element.Wall;
                        }
                        if (tab2d[i, j] == '0')
                        {
                            mazeElement[i, j] = Element.None;
                        }
                    }
                }
            }
        }
        public int GetWidth()
        {
            return mazeWidth;
        }
        public int GetHeight()
        {
            return mazeHeight;
        }
        public void SetElementAt(int column, int row, Element ele)
        {
            //Verifie si les valeurs de row et colonne sont inclus dans le tableau.
            if (row < mazeHeight && column < mazeWidth)
            {
                mazeElement[column, row] = ele;
            }

        }
        public Element GetMazeElementAt(int column, int row)
        {
            //Verifie si les valeurs de row et colonne sont inclus dans le tableau.
            if (row < mazeHeight && column < mazeWidth)
            {
                return mazeElement[column,row];
            }
            else
            {
                return Element.None;
            }
        }
        
    }
}
