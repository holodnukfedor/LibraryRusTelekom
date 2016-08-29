using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryBLL.Services.Interfaces;
using LibraryBLL.DTO;
using LibraryDAL.Pagination;
using LibraryDAL.Sorting;
using LibraryDAL.Repositories;
using LibraryDAL.Repositories.Interfaces;
using LibraryDAL;
using System.Data.Entity.Validation;
namespace LibraryBLL.Services
{
    public class AbonentService : IAbonentService
    {
        IUnitOfWork Database { get; set; }

        public AbonentService(IUnitOfWork database)
        {
            Database = database;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public List<Abonent> GetAbonents(PaginationInfo paginationInfo, out int count, out string errorMessage)
        {
            errorMessage = String.Empty;
            count = 0;
            try
            {
                count = Database.Abonents.Count();
                return Database.Abonents.GetAmount(paginationInfo).ToList();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }

        public List<UserBookDTO> GetBooksOfAbonent(int id, PaginationInfo paginationInfo, out int count, out string errorMessage)
        {
            List<UserBookDTO> bookDTOs = new List<UserBookDTO>();
            errorMessage = String.Empty;
            count = 0;
            try
            {
                PaginationInfo allPaginationInfo = new PaginationInfo { FromRow = 0, Amount = int.MaxValue, SortOrder = SortOrder.ASC, OrderPropertyName = "Id" };
                List<BookAbonent> bookAbonents = Database.BookAbonents.FindAmount(b => b.AbonentId == id && b.ReturnDate == null, allPaginationInfo).ToList();
                allPaginationInfo.OrderPropertyName = "Id";
                foreach (BookAbonent bookAbonent in bookAbonents)
                {
                    UserBookDTO userBookDto = new UserBookDTO
                    {
                        BookAbonentId = bookAbonent.Id,
                        BookId = bookAbonent.BookId,
                        Name = bookAbonent.Book.Name,
                        YearOfPublication = bookAbonent.Book.YearOfPublication.Year,
                        Annotation = bookAbonent.Book.Annotation,
                        DateOfReceiving = bookAbonent.DateOfReceiving
                    };
                    
                    List<String> domainsNames = Database.BookDomains.FindAmount(d => d.BookId == bookAbonent.BookId, allPaginationInfo).Select(d => d.Domain.Name).ToList();
                    userBookDto.Domain = String.Join(" ", domainsNames);

                    List<Author> authors = Database.BookAuthors.FindAmount(a => a.BookId == bookAbonent.BookId, allPaginationInfo).Select(a => a.Author).ToList();
                    StringBuilder authorNames = new StringBuilder(String.Empty);
                    foreach (Author author in authors)
                    {
                        authorNames.Append(author.LastName + " " + author.FirstName.Substring(0, 1) + ". " + author.Patronymic.Substring(0, 1) + ". "); 
                    }
                    userBookDto.Authors = authorNames.ToString();
                    userBookDto.Publisher = Database.Books.Get(bookAbonent.BookId).Publisher.Name;
                    bookDTOs.Add(userBookDto);
                }
        

              
            }
            catch (Exception ex)
            {
                errorMessage += ex.Message;
            }
            return bookDTOs;
        }

        public string AddAbonent(Abonent abonent)
        {
            try
            {
                Database.Abonents.Create(abonent);
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string DeleteAbonent(int id)
        {
            try
            {
                Database.Abonents.Delete(id);
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string EditAbonent(Abonent abonent)
        {
            try
            {
                Database.Abonents.Update(abonent);
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string GiveBook(int bookId, int abonentId)
        {
            try
            {
                Book book = Database.Books.Get(bookId);
                if (book.AmountOfFree > 0)
                {
                    Database.BookAbonents.Create(new BookAbonent { AbonentId = abonentId, BookId = bookId, DateOfReceiving = DateTime.Now });
                    --book.AmountOfFree;
                    Database.Books.Update(book);
                    Database.Save();
                }
                    
            }
            catch (DbEntityValidationException ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string RemoveBook(int bookId, int abonentId)
        {
            try
            {
                PaginationInfo allPaginationInfo = new PaginationInfo { FromRow = 0, Amount = int.MaxValue, OrderPropertyName = "Id", SortOrder = SortOrder.ASC };
                BookAbonent bookAbonent = Database.BookAbonents
                    .FindAmount(b => b.BookId == bookId && b.AbonentId == abonentId && b.ReturnDate == null, allPaginationInfo).First();
                bookAbonent.ReturnDate = DateTime.Now;
                Database.BookAbonents.Update(bookAbonent);

                Book book = Database.Books.Get(bookId);
                ++book.AmountOfFree;
                Database.Books.Update(book);

                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }
    }
}
