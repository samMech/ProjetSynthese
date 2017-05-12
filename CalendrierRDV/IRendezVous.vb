''' <summary>
''' Interface pour tous les objets rendez-vous affichables dans le calendrier
''' </summary>
Public Interface IRendezVous

    'Propriétés à implémenter
    ReadOnly Property ID() As Integer
    ReadOnly Property Debut() As DateTime
    ReadOnly Property Fin() As DateTime
    ReadOnly Property Type() As String
    ReadOnly Property Statut() As String
    ReadOnly Property NomClient() As String

    ReadOnly Property CouleurRDV() As SolidColorBrush
    Property IsSelectionne() As Boolean

End Interface
