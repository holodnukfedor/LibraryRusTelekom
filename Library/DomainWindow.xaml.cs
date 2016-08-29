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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Threading;

namespace Library
{
    public class DomainWithCheked : DomainDTO
    {
        public bool IsChecked { set; get; }
    }
    public enum DomainWindowMode
    {
        Edit,
        AddRemoveDomainOfBook
    }
    /// <summary>
    /// Interaction logic for DomainWindow.xaml
    /// </summary>
    public partial class DomainWindow : Window
    {
        public DomainWindowMode WindowMode { set; get; }
        public List<DomainWithCheked> DomainsInBook { set; get; }
        public List<DomainWithCheked> DomainsNotInBook { set; get; }
        public int BookId { set; get; }

        private static LibraryServiceClient libraryService = new LibraryServiceClient();
        private Timer editHelperCloseTimer;
        private PaginationInfo paginationInfo = new PaginationInfo { Amount = 10, FromRow = 0, OrderPropertyName = "Name", SortOrder = SortOrder.ASC };
        private int AllRowsCount {set;get;}
        private int PageCount { set; get; }
        private List<int> WillBeDeletedDomains { set; get; }

        private List<DomainDTO> DomainsList { set; get; }
        private string ErrorInfo { set; get; }

        public void SetTitle(string bookName)
        {
            this.Title = "Редактирование предметных областей книги " + bookName;
            TitleWithBookName.Content = "Предметные области книги " + bookName;
        }

        public int DomainPage
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
                LoadDomainList();
            }
        }

        public DomainWindow()
        {
            InitializeComponent();

            try
            {
                LoadDomainList();
                DomainPageNumber.DataContext = this;
                editHelperCloseTimer = new Timer(EditHelperClosingCallBack, null, Timeout.Infinite, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDomainList()
        {
              DomainListDTO dto;
            if (WindowMode == DomainWindowMode.Edit)
            {
                dto = libraryService.GetDomainsList(paginationInfo, null);
            }
            else
            {
                Collection<int> collection = new Collection<int>();
                foreach (DomainWithCheked domain in DomainsInBook)
                {
                    collection.Add(domain.Id);
                }
                dto = libraryService.GetDomainsList(paginationInfo, collection);
            }
                if (dto.ErrorMessage != String.Empty)
                    throw new Exception(dto.ErrorMessage);

                DomainsList = dto.Domains.ToList();
                AllRowsCount = dto.AllRowsCount;

                DomainListGrid.ItemsSource = null;
                
                if (WindowMode == DomainWindowMode.Edit)
                {
                    DomainListGrid.ItemsSource = DomainsList;
                }
                else
                {
                    DomainsNotInBook = new List<DomainWithCheked>();
                    DomainsList.ForEach(d => DomainsNotInBook.Add(new DomainWithCheked { Id = d.Id, Name = d.Name}));
                    DomainListGrid.ItemsSource = DomainsNotInBook;
                }
                PageCount = AllRowsCount / paginationInfo.Amount;
                if ( AllRowsCount % paginationInfo.Amount > 0)
                    ++PageCount;
                if (PageCount == 0)
                    PageCount = 1;
                DomainPageCount.Text = " " + PageCount + " ";
                WillBeDeletedDomains = new List<int>();
        }

        private void DomainListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Remove: 
                    DomainDTO oldDomain = e.OldItems[0] as DomainDTO;
                    libraryService.DeleteDomain(oldDomain.Id);
                    LoadDomainList();
                    break;
            }
        }

        private void LeftPagingDomainBtn_Click(object sender, RoutedEventArgs e)
        {
            --DomainPage;
            DomainPageNumber.Text = DomainPage.ToString();
            LoadDomainList();
        }

        private void RightPagingDomainBtn_Click(object sender, RoutedEventArgs e)
        {
            ++DomainPage;
            DomainPageNumber.Text = DomainPage.ToString();
            LoadDomainList();
        }


        private void AddDomainBtn_Click(object sender, RoutedEventArgs e)
        {
            DomainListGrid.CommitEdit();
            DomainsList.Add(new DomainDTO());
            DomainListGrid.ItemsSource = null;
            DomainListGrid.ItemsSource = DomainsList;
        }

        private void DeleteDomainBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = DomainListGrid.SelectedIndex;
            if (index < 0 || index >= DomainsList.Count)
            {
                MessageBox.Show("Выберите строку");
                return;
            }
                
            DomainListGrid.CommitEdit();
            if (DomainsList.Count > 0)
            {
                WillBeDeletedDomains.Add(DomainsList.ElementAt(index).Id);
                DomainsList.RemoveAt(index);
                DomainListGrid.ItemsSource = null;
                DomainListGrid.ItemsSource = DomainsList;
            }
        }

        private void SaveChangesBtn_Click(object sender, RoutedEventArgs e)
        {
            DomainListGrid.CommitEdit();
            foreach (DomainDTO domain in DomainsList)
            {
                if (domain.Id == 0)
                {
                    ErrorInfo = libraryService.CreateDomain(domain);
                    if (ErrorInfo != String.Empty)
                        MessageBox.Show(ErrorInfo);
                }

                else
                {
                    ErrorInfo = libraryService.UpdateDomain(domain);
                    if (ErrorInfo != String.Empty)
                        MessageBox.Show(ErrorInfo);
                }
            }
            foreach (int id in WillBeDeletedDomains)
            {
                ErrorInfo = libraryService.DeleteDomain(id);
                if (ErrorInfo != String.Empty)
                    MessageBox.Show(ErrorInfo);

            }
            DomainPage = 1;
            DomainPageNumber.Text = "1";
            LoadDomainList();
        }

        private void AmountOnPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            paginationInfo.Amount = (int) AmountOnPage.SelectedItem;
            DomainPage = 1;
            DomainPageNumber.Text = "1";
            LoadDomainList();
            
        }

        private void DomainListGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Название")
                paginationInfo.OrderPropertyName = "Name";
            else paginationInfo.OrderPropertyName = "Id";
            if (paginationInfo.SortOrder == SortOrder.ASC)
                paginationInfo.SortOrder = SortOrder.DESC;
            else paginationInfo.SortOrder = SortOrder.ASC;
            e.Handled = true;
            LoadDomainList();
        }

        

        private void EditDomainBtn_Click(object sender, RoutedEventArgs e)
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

        private void DomainWindowExemple_Loaded(object sender, RoutedEventArgs e)
        {
            if (WindowMode == DomainWindowMode.Edit)
            {
                TitleWithBookName.Visibility = System.Windows.Visibility.Collapsed;
                DomainInBookListGrid.Visibility = System.Windows.Visibility.Collapsed;
                RemoveDomainPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                EditModeButtonsPanel.Visibility = System.Windows.Visibility.Collapsed;
                columnName.IsReadOnly = true;
                columnNotInBookChecked.Visibility = System.Windows.Visibility.Visible;
                DomainInBookListGrid.ItemsSource = DomainsInBook;
                AddDomainToBookButtonsPanel.Visibility = System.Windows.Visibility.Visible;
                LoadDomainList();
            }
        }

        private void AddDomainToBookBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (DomainWithCheked domain in DomainsNotInBook)
	        {
                if (domain.IsChecked)
                {
                    domain.IsChecked = false;
                    if (BookId != 0)
                    {
                        libraryService.AddDomainToBook(domain.Id, BookId);
                    }
                    DomainsInBook.Add(domain);
                }
	        }
            DomainInBookListGrid.ItemsSource = null;
            DomainInBookListGrid.ItemsSource = DomainsInBook;
            LoadDomainList();
        }

        private void RemoveDomainFromBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BookId != 0)
            {
                foreach (DomainWithCheked domain in DomainsInBook.Where(d => d.IsChecked))
                {
                    libraryService.RemoveDomainFromBook(domain.Id, BookId);
                }
            }
            DomainsInBook = DomainsInBook.Where(d => d.IsChecked == false).ToList();
            DomainInBookListGrid.ItemsSource = null;
            DomainInBookListGrid.ItemsSource = DomainsInBook;
            LoadDomainList();
        }
    }
}
