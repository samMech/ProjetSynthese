Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class CelluleRDV
    Implements INotifyPropertyChanged

    'Attributs
    Private _couleurRV As SolidColorBrush = New SolidColorBrush(Colors.Transparent)
    Private _infoRV As List(Of String) = New List(Of String)
    Private _isSelectionne As Boolean

    '==============='
    ' Constructeurs '
    '==============='

    ''' <summary>
    ''' Constructeur par défaut
    ''' </summary>
    Public Sub New()

        'Cet appel est requis par le concepteur.
        InitializeComponent()

        'Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        IsSelectionne = False
    End Sub

    ''' <summary>
    ''' Constructeur avec paramètres
    ''' </summary>
    ''' <param name="couleur">La couleur de fond</param>
    ''' <param name="infos">La liste des informations à afficher</param>
    Public Sub New(couleur As SolidColorBrush, infos As List(Of String))
        Me.New()

        'Initialisation des propriétés
        Me.CouleurRV = couleur
        Me.InfoRV = infos
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
    ''' La couleur de la cellule
    ''' </summary>
    Public Property CouleurRV As SolidColorBrush
        Get
            Return _couleurRV
        End Get
        Set(value As SolidColorBrush)
            If value IsNot Nothing Then
                Me._couleurRV = value
                NotifyPropertyChanged()
            End If
        End Set
    End Property

    Public Property InfoRV As List(Of String)
        Get
            Return _infoRV
        End Get
        Set(value As List(Of String))
            If value IsNot Nothing Then
                Me._infoRV = value
                NotifyPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Est-ce que le rendez-vous est sélectionné
    ''' </summary>
    Public Property IsSelectionne As Boolean
        Get
            Return _isSelectionne
        End Get
        Set(value As Boolean)
            Me._isSelectionne = value
            NotifyPropertyChanged()
        End Set
    End Property

    '=========='
    ' Méthodes '
    '=========='

    'Listener pour quand on clique sur le rendez-vous
    Private Sub MaGrille_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)
        'On inverse le statut de sélection
        IsSelectionne = Not IsSelectionne
    End Sub

End Class
