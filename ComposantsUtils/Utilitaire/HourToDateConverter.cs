using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ComposantsUtils.Utilitaire
{
    /// <summary>
    /// Classe pour modifier uniquement la partie heure d'une date
    /// </summary>
    public class HoursToDateConverter : IValueConverter
    {
        // Le temps original
        private DateTime time;

        // Conversion Date --> Heure
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            time = (DateTime)value;
            return time.Hour;
        }

        // Conversion Heure --> Date
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int hours;
            if (Int32.TryParse(value.ToString(), out hours))
            {
                // Ajustement des heures seulement sur le temps original
                time = time.AddHours(hours - time.Hour);
            }
            return time;
        }
    }
}
