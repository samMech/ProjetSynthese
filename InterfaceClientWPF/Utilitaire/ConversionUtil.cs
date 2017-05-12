using InterfaceClientWPF.Model;
using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using InterfaceClientWPF.Modele;

namespace InterfaceClientWPF.Utilitaire
{
    /// <summary>
    /// Classe utilitaire pour des méthodes générales de conversion
    /// </summary>
    public class ConversionUtil
    {
        /// <summary>
        /// Méthode pour convertir un SecureString en String
        /// </summary>
        /// <param name="secureString">La chaîne sécurisée à convertir</param>
        /// <returns>La châine non sécurisée correspondante</returns>
        public static string SecureStringToString(SecureString secureString)
        {
            if (secureString == null)
            {
                return "";
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Méthode pour adapter une liste de Rendezvous en collection observable de IRendezvous
        /// </summary>
        /// <param name="listeRDV">La liste de Rendezvous</param>
        /// <returns>La collection observable de IRendezVous</returns>
        public static ObservableCollection<CalendrierRDV.IRendezVous> ConvertirRDVToIRDV(List<Rendezvous> listeRDV)
        {
            ObservableCollection<CalendrierRDV.IRendezVous> listeIRDV = new ObservableCollection<CalendrierRDV.IRendezVous>();
            foreach (Rendezvous rdv in listeRDV)
            {
                listeIRDV.Add(new RendezVousAdapter(rdv));
            }
            return listeIRDV;
        }

        /// <summary>
        /// Méthode pour adapter une liste de Rendezvous en collection observable de IRendezvous
        /// </summary>
        /// <param name="listeRDV">La liste de Rendezvous</param>
        /// <param name="couleurs">La référence pour les couleurs des statuts</param>
        /// <returns>La collection observable de IRendezVous</returns>
        public static ObservableCollection<CalendrierRDV.IRendezVous> ConvertirRDVToIRDV(List<Rendezvous> listeRDV, Dictionary<string, Color> couleurs)
        {
            ObservableCollection<CalendrierRDV.IRendezVous> listeIRDV = new ObservableCollection<CalendrierRDV.IRendezVous>();
            foreach (Rendezvous rdv in listeRDV)
            {
                listeIRDV.Add(new RendezVousAdapter(rdv, couleurs));
            }
            return listeIRDV;
        }

    }
}
