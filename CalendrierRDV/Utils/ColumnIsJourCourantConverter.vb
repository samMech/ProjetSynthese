Imports System.Globalization

''' <summary>
''' Convertisseur pour retourner vrai si la colonne correspond au jour courant
''' </summary>
Public Class ColumnIsJourCourantConverter
    Implements IValueConverter

    'Integer --> Boolean
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

        'Récupération du no. de colonne
        Dim nomColonne As String = value.ToString()
        Dim noColonne As Integer = CType(nomColonne.Substring(nomColonne.Length - 1), Integer) - 1

        'Calcul du no. du jour courant
        Dim jourCourant As Integer = (CType(DateTime.Today.DayOfWeek, Integer) + 6) Mod 7

        'Comparaison
        Return noColonne = jourCourant

    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
