using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Resources;

namespace HDGraph.WpfDrawEngine
{
    public enum AccelerationType
    {
        /// <summary>
        /// No acceleration is available.
        /// All work is done by the CPU.
        /// This is the lowest mode. User should consider not using the WPF plugin, because
        /// the application might not work very fluently.
        /// </summary>
        None = 0,
        /// <summary>
        /// Some work is done by the GPU (the graphic card). 
        /// </summary>
        Partial = 1,
        /// <summary>
        /// All work is done by the GPU (the graphic card). This is the fastest mode,
        /// the application should respond well !
        /// </summary>
        Full = 2
    }

    public static class WpfUtils
    {
        /// <summary>
        /// WPF a la particularité de faire en sorte que tous les calculs nécessaires 
        /// à l’affichage soit effectués par la carte graphique (GPU), si celle-ci le permet. 
        /// Pour savoir, par programmation, si la carte graphique sera en mesure d’effectuer les 
        /// calculs, il faut utiliser RenderCapability.Tier.
        /// 
        /// </summary>
        public static AccelerationType GetAccelerationType()
        {
            // En fonction de la valeur retournée, vous pouvez connaitre les capacités de 
            // votre carte graphique:
            //      0: Aucune accélération matérielle n’est disponible pour le périphérique
            //      1: La carte graphique supporte une accélération matérielle 
            //         partielle. Cela correspond à une version de DirectX comprise entre 7 et 9.
            //      2: Toutes les fonctionnalités d’accélération matérielle sont disponibles. 
            //         Cela correspond à une version de DirectX supérieure (ou égale) à la version 9.
            //
            // Détails : http://msdn.microsoft.com/en-us/library/system.windows.media.rendercapability.tier.aspx

            // The rendering tier corresponds to the high-order word of the Tier property.
            int renderingTier = (RenderCapability.Tier >> 16);
            return (AccelerationType)renderingTier;
        }

        public static string GetAccelerationTypeString(AccelerationType accelerationType)
        {
            try
            {
                return Properties.Resources.ResourceManager.GetString("AccelerationType" + accelerationType.ToString(), Properties.Resources.Culture);
            }
            catch
            {
                // TODO : log exception.
                return Properties.Resources.AccelerationTypeUnknown;
            }
        }

        public static string GetAccelerationTypeDescription(AccelerationType accelerationType)
        {
            try
            {
                return Properties.Resources.ResourceManager.GetString("AccelerationType" + accelerationType.ToString() + "Desc", Properties.Resources.Culture);
            }
            catch
            {
                // TODO : log exception.
                return Properties.Resources.AccelerationTypeUnknownDesc;
            }
        }

        /// <summary>
        /// Convertit un angle en degrés en radian.
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double GetRadianFromDegree(float degree)
        {
            return degree * Math.PI / 180f;
        }

        /// <summary>
        /// Convertit un angle en radian en degrés.
        /// </summary>
        public static double GetDegreeFromRadian(double radian)
        {
            return radian * 180 / Math.PI;
        }
    }
}
