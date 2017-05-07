using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEntrepriseWPF.Utilitaire
{
    /// <summary>
    /// Classe utilitaire pour des méthodes générales de conversion
    /// </summary>
    public class ConversionUtil
    {
        /// <summary>
        /// Méthode pour convertir un SecureString en String
        /// </summary>
        /// <param name="secureString"></param>
        /// <returns></returns>
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

    }
}
