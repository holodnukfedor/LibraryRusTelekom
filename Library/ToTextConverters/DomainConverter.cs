using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.LibraryServiceWCF;
using System.Windows.Data;

namespace Library
{
    public class DomainConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable<DomainDTO> domains = value as IEnumerable<DomainDTO>;
            if (domains == null)
                throw new Exception("Неверный тип данных для преобразования и вывода информации об предметных областях");

            StringBuilder result = new StringBuilder(String.Empty);
            foreach (DomainDTO domain in domains)
            {
                result.Append(domain.Name + " ");
            }

            return result.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
