''' <summary>
''' Module contenant des méthode utilitaires
''' </summary>
Public Module Utilitaire

    ''' <summary>
    ''' Fonction pour trouver le lundi précédent une date
    ''' </summary>
    ''' <param name="dateCourante">La date courante</param>
    ''' <returns></returns>
    Public Function TrouverLundiPrecedent(dateCourante As Date) As Date

        'Calcul du nombre de jour entre la dateCourante et lundi dernier
        Dim delta As Integer = DayOfWeek.Monday - dateCourante.DayOfWeek

        'Ajustement au cas où le premier jour de la semaine n'est pas lundi
        If delta > 0 Then
            delta -= 7
        End If

        Return dateCourante.AddDays(delta)

    End Function

End Module