﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ProjetRDV247.Modele;
using ProjetRDV247.Utils;
using System.ServiceModel.Web;
using System.Net;
using System.Globalization;

namespace ProjetRDV247.Controle
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "ServiceRDV247" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez ServiceRDV247.svc ou ServiceRDV247.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class ServiceRDV247 : IServiceRDV247
    {
        // Le data entity
        private Dao dao;
        
        // GET
        //============================================================================================================================

        /// <summary>
        /// Retourne la liste de tous les employés
        /// </summary>
        /// <returns>La liste de tous les employés</returns>
        public List<Employe> GetEmployes()
        {
            // Création de l'objet d'accès aux données
            dao = new Dao();

            return dao.GetListeEmployes();
        }

        /// <summary>
        /// Retourne toutes les disponibilités de l'employé pour la semaine courante
        /// </summary>
        /// <param name="idEmploye">L'identifiant de l'employé</param>
        /// <param name="date">La date d'un jour dans la semaine en format 'yyyyMMdd'</param>
        /// <returns>La liste de toutes les disponibilités de l'employé pour cette semaine</returns>
        public List<Rendezvous> GetDisposEmploye(string idEmploye, string date)
        {
            // Validation des paramètres
            if (Utilitaire.ValiderEntier(idEmploye) == false || Utilitaire.ValiderDate(date) == false)         
                throw new WebFaultException(HttpStatusCode.BadRequest);
            
            // Création de l'objet d'accès aux données
            dao = new Dao();

            // On trouve la date du premier jour de la semaine
            DateTime dateDebut = Utilitaire.TrouverLundiPrecedent(DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture));

            // On retourne les disponibilités de l'employé pour la semaine
            return dao.GetDisposEmploye(Convert.ToInt32(idEmploye), dateDebut, dateDebut.AddDays(6));
        }
        
        /// <summary>
        /// Retourne toutes les rendez-vous d'un client à partir de la semaine courante
        /// </summary>
        /// <param name="idClient">L'identifiant de l'employé</param>        
        /// <returns>La liste de tous les rendez-vous futurs pour ce client</returns>
        public List<Rendezvous> GetRDVClient(string idClient)
        {
            // Validation des paramètres
            if (Utilitaire.ValiderEntier(idClient) == false)
                throw new WebFaultException(HttpStatusCode.BadRequest);

            // Création de l'objet d'accès aux données
            dao = new Dao();

            return dao.GetRendezvousClient(Convert.ToInt32(idClient), Utilitaire.TrouverLundiPrecedent(DateTime.Today));
        }

        /// <summary>
        /// Retourne tous les rendez-vous de l'employé pour la semaine commençant à la date fournie
        /// </summary>
        /// <param name="idEmploye">L'identifiant de l'employé</param>
        /// <param name="date">La date en format 'yyyyMMdd'</param>
        /// <returns>La liste de tous les rendez-vous pour cet employé cette journée</returns>
        public List<Rendezvous> GetRDVEmploye(string idEmploye, string date)
        {
            // Validation des paramètres
            if (Utilitaire.ValiderEntier(idEmploye) == false || Utilitaire.ValiderDate(date) == false)
                throw new WebFaultException(HttpStatusCode.BadRequest);

            // Création de l'objet d'accès aux données
            dao = new Dao();

            // On trouve la date du premier jour de la semaine
            DateTime dateJour = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture).Date;

            // On retourne les rendez-vous de l'employé pour cette journée
            List<Rendezvous> resultats = dao.GetDisposEmploye(Convert.ToInt32(idEmploye), dateJour, dateJour.AddDays(6));
            return resultats.Where(r => r.id_client_rdv != null).ToList();            
        }


        public List<Rendezvous> GetDisposRDVEmploye(string idEmploye, string idClient, string date)
        {
            // Validation des paramètres
            if (Utilitaire.ValiderEntier(idClient) == false || Utilitaire.ValiderEntier(idEmploye) == false || Utilitaire.ValiderDate(date) == false)
                throw new WebFaultException(HttpStatusCode.BadRequest);

            // Création de l'objet d'accès aux données
            dao = new Dao();

            // On trouve la date du premier jour de la semaine
            DateTime dateJour = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture).Date;
            DateTime lundi = Utilitaire.TrouverLundiPrecedent(DateTime.Today);
            // On retourne les rendez-vous de l'employé pour cette journée
            int id_client = Convert.ToInt32(idClient);
            List<Rendezvous> resultats = dao.GetDisposEmploye(Convert.ToInt32(idEmploye), lundi, lundi.AddDays(6));
            return resultats.Where(r => r.id_client_rdv == null || r.id_client_rdv == id_client).ToList();
        }

        // POST
        //============================================================================================================================

        /// <summary>
        /// Permet de réserver une disponibilité pour un client
        /// </summary>
        /// <param name="idClient">L'identifiant du client</param>
        /// <param name="idRDV">Le id de la disponibilité à réserver pour rendez-vous</param>
        /// <returns>Le rendez-vous pris ou null si il n'était plus disponible</returns>
        public bool PrendreRDV(int idClient, int idRDV)
        {
            // Création de l'objet d'accès aux données
            dao = new Dao();

            // On récupère la disponibilité au cas où elle aurait changée
            Rendezvous dispo = dao.GetRendezvousById(idRDV);
            if (dispo == null)
            {
                return false;// TODO: Exception dispo inexistante !!!
            }

            // On vérifie si le client existe
            if (dao.GetClientById(idClient) == null)
            {
                return false;// TODO: Exception dispo inexistante !!!
            }

            // On vérifie si la disponibilité est toujours libre
            if (dispo.statut_rdv.Equals("LIBRE") && dispo.id_client_rdv == null)
            {
                // Réservation
                dispo.id_client_rdv = idClient;
                dispo.statut_rdv = "rv";

                // Mise à jour
                dao.UpdateRendezvous(dispo);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Permet d'annuler le rendez-vous d'un client
        /// </summary>
        /// <param name="idClient">L'identifiant du client</param>
        /// <param name="idRDV">Le id du rendez-vous du client</param>
        public bool AnnulerRDV(int idClient, int idRDV)
        {
            // Création de l'objet d'accès aux données
            dao = new Dao();

            // On vérifie si le rendez-vous existe
            Rendezvous rdv = dao.GetRendezvousById(idRDV);
            if (rdv == null)
            {
                return false;// TODO: Exception rdv inexistant !!!
            }

            // On vérifie si le client existe
            Client client = dao.GetClientById(idClient);
            if (client == null)
            {
                return false;// TODO: Exception rdv inexistant !!!
            }

            // On vérifie si le rendez-vous était bien au client
            if (rdv.id_client_rdv == client.id_client && rdv.statut_rdv.Equals("LIBRE") == false)
            {
                // Annulation
                rdv.id_client_rdv = null;
                rdv.Client = null;
                rdv.statut_rdv = "LIBRE";

                // Mise à jour
                dao.UpdateRendezvous(rdv);
                return true;
            }
            return false;
        }         

        /// <summary>
        /// Ajoute des disponibilités pour un employé
        /// </summary>
        /// <param name="idEmploye">L'identifiant de l'employé</param>
        /// <param name="dateDebut">Le début de la plage horaire d'insertion</param>
        /// <param name="dateFin">La fin de la plage horaire d'insertion</param>
        /// <param name="dureeDispo">La durée d'une disponibilité</param>
        /// <param name="idType">Le id du type de disponibilité</param>
        /// <returns>La liste des disponibilités ajoutées (celles qui n'étaient pas en conflit)</returns>
        public List<Rendezvous> AjouterDispos(int idEmploye, DateTime dateDebut, DateTime dateFin, int dureeMinutesDispo, int idType)
        {   
            // Création de l'objet d'accès aux données
            dao = new Dao();
            
            // Création de la liste des nouvelles disponibilités
            List<Rendezvous> dispoAjoutees = new List<Rendezvous>();
            
            // On vérifie si l'employé existe
            if (dao.GetEmployeById(idEmploye) == null)
            {
                return dispoAjoutees;// TODO: exception employé inexistant !!!
            }

            // Récupération des disponibilités existantes de l'employé autour de la plage d'ajout            
            List<Rendezvous> dispoExistantes = dao.GetDisposEmploye(idEmploye, dateDebut.AddDays(-1).Date, dateFin.AddDays(1).Date);

            // Création des disponibilités (TODO: prendre le statut prédéfini dans la BD ???)
            TimeSpan dureeDispo = new TimeSpan(0, dureeMinutesDispo, 0);
            dispoAjoutees = HoraireUtil.CreerDispos(dateDebut, dateFin, dureeDispo, dispoExistantes, idEmploye, idType, "LIBRE");

            // Sauvegarde des nouvelles disponibilités
            dao.InsertListeDispo(dispoAjoutees);

            return dispoAjoutees;
        }

        /// <summary>
        /// Permet de modifier une disponibilité
        /// </summary>
        /// <param name="idEmploye">L'identifiant de l'employé</param>
        /// <param name="idDispo">Le id de la disponibilité à modifier</param>
        /// <param name="newDebut">La nouvelle date de début</param>
        /// <param name="newFin">La nouvelle date de fin</param>
        /// <param name="idType">Le id du type de rendez-vous</param>
        /// <param name="raison">La raison du changement</param>
        /// <returns>Le rendez-vous modifié ou null si non modifié</returns>
        public Rendezvous ModifierDispo(int idEmploye, int idDispo, DateTime newDebut, DateTime newFin, int idType, string raison)
        {
            // Création de l'objet d'accès aux données
            dao = new Dao();

            // On vérifie si l'employé existe
            if (dao.GetEmployeById(idEmploye) == null)
            {
                return null;// TODO: exception employé inexistant
            }

            // Récupération de la disponibilité actuelle à modifier
            Rendezvous dispo = dao.GetRendezvousById(idDispo);
            
            // On vérifie si la disponibilité existe et correspond bien à l'employé
            if (dispo != null && dispo.id_employe_rdv == idEmploye)
            {
                // Récupération des disponibilités existantes de l'employé autour de la nouvelle plage horaire
                List<Rendezvous> dispoExistantes = dao.GetDisposEmploye(idEmploye, newDebut.AddDays(-1).Date, newFin.AddDays(1).Date);

                // Vérification des conflits
                if (!HoraireUtil.IsRDVConflictuel(newDebut, newFin, dispoExistantes))
                {
                    // Modification des informations de la disponibilité
                    Rendezvous dispoModifiee = Utilitaire.ClonerRendezVous(dispo);
                    dispoModifiee.debut_rdv = newDebut;
                    dispoModifiee.fin_rdv = newFin;
                    dispoModifiee.id_typerdv_rdv = idType;

                    // On vérifie si un client a pris rendez-vous pour cette disponibilité                                
                    if (!dispo.statut_rdv.Equals("LIBRE") && dispo.id_client_rdv != null)
                    {
                        // On notifie le client de l'annulation de son rendez-vous
                        CommunicationUtil.NotifierChangementRDV(dispo.Client, dispo, dispoModifiee, dispo.Employe, raison);
                    }

                    // Sauvegarde des modifications
                    dao.UpdateRendezvous(dispoModifiee);

                    return dispoModifiee;
                }
            }

            return null;
        }
        
        /// <summary>
        /// Supprime une disponibilité
        /// </summary>
        /// <param name="idEmploye">L'identifiant de l'employé</param>
        /// <param name="idDispo">Le id de la disponibilité à supprimer</param>
        /// <param name="raison">La raison à fournir en cas de rendez-vous annulé</param>
        public void SupprimerDispo(int idEmploye, int idDispo, string raison)
        { 
            // Création de l'objet d'accès aux données
            dao = new Dao();
            
            // On vérifie si l'employé existe
            if (dao.GetEmployeById(idEmploye) == null)
            {
                return;// TODO: exception employé inexistant
            }

            // Rechargement de la disponibilité au cas où un changement est survenu
            Rendezvous dispo = dao.GetRendezvousById(idDispo);

            // On vérifie si la disponibilité correspond bien à l'employé
            if (dispo != null && dispo.id_employe_rdv == idEmploye)
            {
                // On vérifie si un client a pris rendez-vous pour cette disponibilité                                
                if (!dispo.statut_rdv.Equals("LIBRE") && dispo.id_client_rdv != null)
                {
                    // On notifie le client de l'annulation de son rendez-vous
                    CommunicationUtil.NotifierAnnulationRDV(dispo.Client, dispo, dispo.Employe, raison);
                }

                // Suppression de la disponibilité
                dao.DeleteRendezvous(dispo);
            }            
        }

        /// <summary>
        /// Supprime une liste de disponibilités
        /// </summary>
        /// <param name="idEmploye">L'identifiant de l'employé</param>
        /// <param name="idDispos">La liste des disponibilités à supprimer</param>
        /// <param name="raison">La raison à fournir en cas de rendez-vous annulé</param>        
        public void SupprimerDispos(int idEmploye, List<int> idDispos, string raison)
        {
            foreach (int idDispo in idDispos)
            {
                SupprimerDispo(idEmploye, idDispo, raison);
            }
        }

    }
}
