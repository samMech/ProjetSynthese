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
    }
}