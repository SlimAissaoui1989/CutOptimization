// Importation des espaces de noms nécessaires
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Espace de noms pour les fonctionnalités d'optimisation de coupe à une dimension
namespace CutOptimization.OneD
{
    /// <summary>
    /// Représente une collection de barres utilisée dans l'optimisation de coupe.
    /// </summary>
    public class Bars : List<Bar>
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Bars"/>.
        /// </summary>
        public Bars() : base()
        {
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Bars"/> avec une collection existante de barres.
        /// </summary>
        /// <param name="bars">Collection de barres à ajouter à la liste.</param>
        public Bars(IEnumerable<Bar> bars)
            : this()
        {
            AddRange(bars);
        }

        /// <summary>
        /// Ajoute une barre à la liste.
        /// </summary>
        /// <param name="bar">Barre à ajouter à la liste.</param>
        public new void Add(Bar bar)
        {
            // Vérifie si la barre n'est pas nulle
            if (bar == null)
                throw new NullReferenceException("bar cannot be null");

            // Attribue un identifiant unique à la barre basé sur la position dans la liste
            bar.Id = Count + 1;

            // Ajoute la barre à la liste de base
            base.Add(bar);
        }

        /// <summary>
        /// Ajoute une collection de barres à la liste.
        /// </summary>
        /// <param name="bars">Collection de barres à ajouter à la liste.</param>
        public new void AddRange(IEnumerable<Bar> bars)
        {
            // Ajoute chaque barre de la collection à la liste
            for (int i = 0; i < bars.Count(); i++) Add(bars.ElementAt(i));
        }
    }
}
