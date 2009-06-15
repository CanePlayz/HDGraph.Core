using System;
using System.Collections.Generic;
using System.Text;

namespace HDGraph.Interfaces.ScanEngines
{

    public enum SpecialDirTypes : short
    {
        /// <summary>
        /// Un répertoire ordinaire.
        /// </summary>
        NotSpecial,
        /// <summary>
        /// Indiquant que le répertoire courant est en fait un répertoire fictif représentant 
        /// l'espace libre, et qu'il a été comptabilisé dans la taille du root.
        /// </summary>
        FreeSpaceAndShow,
        /// <summary>
        /// Indiquant que le répertoire courant est en fait un répertoire fictif représentant 
        /// l'espace libre, et qu'il n'a PAS été comptabilisé dans la taille du root.
        /// </summary>
        FreeSpaceAndHide,
        /// <summary>
        /// Indique que le répertoire courant est en fait un répertoire fictif représentant 
        /// les fichiers et dossiers qui n'ont pas été comptabilisés suite à des erreurs d'accès.
        /// </summary>
        UnknownPart,
        /// <summary>
        /// Indique qu'il s'agit d'un répertoire dont le scan a échoué.
        /// </summary>
        ScanError
    }
}