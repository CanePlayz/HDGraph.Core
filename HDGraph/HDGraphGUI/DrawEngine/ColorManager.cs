using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace HDGraph.DrawEngine
{
    internal class ColorManager
    {
        private DrawOptions options;

        #region Ctor
        
        public ColorManager(DrawOptions options)
        {
            this.options = options;
        }

        #endregion

        /// <summary>
        /// Est utilisé dans le cas de la génération aléatoire des couleurs.
        /// </summary>
        Random rand = new Random();

        /// <summary>
        /// Renvoie la prochaine couleur à utiliser pour la prochaine partie du graph à dessiner.
        /// </summary>
        /// <returns></returns>
        internal Color GetNextColor(float angle)
        {
            switch (options.ColorStyleChoice)
            {
                case ModeAffichageCouleurs.RandomNeutral:
                    int[] col = new int[] { rand.Next(100, 255), rand.Next(100, 255), rand.Next(100, 255) };
                    col[rand.Next(3)] -= 100;
                    return Color.FromArgb(col[0], col[1], col[2]);
                case ModeAffichageCouleurs.RandomBright:
                    return ColorByLeft(rand.Next(360));
                case ModeAffichageCouleurs.Linear:
                default:
                    return ColorByLeft(Convert.ToInt32(angle) % 360);
            }
        }

        /// <summary>
        /// Renvoie une couleur de l'arc en ciel.
        /// </summary>
        /// <param name="valeurSur360">une valeur comprise entre 0 et 360.</param>
        /// <returns></returns>
        internal static Color ColorByLeft(int valeurSur360)
        {
            if (valeurSur360 > 360 || valeurSur360 < 0)
                throw new ArgumentOutOfRangeException("valeurSur360", "Value must be between 0 and 360.");
            return ColorByLeft(valeurSur360, 360);
        }

        internal static Color ColorByLeft(int valeur, int valeurMax)
        {
            int valMax = valeurMax;
            int section = valeur * 6 / (valMax);
            valeur = Convert.ToInt32(
                        ((float)valeur % (valMax / 6f)) * 255 * 6f / valMax);

            switch (section)
            {
                //						       r     G     b
                case 0: return Color.FromArgb(255, 0, valeur);
                case 1: return Color.FromArgb(255 - valeur, 0, 255);
                case 2: return Color.FromArgb(0, valeur, 255);
                case 3: return Color.FromArgb(0, 255, 255 - valeur);
                case 4: return Color.FromArgb(valeur, 255, 0);
                case 5: return Color.FromArgb(255, 255 - valeur, 0);
                default: return Color.Red;
            }
        }
    }
}
