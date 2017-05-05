using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InterfaceEntrepriseWPF.Utilitaire
{
    /// <summary>
    /// Classe pour convertir un int en string et vice versa
    /// </summary>
    class IntToStringConverter : IValueConverter
    {
        // Convertion int --> string
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val;
            if (Int32.TryParse(value.ToString(), out val))
            {
                return System.Convert.ToInt32(value.ToString());
            }
            return 0;
        }
    }
}
