''' <summary>
''' Interface pour tous les objets rendez-vous affichables dans le calendrier
''' </summary>
Public Interface IRendezVous

    'Propriétés à implémenter
    ReadOnly Property ID() As Integer
    ReadOnly Property JourHeure() As DateTime
    ReadOnly Property Duree() As TimeSpan
    ReadOnly Property Type() As String
    ReadOnly Property Statut() As String
    ReadOnly Property NomClient() As String

End Interface
