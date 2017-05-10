Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class CelluleRDV
    Implements INotifyPropertyChanged

    'Attributs
    Private _irdv As IRendezVous

    '==============='
    ' Constructeurs '
    '==============='

    ''' <summary>
    ''' Constructeur par défaut
    ''' </summary>
    Public Sub New()
        'Cet appel est requis par le concepteur.
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Constructeur avec paramètres
    ''' </summary>
    ''' <param name="irdv">L'objet de type IRendezVous à afficher</param>
    Public Sub New(irdv As IRendezVous)
        Me.New()

        'Initialisation des propriétés
        Me.IRDV = irdv
    End Sub

    '============'
    ' Propriétés '
    '============'

    'Événement pour notifier des changements de propriétés
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    ''' <summary>
    ''' Méthode pour déclencher l'événement qui notifie le changement de propriété
    ''' </summary>
    ''' <param name="propertyName">Le nom de la propriété</param>
    Protected Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    ''' <summary>
    ''' L'objet de type IRendezVous à afficher
    ''' </summary>
    Public Property IRDV As IRendezVous
        Get
            Return _irdv
        End Get
        Set(value As IRendezVous)
            If value IsNot Nothing Then
                Me._irdv = value
                NotifyPropertyChanged()
            End If
        End Set
    End Property

    '=========='
    ' Méthodes '
    '=========='

    'Listener pour quand on clique sur le rendez-vous
    Private Sub RDVPanel_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        'On inverse le statut de sélection
        IRDV.IsSelectionne = Not IRDV.IsSelectionne
    End Sub

End Class
