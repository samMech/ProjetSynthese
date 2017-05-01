namespace InterfaceEntrepriseWPF
{
    /// <summary>
    /// Interface pour communiquer entre les différents
    /// composants d'une application lors d'un switch
    /// </summary>
    internal interface ISwitchable
    {
        void UtilizeState(object state);
    }
}