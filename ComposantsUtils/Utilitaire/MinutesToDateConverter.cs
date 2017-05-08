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
    /// Classe pour modifier uniquement les minutes d'une date
    /// </summary>
    public class MinutesToDateConverter : IValueConverter
    {
        // Le temps original
        private DateTime time;

        // Conversion Date --> Minutes
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            time = (DateTime)value;
            return time.Minute;
        }

        // Conversion Minutes --> Date
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int minutes;
            if (Int32.TryParse(value.ToString(), out minutes))
            {
                // Ajustement des minutes seulement sur le temps original
                time = time.AddMinutes(minutes - time.Minute);                
            }
            return time;            
        }
    }
}
