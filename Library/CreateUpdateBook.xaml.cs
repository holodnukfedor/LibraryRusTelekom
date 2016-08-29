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
    /// Interaction logic for CreateUpdateBook.xaml
    /// </summary>
    public partial class CreateUpdateBook : Window
    {
         LibraryServiceClient client = new LibraryServiceClient();

        private string ErrorMessage { set; get; }

        private BookDTO _book;

        public bool IsPublisherSetted { set; get; }

        private int PublisherId { set; get; }
        public BookDTO Book
        {
            set
            {
                _book = value;
                Id.DataContext = _book;
                Name.DataContext = _book;
                Annotation.DataContext = _book;
                YearOfPublication.DataContext = _book;
                Publisher.DataContext = _book;
                Authors.DataContext = _book;
                Domain.DataContext = _book;
                Count.DataContext = _book;
            }
            get
            {
                return _book;
            }
        }

        public CreateUpdateBook()
        {
            InitializeComponent();
        }

        public void SetLabelTitle(string title)
        {
            LabelTitle.Content = title;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errorMessages = new StringBuilder(String.Empty);
            try 
	        {	        
		         BookEditingInfo editingInfo = new BookEditingInfo { Name = Book.Name, Annotation = Book.Annotation, 
                                Count = Book.Count, YearOfPublication = Book.YearOfPublication, Id = Book.Id };

                 System.Collections.ObjectModel.Collection<int> domainsIds = new System.Collections.ObjectModel.Collection<int>();
                 System.Collections.ObjectModel.Collection<int> authorsIds = new System.Collections.ObjectModel.Collection<int>();
                 Book.Authors.Select(a => a.Id).ToList().ForEach(a => authorsIds.Add(a));
                 Book.Domain.Select(d => d.Id).ToList().ForEach(d => domainsIds.Add(d));

                 if (authorsIds.Count <= 0)
                     errorMessages.Append("Книга должна иметь хотя бы одного автора!\n");
                 if (domainsIds.Count <= 0)
                     errorMessages.Append("Книга должна иметь хотя бы одну предметную область!\n");
                 if (String.IsNullOrEmpty(Publisher.Text))
                     errorMessages.Append("Книга должна иметь издателя!\n");
                 if (Book.YearOfPublication <= 0 || Book.YearOfPublication >= 9999)
                     errorMessages.Append("Год задан неверно!");
                 if (errorMessages.Length > 0)
                     throw new Exception(errorMessages.ToString());
                 if (Book.Count < 0)
                     throw new Exception("Количество долно быть больше нуля!");
                if (Book.Id == 0)
                {
                    ErrorMessage = client.CreateBook(editingInfo, PublisherId,
                        domainsIds, authorsIds);
                    if (ErrorMessage != String.Empty)
                    {
                        throw new Exception(ErrorMessage);
                    }
                    else
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                else
                {
                    if (PublisherId != 0)
                    {
                        ErrorMessage = client.ChangePublisher(PublisherId, Book.Id);
                        if (ErrorMessage != String.Empty)
                        {
                            throw new Exception(ErrorMessage);
                        }
                    }
                    ErrorMessage = client.UpdateBook(editingInfo);
                    if (ErrorMessage != String.Empty)
                    {
                        throw new Exception(ErrorMessage);
                    }
                    else
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                }
	        }
	        catch (Exception ex)
	        {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
	        }
        }

        private void ChangePublisher_Click(object sender, RoutedEventArgs e)
        {
            PublisherWindow publisherWindow = new PublisherWindow();
            publisherWindow.SetTitle("Изменения издателя книги " + Book.Name);
            publisherWindow.WindowMode = PublisherWindowMode.ChangePublisherOfBook;
            if (publisherWindow.ShowDialog() == true)
            {
                PublisherId = publisherWindow.NewPublisher.Id;
                Publisher.Text = publisherWindow.NewPublisher.Name;
            }
        }

        private void EditDomains_Click(object sender, RoutedEventArgs e)
        {
            DomainWindow domainWindow = new DomainWindow();
            domainWindow.SetTitle(Book.Name);
            domainWindow.WindowMode = DomainWindowMode.AddRemoveDomainOfBook;
            List<DomainWithCheked> domainsWithChecked = new List<DomainWithCheked>();
            foreach (DomainDTO domain in Book.Domain)
	        {
                domainsWithChecked.Add(new DomainWithCheked
                    {
                        Id = domain.Id,
                        Name = domain.Name
                    });
	        }
            domainWindow.DomainsInBook = domainsWithChecked;
            domainWindow.BookId = Book.Id;
            domainWindow.ShowDialog();
            
            Book.Domain = new System.Collections.ObjectModel.Collection<DomainDTO>();
            foreach (DomainWithCheked domain in domainWindow.DomainsInBook)
            {
                Book.Domain.Add(new DomainDTO { Id = domain.Id, Name = domain.Name });
            }
            Domain.DataContext = null;
            Domain.DataContext = Book;
        }

        private void EditAuthors_Click(object sender, RoutedEventArgs e)
        {
            AuthorsWindow authorWindow = new AuthorsWindow();
            authorWindow.SetTitle(Book.Name);
            authorWindow.WindowMode = AuthorWindowMode.AddRemoveAuthorOfBook;
            List<AuthorWithCheked> authorsWithChecked = new List<AuthorWithCheked>();
            foreach (AuthorDTO author in Book.Authors)
            {
                authorsWithChecked.Add(new AuthorWithCheked
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Patronymic = author.Patronymic
                });
            }
            authorWindow.AuthorsInBook = authorsWithChecked;
            authorWindow.BookId = Book.Id;
            authorWindow.ShowDialog();

            Book.Authors = new System.Collections.ObjectModel.Collection<AuthorDTO>();
            foreach (AuthorWithCheked author in authorWindow.AuthorsInBook)
            {
                Book.Authors.Add(new AuthorDTO 
                { Id = author.Id, FirstName = author.FirstName, LastName = author.LastName, Patronymic = author.Patronymic });
            }
            Authors.DataContext = null;
            Authors.DataContext = Book;
        }

        
    }
}
