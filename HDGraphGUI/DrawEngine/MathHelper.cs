using System;

namespace HDGraph.DrawEngine
{
    public class MathHelper
    {

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
