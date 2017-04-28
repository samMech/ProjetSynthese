using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ProjetRDV247.Modele
{
    public class Dao
    {
        public Employe getEmployeById(int idEmployeValue)
        {
            throw new NotImplementedException();
        }

        public Client getClientById(int idClientValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retourne la liste des disponibilités d'un employé pour une journée
        /// </summary>
        /// <param name="idEmploye">Le id de l'employé</param>
        /// <param name="date">La date recherchée</param>
        /// <returns>La liste des disponibilités de l'employés pour cette date</returns>
        internal List<Rendezvous> GetDisposEmploye(int idEmploye, DateTime date)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ajoute une liste de nouvelles disponibilités
        /// </summary>
        /// <param name="dispoAjoutees">La liste des disponibilités à ajouter</param>
        internal void InsertListeDispo(List<Rendezvous> dispoAjoutees)
        {
            throw new NotImplementedException();
        }
    }
}