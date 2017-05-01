using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceEntrepriseWPF
{
    /// <summary>
    /// Classe pour une commande
    /// </summary>
    class RelayCommand : ICommand
    {
        // Attributs
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        //===============//
        // Constructeurs //
        //===============//

        /// <summary>
        /// Constructeur pour une commande toujours exécutable
        /// </summary>
        /// <param name="execute">L'action à exécuter</param>
        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Constructeur pour une commande
        /// </summary>
        /// <param name="execute">L'action à exécuter</param>
        /// <param name="canExecute">Predicat pour savoir si la fonction est exécutable</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            // Validation
            if (execute == null)
                throw new ArgumentNullException("execute");

            if (canExecute == null)
                throw new ArgumentNullException("canExecute");

            // Initialisation
            this.execute = execute;
            this.canExecute = canExecute;
        }


        //========================================//
        // Implémentation de l'interface ICommand //
        //========================================//

        /// <summary>
        /// Événement pour gérer l'exécution de la commande
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Méthode pour savoir si la commande peut s'exécuter
        /// </summary>
        /// <param name="parameter">Les paramètres</param>
        /// <returns>Vrai si la commande peut être exécutée</returns>
        public bool CanExecute(object parameters)
        {
            return canExecute == null ? true : canExecute(parameters);
        }

        /// <summary>
        /// Méthode pour exécuter la commande
        /// </summary>
        /// <param name="parameter">Les paramètres</param>
        public void Execute(object parameters)
        {
            execute(parameters);
        }

    }
}
