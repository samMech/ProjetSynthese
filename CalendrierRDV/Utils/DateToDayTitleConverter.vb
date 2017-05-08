Imports System.Globalization

''' <summary>
''' Convertisseur pour retourner le nombre du jour de la semaine d'après la date du début
''' </summary>
Public Class DateToDayTitleConverter
    Implements IValueConverter

    'Date --> jour (selon le paramètre)
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

        Dim dateDebut As DateTime = CType(value, DateTime)
        Select Case CType(parameter, Integer)
            Case 0
                Return String.Format("Lundi ({0})", dateDebut.Day)
            Case 1
                Return String.Format("Mardi ({0})", dateDebut.AddDays(1).Day)
            Case 2
                Return String.Format("Mercredi ({0})", dateDebut.AddDays(2).Day)
            Case 3
                Return String.Format("Jeudi ({0})", dateDebut.AddDays(3).Day)
            Case 4
                Return String.Format("Vendredi ({0})", dateDebut.AddDays(4).Day)
            Case 5
                Return String.Format("Samedi ({0})", dateDebut.AddDays(5).Day)
            Case Else
                Return String.Format("Dimanche ({0})", dateDebut.AddDays(6).Day)
        End Select

    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function

End Class
