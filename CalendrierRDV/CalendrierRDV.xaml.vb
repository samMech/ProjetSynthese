Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Threading

Public Class CalendrierRDV
    Implements INotifyPropertyChanged

    '============'
    ' Constantes '
    '============'

    ''' <summary>
    ''' L'heure de travail minimum par défaut
    ''' </summary>
    Public ReadOnly HEURE_MIN_DEFAULT As DateTime = TimeValue("8:00:00")

    ''' <summary>
    ''' L'heure de travail maximum par défaut
    ''' </summary>
    Public ReadOnly HEURE_MAX_DEFAULT As DateTime = TimeValue("17:00:00")

    ''' <summary>
    ''' La variation de temps la plus courte dans la grille par défaut
    ''' </summary>
    Public ReadOnly DELTA_TIME_MIN_DEFAULT As TimeSpan = New TimeSpan(0, 5, 0) '5min

    ''' <summary>
    ''' La couleur pour mettre en évidence le jour actif
    ''' </summary>
    Public ReadOnly COULEUR_JOUR_ACTIF_DEFAULT As SolidColorBrush = New SolidColorBrush(Colors.LightGreen)

    '============'
    ' Attributs '
    '============'

    'Propriétés
    Private _dateDebut As Date
    Private _heureMin As DateTime = HEURE_MIN_DEFAULT
    Private _heureMax As DateTime = HEURE_MAX_DEFAULT
    Private _deltaTimeMin As TimeSpan = DELTA_TIME_MIN_DEFAULT

    '=============='
    ' Constructeur '
    '=============='

    ''' <summary>
    ''' Constructeur par défaut
    ''' </summary>
    Public Sub New()
        'Cet appel est requis par le concepteur.
        InitializeComponent()

        'Ajustement de la culture
        Me.Language = System.Windows.Markup.XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.IetfLanguageTag)

        'Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        DateCourante = DateTime.Now
        ListeIRDV = New ObservableCollection(Of IRendezVous)
        ReconstruireGrilleHoraire()
    End Sub

    '======================================'
    ' Gestion du changement des propriétés '
    '======================================'

    'Événement pour les changements de propriétés
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    ''' <summary>
    ''' Méthode pour déclencher l'événement qui notifie le changement de propriété
    ''' </summary>
    ''' <param name="propertyName">Le nom de la propriété</param>
    Protected Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    '============'
    ' Propriétés '
    '============'

    Public Property DateDebut() As DateTime
        Get
            Return _dateDebut
        End Get
        Set(ByVal value As DateTime)
            Me._dateDebut = value
            NotifyPropertyChanged()
        End Set
    End Property

    '==================================================================================
    'Propriété de dépendance pour la date courante
    Public Shared ReadOnly Property DateCouranteProperty As DependencyProperty =
        DependencyProperty.Register("DateCourante",
        GetType(Date), GetType(CalendrierRDV),
        New FrameworkPropertyMetadata(DateTime.Today, AddressOf OnDateCourantePropertyChanged))

    'Encapsulation (interne: NE PAS TOUCHER !)
    Public Property DateCourante() As Date
        Get
            Return CType(GetValue(DateCouranteProperty), Date)
        End Get
        Set(value As Date)
            SetValue(DateCouranteProperty, value)
        End Set
    End Property

    'Méthode pour signaler le changement au composant
    Private Shared Sub OnDateCourantePropertyChanged(source As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim cal As CalendrierRDV = TryCast(source, CalendrierRDV)
        If Not IsNothing(cal) Then
            cal.DateDebut = Utilitaire.TrouverLundiPrecedent(cal.DateCourante)
            cal.AfficherListeRV()
        End If
    End Sub
    '==================================================================================

    <Browsable(True), Category("Paramètres"), Description("L'heure minimale affichée.")>
    Public Property HeureDebut() As DateTime
        Get
            Return _heureMin
        End Get
        Set(ByVal value As DateTime)
            If value.TimeOfDay < HeureFin.TimeOfDay Then
                Me._heureMin = value
                ReconstruireGrilleHoraire()
                NotifyPropertyChanged()
            End If
        End Set
    End Property

    <Browsable(True), Category("Paramètres"), Description("L'heure maximale affichée.")>
    Public Property HeureFin() As DateTime
        Get
            Return _heureMax
        End Get
        Set(ByVal value As DateTime)
            If value.TimeOfDay > HeureDebut.TimeOfDay Then
                Me._heureMax = value
                ReconstruireGrilleHoraire()
                NotifyPropertyChanged()
            End If
        End Set
    End Property

    <Browsable(True), Category("Paramètres"), Description("Le nombre de minutes pour l'intervalle de temps le plus court pour les cases de la grille (Doit être un multiple de 60).")>
    Public Property IntervalleTempsMin() As TimeSpan
        Get
            Return _deltaTimeMin
        End Get
        Set(ByVal value As TimeSpan)
            If value.TotalMinutes > 1 AndAlso value.TotalMinutes <= 60 AndAlso 60 Mod value.TotalMinutes = 0 Then
                Me._deltaTimeMin = value
                ReconstruireGrilleHoraire()
                NotifyPropertyChanged()
            End If
        End Set
    End Property

    '==================================================================================
    'Propriété de dépendance pour la liste des IRendezVous
    Public Shared ReadOnly Property ListeIRDVProperty As DependencyProperty =
        DependencyProperty.Register("ListeIRDV",
        GetType(ObservableCollection(Of IRendezVous)), GetType(CalendrierRDV),
        New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AddressOf OnListeIRDVPropertyChanged))

    'Encapsulation (interne: NE PAS TOUCHER !)
    Public Property ListeIRDV() As ObservableCollection(Of IRendezVous)
        Get
            Return CType(GetValue(ListeIRDVProperty), ObservableCollection(Of IRendezVous))
        End Get
        Set(value As ObservableCollection(Of IRendezVous))
            SetValue(ListeIRDVProperty, value)
        End Set
    End Property

    'Méthode pour signaler le changement au composant
    Private Shared Sub OnListeIRDVPropertyChanged(source As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim cal As CalendrierRDV = TryCast(source, CalendrierRDV)
        If Not IsNothing(cal) Then

            'Suppression du listener sur la vielle collection
            If e.OldValue IsNot Nothing Then
                Dim collection = CType(e.OldValue, INotifyCollectionChanged)
                RemoveHandler collection.CollectionChanged, AddressOf cal.OnListeIRDVCollectionChanged
            End If

            'Ajout d'un listener pour intercepter les changements sur la liste de IRendezVous
            If e.NewValue IsNot Nothing Then
                Dim collection = CType(e.NewValue, ObservableCollection(Of IRendezVous))
                AddHandler collection.CollectionChanged, AddressOf cal.OnListeIRDVCollectionChanged
            End If

            'Mise à jour de l'affichage
            cal.AfficherListeRV()
        End If
    End Sub

    'Méthode pour mettre à jour l'affichage quand la liste des rendez-vous change
    Private Sub OnListeIRDVCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
        'Choix selon l'action
        Select Case e.Action
            Case NotifyCollectionChangedAction.Add
                For Each r As IRendezVous In e.NewItems
                    AfficherRV(r)
                Next
            Case NotifyCollectionChangedAction.Remove
                AfficherListeRV()
            Case NotifyCollectionChangedAction.Replace
            Case NotifyCollectionChangedAction.Move
            Case Else
        End Select
    End Sub

    '==================================================================================

    '==================================================================================
    'Propriété de dépendance pour la liste couleurs de statut
    Public Shared ReadOnly Property CouleurStatutProperty As DependencyProperty =
        DependencyProperty.Register("CouleurStatut",
        GetType(Dictionary(Of String, Color)), GetType(CalendrierRDV),
        New FrameworkPropertyMetadata(Nothing, AddressOf OnCouleurStatutPropertyChanged))

    'Encapsulation (interne: NE PAS TOUCHER !)
    Public Property CouleurStatut() As Dictionary(Of String, Color)
        Get
            Return CType(GetValue(CouleurStatutProperty), Dictionary(Of String, Color))
        End Get
        Set(value As Dictionary(Of String, Color))
            SetValue(CouleurStatutProperty, value)
        End Set
    End Property

    'Méthode pour signaler le changement au composant
    Private Shared Sub OnCouleurStatutPropertyChanged(source As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim cal As CalendrierRDV = TryCast(source, CalendrierRDV)
        If cal IsNot Nothing Then
            cal.AfficherListeRV()
        End If
    End Sub
    '==================================================================================

    '=========='
    ' Méthodes '
    '=========='

    ''' <summary>
    ''' Fonction pour retourner la liste des rendez-vous présentement sélectionnés
    ''' </summary>
    ''' <returns></returns>
    Public Function GetSelectedRV() As ObservableCollection(Of IRendezVous)
        Dim id As Integer
        Dim liste As ObservableCollection(Of IRendezVous) = New ObservableCollection(Of IRendezVous)
        For Each rvCell As CelluleRDV In gHoraire.Children.OfType(Of FrameworkElement).Where(Function(x) x.Name.StartsWith("rvcell")).ToList()
            If rvCell.IsSelectionne Then
                id = Convert.ToInt32(rvCell.Name.Replace("rvcell", ""))
                liste.Add((From r In ListeIRDV Where r.ID = id Select r).First())
            End If
        Next
        Return liste
    End Function

    ''' <summary>
    ''' Méthode pour ajouter un rendez-vous à la liste
    ''' </summary>
    ''' <param name="rv">Le rendez-vous à ajouter</param>
    Public Sub AjouterRV(rv As IRendezVous)
        If (From r In ListeIRDV Where r.ID = rv.ID Select r).FirstOrDefault() Is Nothing Then
            ListeIRDV.Add(rv)
            AfficherRV(rv)
        End If
    End Sub

    ''' <summary>
    ''' Méthode pour supprimer un rendez-vous de la liste
    ''' </summary>
    ''' <param name="rvID">Le id du rendez-vous à supprimer</param>
    Public Sub SupprimerRV(rvID As Integer)
        Dim rv = (From r In ListeIRDV Where r.ID = rvID Select r).FirstOrDefault
        If rv IsNot Nothing Then
            ListeIRDV.Remove(rv)
            Dim rvCell = gHoraire.Children.OfType(Of FrameworkElement).Where(Function(x) x.Name.Equals("rvcell" + rvID.ToString)).First()
            gHoraire.Children.Remove(rvCell)
        End If
    End Sub

    ''' <summary>
    ''' Méthode pour modifier un rendez-vous existant ou l'ajout
    ''' </summary>
    ''' <param name="rv2">Le rendez-vous modifié</param>
    Public Sub ModifierRV(rv2 As IRendezVous)

        'Suppression du rendez-vous existant
        Dim rv = (From r In ListeIRDV Where r.ID = rv2.ID Select r).FirstOrDefault
        If rv IsNot Nothing Then
            ListeIRDV.Remove(rv)
            Dim rvCell = gHoraire.Children.OfType(Of FrameworkElement).Where(Function(x) x.Name.Equals("rvcell" + rv2.ID.ToString)).First()
            gHoraire.Children.Remove(rvCell)
        End If

        'Ajout du rendez-vous modifié
        AjouterRV(rv2)
    End Sub

    'Méthode pour afficher la liste des rendez-vous actuelle
    Private Sub AfficherListeRV()
        'Réinitialisation des rendez-vous affichés
        For Each elem In gHoraire.Children.OfType(Of FrameworkElement).Where(Function(x) x.Name.StartsWith("rvcell")).ToList()
            gHoraire.Children.Remove(elem)
        Next

        If ListeIRDV IsNot Nothing Then
            'On affiche la liste de rendez-vous
            For Each rv In ListeIRDV
                AfficherRV(rv)
            Next
        End If

    End Sub

    'Méthode pour afficher un rendez-vous
    Private Sub AfficherRV(rv As IRendezVous)
        'On vérifie d'abord si le rendez-vous est valide pour la vue courante
        If rv.JourHeure >= DateDebut + HeureDebut.TimeOfDay AndAlso rv.JourHeure + rv.Duree < DateDebut.AddDays(6) + HeureFin.TimeOfDay Then

            'Calcul de la position et de la hauteur du RV
            Dim heureBase As DateTime = New DateTime(HeureDebut.Year, HeureDebut.Month, HeureDebut.Day, HeureDebut.Hour, 0, 0)
            Dim colonneRV = 1 + (rv.JourHeure - DateDebut).Days
            Dim ligneRV = 2 + (rv.JourHeure.TimeOfDay - heureBase.TimeOfDay).TotalMinutes / IntervalleTempsMin.TotalMinutes
            Dim nbLignesRV = (rv.Duree.TotalMinutes / IntervalleTempsMin.TotalMinutes)

            'Préparation des infos à afficher dans la cellule
            Dim infos = New List(Of String)
            infos.Add(rv.JourHeure.TimeOfDay.ToString("hh\:mm") + " à " + rv.JourHeure.TimeOfDay.Add(rv.Duree).ToString("hh\:mm"))
            If (rv.Type IsNot Nothing) AndAlso (rv.Type.Length <> 0) Then
                infos.Add(rv.Type)
            End If
            If (rv.NomClient IsNot Nothing) AndAlso (rv.NomClient.Length <> 0) Then
                infos.Add(rv.NomClient)
            End If

            'Création du composant pour afficher le rendez-vous
            Dim couleurRV As Color = If(rv.Statut IsNot Nothing AndAlso CouleurStatut IsNot Nothing AndAlso CouleurStatut.ContainsKey(rv.Statut), CouleurStatut(rv.Statut), Colors.Transparent)
            Dim celluleRV = New CelluleRDV(New SolidColorBrush(couleurRV), infos)
            celluleRV.Name = String.Format("rvcell{0}", rv.ID)

            'Ajout du composant dans la grille
            gHoraire.Children.Add(celluleRV)
            Grid.SetRow(celluleRV, ligneRV)
            Grid.SetColumn(celluleRV, colonneRV)
            Grid.SetRowSpan(celluleRV, nbLignesRV)

        End If
    End Sub

    'Méthode pour reconstruire la grille selon les paramètres actuel
    Private Sub ReconstruireGrilleHoraire()

        'Vérification au cas où pour ne pas avoir de boucle infinie
        If _heureMax.TimeOfDay > HeureDebut.TimeOfDay AndAlso _deltaTimeMin.Minutes > 0 Then
            'Récupération des deux premières lignes
            Dim l0 As RowDefinition = gHoraire.RowDefinitions.ElementAt(0)
            Dim l1 As RowDefinition = gHoraire.RowDefinitions.ElementAt(1)

            'On efface la grille et les labels pour les heures
            gHoraire.RowDefinitions.Clear()
            For Each elem In gHoraire.Children.OfType(Of FrameworkElement).Where(Function(x) x.Name.StartsWith("lblTemps")).ToList()
                gHoraire.Children.Remove(elem)
            Next

            'On remet les lignes d'entêtes en place
            gHoraire.RowDefinitions.Add(l0)
            gHoraire.RowDefinitions.Add(l1)

            'Calcul du nombre de lignes par heure
            Dim nbLignesHeure As Integer = 60 / _deltaTimeMin.Minutes

            'Ajout des cases de l'heure de début à l'heure de fin selon l'intervalle minimum
            For i = HeureDebut.Hour To _heureMax.Hour
                For j = 1 To nbLignesHeure
                    gHoraire.RowDefinitions.Add(New RowDefinition())
                Next
            Next

            'On remet le binding pour les bordures
            Dim exp As BindingExpression
            For Each elem In gHoraire.Children
                exp = elem.GetBindingExpression(Grid.RowSpanProperty)
                If exp IsNot Nothing Then
                    exp.UpdateTarget()
                End If
            Next

            'On ajoute les labels et les séparateurs
            Dim label As Label
            Dim sep As Separator
            Dim tempsCourant As DateTime = New DateTime(HeureDebut.Year, HeureDebut.Month, HeureDebut.Day, HeureDebut.Hour, 0, 0)
            For i = 2 To gHoraire.RowDefinitions.Count - 2
                If (i - 2) Mod nbLignesHeure = 0 Then
                    'Ajout du label
                    label = New Label()
                    label.Content = tempsCourant.ToString("HH:mm")
                    label.Name = "lblTemps" + tempsCourant.ToString("HHmm")
                    gHoraire.Children.Add(label)
                    Grid.SetColumn(label, 0)
                    Grid.SetRow(label, i)
                    Grid.SetRowSpan(label, nbLignesHeure)

                    'Ajout du séparateur horizontal
                    If i <> 2 Then
                        sep = New Separator()
                        sep.Name = "lblTempsSep" + tempsCourant.ToString("HHmm")
                        gHoraire.Children.Add(sep)
                        Grid.SetColumn(sep, 0)
                        Grid.SetRow(sep, i - 1)
                        Grid.SetColumnSpan(sep, 8)
                    End If
                End If
                tempsCourant += _deltaTimeMin
            Next

        End If
    End Sub

End Class
