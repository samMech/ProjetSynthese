using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ProjetRDV247.Modele
{
    public class Dao
    {
        public Employe GetEmployeById(int idEmployeValue)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {                
                return bd.Employe.Include(x => x.Typerdv).SingleOrDefault(x => x.id_employe == idEmployeValue);                
            }
        }

        public Client GetClientById(int idClientValue)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                return bd.Client.Find(idClientValue);
            }
        }

        public Rendezvous GetRendezvousById(int idRendezvousValue)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                return bd.Rendezvous.Find(idRendezvousValue);
            }
        }

        public Typerdv GetTyperdvById(int idTyperdvValue)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                return bd.Typerdv.Find(idTyperdvValue);
            }
        }

        public int InsertDispo(Rendezvous rdv)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                bd.Rendezvous.Add(rdv);
                bd.SaveChanges();

                return rdv.id_rdv;
            }
        }

        public void DeleteRendezvous(Rendezvous rdv)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                bd.Rendezvous.Remove(rdv);
                bd.SaveChanges();
            }
        }

        /// <summary>
        /// Retourne la liste des disponibilités d'un employé
        /// </summary>
        /// <param name="idEmploye">Le id de l'employé</param>
        /// <param name="dateDebut">La date du début de l'intervalle recherché</param>
        /// <param name="dateFin">La date de fin de l'intervalle recherché</param>
        /// <returns>La liste des disponibilités de l'employés pour cette date</returns>
        internal List<Rendezvous> GetDisposEmploye(int idEmploye, DateTime dateDebut, DateTime dateFin)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                List<Rendezvous> dispos = (from r in bd.Rendezvous//.Include(x => x.Client)
                                           where r.id_employe_rdv == idEmploye
                                            && r.debut_rdv >= DbFunctions.TruncateTime(dateDebut)
                                            && r.fin_rdv <= DbFunctions.TruncateTime(DbFunctions.AddDays(dateFin,1))
                                           select r).OrderBy(r => r.debut_rdv).ToList();
                return dispos;
            }
        }
        
        /// <summary>
        /// Ajoute une liste de nouvelles disponibilités
        /// </summary>
        /// <param name="dispoAjoutees">La liste des disponibilités à ajouter</param>
        internal void InsertListeDispo(List<Rendezvous> dispoAjoutees)
        {
            int id;
            foreach (Rendezvous r in dispoAjoutees)
            {
                id = InsertDispo(r);
                r.id_rdv = id;// Mise à jour du id
            }
        }

        /// <summary>
        /// Méthode pour mettre à jour un rendez-vous
        /// </summary>
        /// <param name="rdv">Le rendez-vous à mettre à jour</param>
        internal void UpdateRendezvous(Rendezvous rdvModifie)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                // Mise à jour du rendez-vous
                Rendezvous rdv = bd.Rendezvous.Find(rdvModifie.id_rdv);
                bd.Entry(rdv).CurrentValues.SetValues(rdvModifie);                
                bd.SaveChanges();
            }
        }

        /// <summary>
        /// Retourne la liste de tous les employés
        /// </summary>
        /// <returns>La liste de tous les employés</returns>
        internal List<Employe> GetListeEmployes()
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                return bd.Employe.ToList();
            }
        }

        /// <summary>
        /// Retourne tous les rendez-vous d'un client à partir d'une date
        /// </summary>
        /// <param name="idClient">L'identifiant du client</param>
        /// <param name="date">La date à partir de laquelle chercher</param>
        /// <returns></returns>
        internal List<Rendezvous> GetRendezvousClient(int idClient, DateTime date)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                List<Rendezvous> rdvs = (from r in bd.Rendezvous
                                         where r.id_client_rdv == idClient
                                            && r.debut_rdv >= DbFunctions.TruncateTime(date)
                                         select r).OrderBy(r => r.debut_rdv).ToList();
                return rdvs;
            }
        }
    }
}