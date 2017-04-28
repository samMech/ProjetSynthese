using System.Data.Entity;

namespace ProjetRDV247.Modele
{
    public class Dao
    {
        public Employe GetEmployeById(int idEmployeValue)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                return bd.Employe.Find(idEmployeValue);
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