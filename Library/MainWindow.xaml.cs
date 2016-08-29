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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library.LibraryServiceWCF;
using System.Xml.Serialization;
using System.IO;

namespace Library
{
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LibraryServiceClient client = new LibraryServiceClient();

        private List<AbonentDTO> abonentsList;
        private List<BookDTO> booksList;

        private int AbonentAllRowsCount { set; get; }
        public int AbonentPageCount { set; get; }

        private int BookAllRowsCount { set; get; }
        public int BookPageCount { set; get; }

        private string ErrorMessage { set; get; }

        private PaginationInfo abonentPaginationInfo = new PaginationInfo { Amount = 10, FromRow = 0, OrderPropertyName = "LastName", SortOrder = SortOrder.ASC };
        private PaginationInfo bookPaginationInfo = new PaginationInfo { Amount = 10, FromRow = 0, OrderPropertyName = "Name", SortOrder = SortOrder.ASC };
        public MainWindow()
        {
            InitializeComponent();
            AbonentPageNumberTxt.DataContext = this;
            BookPageNumberTxt.DataContext = this;
            LoadAbonents();
            LoadBooks();
        }

        public int AbonentPage
        {
            get { return (abonentPaginationInfo.FromRow / abonentPaginationInfo.Amount + 1); }
            set
            {
                if (value > AbonentPageCount)
                    value = AbonentPageCount;
                if (value <= 0)
                    value = 1;
                int fromRow = (value - 1) * abonentPaginationInfo.Amount;

                abonentPaginationInfo.FromRow = fromRow;
                LoadAbonents();
            }
        }

        public int BookPage
        {
            get { return (bookPaginationInfo.FromRow / bookPaginationInfo.Amount + 1); }
            set
            {
                if (value > BookPageCount)
                    value = BookPageCount;
                if (value <= 0)
                    value = 1;
                int fromRow = (value - 1) * bookPaginationInfo.Amount;

                bookPaginationInfo.FromRow = fromRow;
                LoadBooks();
            }
        }

        private void LeftPagingAbonentBtn_Click(object sender, RoutedEventArgs e)
        {
            --AbonentPage;
            AbonentPageNumberTxt.Text = AbonentPage.ToString();
            LoadAbonents();
        }

        private void LeftPagingBookBtn_Click(object sender, RoutedEventArgs e)
        {
            --BookPage;
            BookPageNumberTxt.Text = BookPage.ToString();
            LoadBooks();
        }

        private void RightPagingAbonentBtn_Click(object sender, RoutedEventArgs e)
        {
            ++AbonentPage;
            AbonentPageNumberTxt.Text = AbonentPage.ToString();
            LoadAbonents();
        }

        private void RightPagingBookBtn_Click(object sender, RoutedEventArgs e)
        {
            ++BookPage;
            BookPageNumberTxt.Text = BookPage.ToString();
            LoadBooks();
        }

        private void LoadAbonents()
        {
            try
            {
                AbonentDTOList dto = client.GetAbonentsList(abonentPaginationInfo);
                abonentsList = dto.Abonents.ToList();
                if (dto.ErrorMessage != String.Empty)
                    throw new Exception(dto.ErrorMessage);
                
                AbonentsList.ItemsSource = null;
                AbonentsList.ItemsSource = abonentsList;

                AbonentAllRowsCount = dto.AllRowsCount;
                AbonentPageCount = AbonentAllRowsCount / abonentPaginationInfo.Amount;
                if (AbonentAllRowsCount % abonentPaginationInfo.Amount > 0)
                    ++AbonentPageCount;
                if (AbonentPageCount == 0)
                    AbonentPageCount = 1;
                AbonentPageCountTxt.Text = " " + AbonentPageCount + " ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBooks()
        {
            try
            {
                BookDTOList dto = client.GetBooksList(bookPaginationInfo);
                booksList = dto.Books.ToList();
                if (dto.ErrorMessage != String.Empty)
                    throw new Exception(dto.ErrorMessage);

                BooksList.ItemsSource = null;
                BooksList.ItemsSource = booksList;

                BookAllRowsCount = dto.AllRowsCount;
                BookPageCount = BookAllRowsCount / bookPaginationInfo.Amount;
                if (BookAllRowsCount % bookPaginationInfo.Amount > 0)
                    ++BookPageCount;
                if (BookPageCount == 0)
                    BookPageCount = 1;
                BookPageCountTxt.Text = " " + BookPageCount + " ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DomainMenu_Click(object sender, RoutedEventArgs e)
        {
            DomainWindow domainWindow = new DomainWindow();
            domainWindow.ShowDialog();
        }

        private void PublisherMenu_Click(object sender, RoutedEventArgs e)
        {
            PublisherWindow publisherWindow = new PublisherWindow();
            publisherWindow.ShowDialog();
        }

        private void AuthorsMenu_Click(object sender, RoutedEventArgs e)
        {
            AuthorsWindow authorsWindow = new AuthorsWindow();
            authorsWindow.ShowDialog();
        }

        private void AddAbonentBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenCreateEditAbonentDialog(new AbonentDTO(), "Создание абонента");
        }

        private void AddBookBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenCreateEditBookDialog(new BookDTO() { Authors = new System.Collections.ObjectModel.Collection<AuthorDTO>(),
                Domain = new System.Collections.ObjectModel.Collection<DomainDTO>(),
                Annotation =String.Empty }, "Создание книги");
        }

        private void EditAbonentBtn_Click(object sender, RoutedEventArgs e)
        {
            AbonentDTO abonent = AbonentsList.SelectedItem as AbonentDTO;
            if (abonent == null)
                 MessageBox.Show("Выделите строку для редактирования", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            else OpenCreateEditAbonentDialog(abonent, "Редактирование информации об абоненте");
        }

        private void EditBookBtn_Click(object sender, RoutedEventArgs e)
        {
            BookDTO book = BooksList.SelectedItem as BookDTO;
            if (book == null)
                MessageBox.Show("Выделите строку для редактирования", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            else OpenCreateEditBookDialog(book, "Редактирование информации о книге");
        }

        private void OpenCreateEditAbonentDialog(AbonentDTO abonent, string title)
        {
            CreateUpdateAbonent createUpdateAbonentWindow = new CreateUpdateAbonent();
            createUpdateAbonentWindow.Title = title;
            createUpdateAbonentWindow.SetLabelTitle(title);
            createUpdateAbonentWindow.Abonent = abonent;
            if (createUpdateAbonentWindow.ShowDialog() == true)
            {
                AbonentPage = 1;
                AbonentPageNumberTxt.Text = "1";
                LoadAbonents();
            }
        }

        private void OpenCreateEditBookDialog(BookDTO book, string title)
        {
            CreateUpdateBook createUpdateBookWindow = new CreateUpdateBook();
            createUpdateBookWindow.Title = title;
            createUpdateBookWindow.SetLabelTitle(title);
            createUpdateBookWindow.Book = book;
            if (createUpdateBookWindow.ShowDialog() == true)
            {
                BookPage = 1;
                BookPageNumberTxt.Text = "1";
                LoadBooks();
            }
        }

        private void DeleteAbonentBtn_Click(object sender, RoutedEventArgs e)
        {
            AbonentDTO abonent = AbonentsList.SelectedItem as AbonentDTO;
            if (abonent != null)
            {
                ErrorMessage= client.DeleteAbonent(abonent.Id);
                if (ErrorMessage != String.Empty)
                    MessageBox.Show(ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    AbonentPage = 1;
                    AbonentPageNumberTxt.Text = "1";
                    LoadAbonents();
                }
            }
        }

        private void DeleteBookBtn_Click(object sender, RoutedEventArgs e)
        {
            BookDTO book = BooksList.SelectedItem as BookDTO;
            if (book != null)
            {
                ErrorMessage = client.DeleteBook(book.Id);
                if (ErrorMessage != String.Empty)
                    MessageBox.Show(ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    BookPage = 1;
                    BookPageNumberTxt.Text = "1";
                    LoadBooks();
                }
            }
        }

        private void AbonentSortProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            switch (AbonentSortProperty.SelectedItem.ToString())
            {
                case "Id (восх)":
                    abonentPaginationInfo.SortOrder = SortOrder.ASC;
                    abonentPaginationInfo.OrderPropertyName = "Id";
                    break;
                case "Id (нисх)":
                    abonentPaginationInfo.SortOrder = SortOrder.DESC;
                    abonentPaginationInfo.OrderPropertyName = "Id";
                    break;
                case "Фамилия (восх)":
                    abonentPaginationInfo.SortOrder = SortOrder.ASC;
                    abonentPaginationInfo.OrderPropertyName = "LastName";
                    break;
                case "Фамилия (нисх)":
                    abonentPaginationInfo.SortOrder = SortOrder.DESC;
                    abonentPaginationInfo.OrderPropertyName = "LastName";
                    break;
            }
            LoadAbonents();
        }

        private void AbonentAmountOnPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            abonentPaginationInfo.Amount = (int)AbonentAmountOnPage.SelectedItem;
            AbonentPage = 1;
            AbonentPageNumberTxt.Text = "1";
            LoadAbonents();
        }

        private void BookAmountOnPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bookPaginationInfo.Amount = (int)BookAmountOnPage.SelectedItem;
            BookPage = 1;
            BookPageNumberTxt.Text = "1";
            LoadBooks();
        }

        private void BookSortProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch (BookSortProperty.SelectedItem.ToString())
            {
                case "Id (восх)":
                    bookPaginationInfo.SortOrder = SortOrder.ASC;
                    bookPaginationInfo.OrderPropertyName = "Id";
                    break;
                case "Id (нисх)":
                    bookPaginationInfo.SortOrder = SortOrder.DESC;
                    bookPaginationInfo.OrderPropertyName = "Id";
                    break;
                case "Название (восх)":
                    bookPaginationInfo.SortOrder = SortOrder.ASC;
                    bookPaginationInfo.OrderPropertyName = "Name";
                    break;
                case "Название (нисх)":
                    bookPaginationInfo.SortOrder = SortOrder.DESC;
                    bookPaginationInfo.OrderPropertyName = "Name";
                    break;
                case "Год издания (восх)":
                    bookPaginationInfo.SortOrder = SortOrder.ASC;
                    bookPaginationInfo.OrderPropertyName = "YearOfPublication";
                    break;
                case "Год издания (нисх)":
                    bookPaginationInfo.SortOrder = SortOrder.DESC;
                    bookPaginationInfo.OrderPropertyName = "YearOfPublication";
                    break;
                case "Всего (восх)":
                    bookPaginationInfo.SortOrder = SortOrder.ASC;
                    bookPaginationInfo.OrderPropertyName = "Count";
                    break;
                case "Всего (нисх)":
                    bookPaginationInfo.SortOrder = SortOrder.DESC;
                    bookPaginationInfo.OrderPropertyName = "Count";
                    break;
                case "Свободно (восх)":
                    bookPaginationInfo.SortOrder = SortOrder.ASC;
                    bookPaginationInfo.OrderPropertyName = "AmountOfFree";
                    break;
                case "Свободно (нисх)":
                    bookPaginationInfo.SortOrder = SortOrder.DESC;
                    bookPaginationInfo.OrderPropertyName = "AmountOfFree";
                    break;
            }
            LoadBooks();
        }

        private void AbonentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AbonentDTO abonent = AbonentsList.SelectedItem as AbonentDTO;
            if (abonent != null)
            {
                AbonentBooksPanel.Visibility = System.Windows.Visibility.Visible;
                AbonentBookListGrid.Visibility = System.Windows.Visibility.Visible;
                LoadBookOfAbonent(abonent.Id);
                AbonentBooksLbl.Content = "Книги абонента (" + abonent.Id + ") " + abonent.LastName + " " + abonent.FirstName.Substring(0, 1) + ". " + abonent.Patronymic.Substring(0, 1) + ".";
            }
        }

        private void LoadBookOfAbonent(int id)
        {
            UserBookDtoList bookDtoList = client.GetBooksOfAbonent(id, new PaginationInfo {Amount = int.MaxValue, FromRow = 0, OrderPropertyName ="Name", SortOrder = SortOrder.ASC});

            if (bookDtoList.ErrorMessage != String.Empty)
                MessageBox.Show(bookDtoList.ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            AbonentBookListGrid.ItemsSource = null;
            AbonentBookListGrid.ItemsSource = bookDtoList.Books;
        }

        private void ReturnBookBtn_Click(object sender, RoutedEventArgs e)
        {
            UserBookDTO book = AbonentBookListGrid.SelectedItem as UserBookDTO;
            AbonentDTO abonent = AbonentsList.SelectedItem as AbonentDTO;
            if (book == null)
            {
                MessageBox.Show("Выберите книгу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (abonent == null)
            {
                MessageBox.Show("Выберите абонета", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ErrorMessage = client.ReturnBookFromAbonent(book.BookId, abonent.Id);
            if (ErrorMessage != String.Empty)
                MessageBox.Show(ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            LoadBooks();
            LoadBookOfAbonent(abonent.Id);
        }

        private void GiveBookBtn_Click(object sender, RoutedEventArgs e)
        {
            BookDTO book = BooksList.SelectedItem as BookDTO;
            AbonentDTO abonent = AbonentsList.SelectedItem as AbonentDTO;
            if (book == null)
            {
                MessageBox.Show("Выберите книгу", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (abonent == null)
            {
                MessageBox.Show("Выберите абонета", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (book.AmountOfFree <= 0)
            {
                MessageBox.Show("На данный момент нет свободных книг", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ErrorMessage = client.GiveBookToAbonent(book.Id, abonent.Id);
            if (ErrorMessage != String.Empty)
                MessageBox.Show(ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            LoadBooks();
            LoadBookOfAbonent(abonent.Id);
        }

        private void XmlReportMenu_Click(object sender, RoutedEventArgs e)
        {
            BookPopularityXml bookPopularityXmlWnd = new BookPopularityXml();
            bookPopularityXmlWnd.ShowDialog();
        }

        private void XsltReportMenu_Click(object sender, RoutedEventArgs e)
        {
            XmlWithXsltTransformation xsltDialog = new XmlWithXsltTransformation();
            xsltDialog.ShowDialog();
        }
    }
}
