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
using System.Threading;
using Library.LibraryServiceWCF;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Threading;

namespace Library
{
    public class AuthorWithCheked : AuthorDTO
    {
        public bool IsChecked { set; get; }
    }
    public enum AuthorWindowMode
    {
        Edit,
        AddRemoveAuthorOfBook
    }
    /// <summary>
    /// Interaction logic for AuthorsWindow.xaml
    /// </summary>
    public partial class AuthorsWindow : Window
    {
        public AuthorWindowMode WindowMode { set; get; }
        public List<AuthorWithCheked> AuthorsInBook { set; get; }
        public List<AuthorWithCheked> AuthorsNotInBook { set; get; }
        public int BookId { set; get; }

        private static LibraryServiceClient libraryService = new LibraryServiceClient();
        private Timer editHelperCloseTimer;
        private PaginationInfo paginationInfo = new PaginationInfo { Amount = 10, FromRow = 0, OrderPropertyName = "LastName", SortOrder = SortOrder.ASC };
        private int AllRowsCount {set;get;}
        private int PageCount { set; get; }
        private List<int> WillBeDeletedEntitiesIds { set; get; }
        private string ErrorInfo { set; get; }

        private List<AuthorDTO> EntitiesList { set; get; }

        public void SetTitle(string bookName)
        {
            this.Title = "Редактирование авторов книги " + bookName;
            TitleWithBookName.Content = "Авторы книги " + bookName;
        }

        public int EntityPage
        {
            get { return (paginationInfo.FromRow / paginationInfo.Amount + 1); }
            set 
            {
                if (value > PageCount)
                    value = PageCount;
                if (value <= 0)
                    value = 1;
                int fromRow = (value - 1) * paginationInfo.Amount;

                paginationInfo.FromRow = fromRow;
                LoadEntitiesList();
            }
        }

        public AuthorsWindow()
        {
            InitializeComponent();

            try
            {
                LoadEntitiesList();
                EntityPageNumber.DataContext = this;
                editHelperCloseTimer = new Timer(EditHelperClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadEntitiesList()
        {
           AuthorDTOList dto;
            if (WindowMode == AuthorWindowMode.Edit)
            {
                dto = libraryService.GetAuthorsList(paginationInfo, null);
            }
            else
            {
                Collection<int> collection = new Collection<int>();
                foreach (AuthorWithCheked author in AuthorsInBook)
                {
                    collection.Add(author.Id);
                }
                dto = libraryService.GetAuthorsList(paginationInfo, collection);
            }
            if (dto.ErrorMessage != String.Empty)
                throw new Exception(dto.ErrorMessage);

                EntitiesList = dto.Authors.ToList();
                AllRowsCount = dto.AllRowsCount;

                EntityListGrid.ItemsSource = null;

                if (WindowMode == AuthorWindowMode.Edit)
                {
                    EntityListGrid.ItemsSource = EntitiesList;
                }
                else
                {
                    AuthorsNotInBook = new List<AuthorWithCheked>();
                    EntitiesList.ForEach(a => AuthorsNotInBook.Add(new AuthorWithCheked { Id = a.Id, 
                        FirstName = a.FirstName, LastName = a.LastName, Patronymic = a.Patronymic }));
                    EntityListGrid.ItemsSource = AuthorsNotInBook;
                }
                PageCount = AllRowsCount / paginationInfo.Amount;
                if ( AllRowsCount % paginationInfo.Amount > 0)
                    ++PageCount;
                if (PageCount == 0)
                    PageCount = 1;
                EntityPageCount.Text = " " + PageCount + " ";
                WillBeDeletedEntitiesIds = new List<int>();
        }

        private void LeftPagingBtn_Click(object sender, RoutedEventArgs e)
        {
            --EntityPage;
            EntityPageNumber.Text = EntityPage.ToString();
            LoadEntitiesList();
        }

        private void RightPagingBtn_Click(object sender, RoutedEventArgs e)
        {
            ++EntityPage;
            EntityPageNumber.Text = EntityPage.ToString();
            LoadEntitiesList();
        }


        private void AddEntityBtn_Click(object sender, RoutedEventArgs e)
        {
            EntityListGrid.CommitEdit();
            EntitiesList.Add(new AuthorDTO());
            EntityListGrid.ItemsSource = null;
            EntityListGrid.ItemsSource = EntitiesList;
        }

        private void DeleteEntityBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = EntityListGrid.SelectedIndex;
            if (index < 0 || index >= EntitiesList.Count)
            {
                MessageBox.Show("Выберите строку");
                return;
            }

            EntityListGrid.CommitEdit();
            if (EntitiesList.Count > 0)
            {
                WillBeDeletedEntitiesIds.Add(EntitiesList.ElementAt(index).Id);
                EntitiesList.RemoveAt(index);
                EntityListGrid.ItemsSource = null;
                EntityListGrid.ItemsSource = EntitiesList;
            }
        }

        private void SaveChangesBtn_Click(object sender, RoutedEventArgs e)
        {
            EntityListGrid.CommitEdit();
            foreach (AuthorDTO entity in EntitiesList)
            {
                if (entity.Id == 0)
                {
                   ErrorInfo = libraryService.CreateAuthor(entity);
                   if (ErrorInfo != String.Empty)
                       MessageBox.Show(ErrorInfo);
                }
                else
                {
                    ErrorInfo = libraryService.UpdateAuthor(entity);
                    if (ErrorInfo != String.Empty)
                        MessageBox.Show(ErrorInfo);
                }
            }
            foreach (int id in WillBeDeletedEntitiesIds)
            {
                ErrorInfo = libraryService.DeleteAuthor(id);
                if (ErrorInfo != String.Empty)
                    MessageBox.Show(ErrorInfo);
            }
            EntityPage = 1;
            EntityPageNumber.Text = "1";
            LoadEntitiesList();
        }

        private void AmountOnPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            paginationInfo.Amount = (int) AmountOnPage.SelectedItem;
            EntityPage = 1;
            EntityPageNumber.Text = "1";
            LoadEntitiesList();
            
        }

        private void AuthorListGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "Id":
                    paginationInfo.OrderPropertyName = "Id";
                    break;
                case "Фамилия":
                    paginationInfo.OrderPropertyName = "LastName";
                    break;
                case "Имя":
                    paginationInfo.OrderPropertyName = "FirstName";
                    break;
                case "Отчество":
                    paginationInfo.OrderPropertyName = "Patronymic";
                    break;
            }
            if (paginationInfo.SortOrder == SortOrder.ASC)
                paginationInfo.SortOrder = SortOrder.DESC;
            else paginationInfo.SortOrder = SortOrder.ASC;
            e.Handled = true;
            LoadEntitiesList();
        }



        private void EditEntityBtn_Click(object sender, RoutedEventArgs e)
        {
            editHelper.IsOpen = true;
            editHelperCloseTimer.Change(2500, Timeout.Infinite);
        }

        private void EditHelperClosingCallBack(object p_useless)
        {
            Dispatcher.Invoke(() =>
            {
                editHelper.IsOpen = false;
            });
        }

        private void AuthosrWindowExample_Loaded(object sender, RoutedEventArgs e)
        {
            if (WindowMode == AuthorWindowMode.Edit)
            {
                TitleWithBookName.Visibility = System.Windows.Visibility.Collapsed;
                EntityListInBookGrid.Visibility = System.Windows.Visibility.Collapsed;
                RemoveAuthorPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                EditModeButtonsPanel.Visibility = System.Windows.Visibility.Collapsed;

                columnFirstName.IsReadOnly = true;
                columnLastName.IsReadOnly = true;
                columnPatronymic.IsReadOnly = true;

                columnNotInBookChecked.Visibility = System.Windows.Visibility.Visible;
                EntityListInBookGrid.ItemsSource = AuthorsInBook;
                AddAuthorToBookButtonsPanel.Visibility = System.Windows.Visibility.Visible;
                LoadEntitiesList();
            }
        }

        private void RemoveAthorFromBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BookId != 0)
            {
                foreach (AuthorWithCheked author in AuthorsInBook.Where(a => a.IsChecked))
                {
                    libraryService.RemoveAuthorFromBook(author.Id, BookId);
                }
            }
            AuthorsInBook = AuthorsInBook.Where(a => a.IsChecked == false).ToList();
            EntityListInBookGrid.ItemsSource = null;
            EntityListInBookGrid.ItemsSource = AuthorsInBook;
            LoadEntitiesList();
        }

        private void AddAuthorToBookBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (AuthorWithCheked author in AuthorsNotInBook)
            {
                if (author.IsChecked)
                {
                    author.IsChecked = false;
                    if (BookId != 0)
                    {
                        libraryService.AddAuthorToBook(author.Id, BookId);
                    }
                    AuthorsInBook.Add(author);
                }
            }
            EntityListInBookGrid.ItemsSource = null;
            EntityListInBookGrid.ItemsSource = AuthorsInBook;
            LoadEntitiesList();
        }
    }
}
