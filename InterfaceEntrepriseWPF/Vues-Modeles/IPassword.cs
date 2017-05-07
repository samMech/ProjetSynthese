using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    /// <summary>
    /// Interface pour récupérer le password de la vue par le vue-modèle
    /// 
    /// Source: https://code.msdn.microsoft.com/windowsdesktop/Get-Password-from-df012a86
    /// </summary>
    interface IPassword
    {
        // Propriété à implémenter pour récupérer le password
        System.Security.SecureString Password { get; }
    }
}
