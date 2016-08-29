using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Library.LibraryServiceWCF;
using System.Xml.Serialization;
using System.IO;

namespace Library
{
    /// <summary>
    /// Interaction logic for BookPopularityXml.xaml
    /// </summary>
    public partial class BookPopularityXml : Window
    {
        private LibraryServiceClient libraryService = new LibraryServiceClient();
        public BookPopularityXml()
        {
            InitializeComponent();
            FirstDate.SelectedDate = DateTime.Now;
            LastDate.SelectedDate = DateTime.Now;
        }

        private void OpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            dlg.DefaultExt = ".xml";
            dlg.Filter = "Файлы XML (*.xml)|*.xml|Все файлы (*.*)|*.*";
            DateTime firstDate = (DateTime) FirstDate.SelectedDate;
            DateTime lastDate = (DateTime) LastDate.SelectedDate;
            if (firstDate > lastDate)
            {
                MessageBox.Show("Начальная дата не может быть раньше конечной", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    makeXmlReportAboutBookPopuparity(dlg.FileName, firstDate, lastDate);
                    MessageBox.Show("Файл сохранен", "Успешное сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка записи в файл. " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void makeXmlReportAboutBookPopuparity(string fileName, DateTime firstDate, DateTime lastDate)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ReportBookPopularity));
            using (TextWriter textWriter = new StreamWriter(fileName))
            {
                ReportBookPopularityDto dto = libraryService.GetBookPopularityReport(firstDate, lastDate);
                if (dto.ErrorMessage != String.Empty)
                    throw new Exception(dto.ErrorMessage);
                serializer.Serialize(textWriter, dto.ReportBookPopularity);
            }
        }
    }
}
