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

namespace Library
{
    /// <summary>
    /// Interaction logic for CreateUpdateAbonent.xaml
    /// </summary>
    public partial class CreateUpdateAbonent : Window
    {
        LibraryServiceClient client = new LibraryServiceClient();

        private string ErrorMessage { set; get; }

        private AbonentDTO _abonent;
        public AbonentDTO Abonent
        { 
            set
            {
                _abonent = value;
                Id.DataContext = _abonent;
                LastName.DataContext = _abonent;
                FirstName.DataContext = _abonent;
                Patronymic.DataContext = _abonent;
            }
            get
            {
                return _abonent;
            }
        }

        public CreateUpdateAbonent()
        {
            InitializeComponent();
        }

        public void SetLabelTitle(string title)
        {
            LabelTitle.Content = title;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Abonent.FirstName) || String.IsNullOrEmpty(Abonent.LastName) || String.IsNullOrEmpty(Abonent.Patronymic))
            {
                MessageBox.Show("Хотя бы одно из полей не проинициализировано. Задайте значения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Abonent.Id == 0)
            {
                ErrorMessage = client.CreateAbonent(Abonent);
                if (ErrorMessage != String.Empty)
                {
                    MessageBox.Show(ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.DialogResult = false;
                }
                else this.DialogResult = true;
            }
            else
            {
                ErrorMessage = client.UpdateAbonent(Abonent);
                if (ErrorMessage != String.Empty)
                {
                    MessageBox.Show(ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.DialogResult = false;
                }
                else this.DialogResult = true;
            }
        }
    }
}
