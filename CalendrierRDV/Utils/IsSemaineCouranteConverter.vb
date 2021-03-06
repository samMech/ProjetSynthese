﻿Imports System.Globalization

''' <summary>
''' Convertisseur pour savoir si la semaine courante est affichée
''' </summary>
Public Class IsSemaineCouranteConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

        'Récupération du premier jour affiché
        Dim premierJour As DateTime = (CType(value, DateTime))

        'Calcul du premier jour de la semaine courante
        Dim premierJourCourant As DateTime = Utilitaire.TrouverLundiPrecedent(DateTime.Today)

        'Comparaison
        Return Math.Abs((premierJour.Date - premierJourCourant).Days) < 7

    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
