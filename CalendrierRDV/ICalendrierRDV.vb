''' <summary>
''' Interface pour tous les objets rendez-vous affichables dans le calendrier
''' </summary>
Public Interface ICalendrierRDV

    'Propriétés à implémenter
    Property ID As Integer
    Property JourHeure As DateTime
    Property Duree As TimeSpan
    Property Type As String
    Property statut As String
    Property NomClient As String

End Interface
