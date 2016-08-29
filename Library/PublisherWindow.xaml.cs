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

namespace Library
{
    public enum PublisherWindowMode
    {
        Edit,
        ChangePublisherOfBook
    }
    /// <summary>
    /// Interaction logic for PublisherWindow.xaml
    /// </summary>
    public partial class PublisherWindow : Window
    {
        public PublisherWindowMode WindowMode { set; get; }
        public PublisherDTO NewPublisher { set; get; }
        private static LibraryServiceClient libraryService = new LibraryServiceClient();
        private Timer editHelperCloseTimer;
        private PaginationInfo paginationInfo = new PaginationInfo { Amount = 10, FromRow = 0, OrderPropertyName = "Name", SortOrder = SortOrder.ASC };
        private int AllRowsCount {set;get;}
        private int PageCount { set; get; }
        private List<int> WillBeDeletedEntitiesIds { set; get; }

        private List<PublisherDTO> EntitiesList { set; get; }
        private string ErrorInfo { set; get; }

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

        public PublisherWindow()
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
              PublisherDTOList dto = libraryService.GetPublishersList(paginationInfo);
                if (dto.ErrorMessage != String.Empty)
                    throw new Exception(dto.ErrorMessage);

                EntitiesList = dto.Publishers.ToList();
                AllRowsCount = dto.AllRowsCount;

                EntityListGrid.ItemsSource = null;
                EntityListGrid.ItemsSource = EntitiesList;
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
            EntitiesList.Add(new PublisherDTO());
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
            foreach (PublisherDTO entity in EntitiesList)
            {
                if (entity.Id == 0)
                {
                    ErrorInfo = libraryService.CreatePublisher(entity);
                    if (ErrorInfo != String.Empty)
                        MessageBox.Show(ErrorInfo);
                }
                else
                {
                    ErrorInfo = libraryService.UpdatePublisher(entity);
                    if (ErrorInfo != String.Empty)
                        MessageBox.Show(ErrorInfo);
                }
            }
            foreach (int id in WillBeDeletedEntitiesIds)
            {
                ErrorInfo = libraryService.DeletePublisher(id);
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

        private void DomainListGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "Id":
                    paginationInfo.OrderPropertyName = "Id";
                    break;
                case "Имя":
                    paginationInfo.OrderPropertyName = "Name";
                    break;
                case "Информация":
                    paginationInfo.OrderPropertyName = "Info";
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

        public void SetTitle(string title)
        {
            this.Title = title;
            TitleLbl.Content = title;
        }

        private void ChangePublisherBtn_Click(object sender, RoutedEventArgs e)
        {
            PublisherDTO publisher = EntityListGrid.SelectedItem as PublisherDTO;
            if (publisher == null)
            {
                MessageBox.Show("Выберите строку");
                return;
            }
            NewPublisher = publisher;
            DialogResult = true;
            this.Close();
        }

        private void PublisherWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (WindowMode == PublisherWindowMode.Edit)
            {
                ChangePublisherBtn.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                AddEntityBtn.Visibility = System.Windows.Visibility.Collapsed;
                EditEntityBtn.Visibility = System.Windows.Visibility.Collapsed;
                DeleteEntityBtn.Visibility = System.Windows.Visibility.Collapsed;
                SaveChangesBtn.Visibility = System.Windows.Visibility.Collapsed;
                columnName.IsReadOnly = true;
                columnInfo.IsReadOnly = true;
            }
        }
    }
}
