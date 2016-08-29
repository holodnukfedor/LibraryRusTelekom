using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Library.LibraryServiceWCF;

namespace Library
{
    public class AuthorsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (value == null)
            //    return String.Empty;
            ICollection<AuthorDTO> authors = value as ICollection<AuthorDTO>;
            if (authors == null)
                throw new Exception("Неверный тип данных для преобразования и вывода информации об авторах");

            StringBuilder result = new StringBuilder(String.Empty);
            foreach (AuthorDTO author in authors)
            {
                result.Append(author.LastName + " " + author.FirstName.Substring(0, 1) + ". " + author.Patronymic.Substring(0, 1) + ". ");
            }


            return result.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
