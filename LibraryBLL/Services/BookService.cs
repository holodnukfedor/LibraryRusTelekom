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

namespace LibraryBLL.Services
{
    public class BookService : IBookService
    {
        IUnitOfWork Database { get; set; }

        public BookService(IUnitOfWork database)
        {
            Database = database;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public List<BookDTO> GetBooks(PaginationInfo paginationInfo, out int count, out string errorMessage)
        {

            List<BookDTO> bookDTOs = new List<BookDTO>();
            errorMessage = String.Empty;
            count = 0;

            try
            {
                List<Book> books = Database.Books.GetAmount(paginationInfo).ToList();
                count = Database.Books.Count();

                foreach (Book book in books)
                {
                    BookDTO bookDTO = new BookDTO()
                    {
                        Id = book.Id,
                        Name = book.Name,
                        Annotation = book.Annotation,
                        Publisher = book.Publisher.Name,
                        YearOfPublication = book.YearOfPublication.Year,
                        Count = book.Count,
                        AmountOfFree = book.AmountOfFree
                    };
                    PaginationInfo allPaginationInfo = new PaginationInfo { FromRow = 0, Amount = int.MaxValue, SortOrder = SortOrder.ASC, OrderPropertyName = "Id" };
                    List<int> authorIds = Database.BookAuthors.FindAmount(a => a.BookId == book.Id, allPaginationInfo).Select(a => a.AuthorId).ToList();

                    allPaginationInfo.OrderPropertyName = "LastName";
                    List<Author> autors = Database.Authors.FindAmount(a => authorIds.Contains(a.Id), allPaginationInfo).ToList();
                    List<AuthorDTO> authorDtos = new List<AuthorDTO>();
                    autors.ForEach(a => authorDtos.Add(new AuthorDTO { Id = a.Id, FirstName = a.FirstName, LastName = a.LastName, Patronymic = a.Patronymic}));
                    bookDTO.Authors = authorDtos;


                    allPaginationInfo.OrderPropertyName = "Id";
                    List<int> domainIds = Database.BookDomains.FindAmount(d => d.BookId == book.Id, allPaginationInfo).Select(d => d.DomainId).ToList();

                    allPaginationInfo.OrderPropertyName = "Name";

                    List<Domain> domains = Database.Domains.FindAmount(d => domainIds.Contains(d.Id), allPaginationInfo).ToList();
                    List<DomainDTO> domainDtos = new List<DomainDTO>();
                    domains.ForEach(d => domainDtos.Add(new DomainDTO {Id = d.Id, Name = d.Name}));
                    bookDTO.Domain = domainDtos;
                    bookDTOs.Add(bookDTO);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return bookDTOs;
        }

        public string AddAuthor(int authorId, int bookId)
        {
            try
            {
                if (Database.BookAuthors.Count(b => b.AuthorId == authorId && b.BookId == bookId) == 0)
                {
                    Database.BookAuthors.Create(new BookAuthor { AuthorId = authorId, BookId = bookId });
                    Database.Save();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string RemoveAuthor(int authorId, int bookId)
        {
            try
            {
                PaginationInfo allPaginationInfo = new PaginationInfo { FromRow = 0, Amount = int.MaxValue, OrderPropertyName = "Id", SortOrder = SortOrder.ASC};
                int id = Database.BookAuthors.FindAmount(b => b.AuthorId == authorId && b.BookId == bookId, allPaginationInfo).First().Id;
                Database.BookAuthors.Delete(id);
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }
        public string AddDomain(int domainId, int bookId)
        {
            try
            {
                if (Database.BookDomains.Count(b => b.DomainId == domainId && b.BookId == bookId) == 0)
                {
                    Database.BookDomains.Create(new BookDomain { DomainId = domainId, BookId = bookId });
                    Database.Save();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string RemoveDomain(int domainId, int bookId)
        {
            try
            {
                PaginationInfo allPaginationInfo = new PaginationInfo { FromRow = 0, Amount = int.MaxValue, OrderPropertyName = "Id", SortOrder = SortOrder.ASC };
                int id = Database.BookDomains.FindAmount(b => b.DomainId == domainId && b.BookId == bookId, allPaginationInfo).First().Id;
                Database.BookDomains.Delete(id);
                Database.Save();
            }
             catch (Exception ex)
             {
                 return ex.Message;
             }
             return String.Empty;
        }

        public string ChangePublisher(int publisherId, int bookId)
        {
            try
            {
                Book book = Database.Books.Get(bookId);
                book.PublisherId = publisherId;
                Database.Books.Update(book);
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string CreateBook(BookEditingInfo bookEditingInfo, int publisherId, List<int> domainIds, List<int> authorIds)
        {
            Book book = new Book
            {
                Annotation = bookEditingInfo.Annotation,
                Name = bookEditingInfo.Name,
                YearOfPublication = new DateTime(bookEditingInfo.YearOfPublication, 1, 1),
                Count = bookEditingInfo.Count,
                AmountOfFree = bookEditingInfo.Count,
                PublisherId = publisherId
            };
            try
            {
                Database.Books.Create(book);
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            try
            {
                domainIds.ForEach(d => Database.BookDomains.Create(new BookDomain { BookId = book.Id, DomainId  = d}));
                authorIds.ForEach(a => Database.BookAuthors.Create(new BookAuthor { BookId = book.Id, AuthorId = a }));
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return String.Empty;
        }

        public string EditBook(BookEditingInfo bookEditingInfo)
        {
            try
            {
                Book book = Database.Books.Get(bookEditingInfo.Id);
                book.Name = bookEditingInfo.Name;
                book.YearOfPublication = new DateTime(bookEditingInfo.YearOfPublication, 1, 1);
                book.Annotation = bookEditingInfo.Annotation;

                if (bookEditingInfo.Count > book.Count)
                {
                    book.AmountOfFree += bookEditingInfo.Count - book.Count;
                }
                else
                {
                    book.AmountOfFree -= book.Count - bookEditingInfo.Count;
                }
                book.Count = bookEditingInfo.Count;
                Database.Books.Update(book);
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string DeleteBook(int id)
        {
            try
            {
                Database.Books.Delete(id);
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public ReportBookPopularity GetBookPopularityReport(DateTime firstDate, DateTime lastDate, out string errorMessage)
        {
            errorMessage = String.Empty;
            ReportBookPopularity report = new ReportBookPopularity
            {
                FirstDate = firstDate,
                LastDate = lastDate
            };

            report.BookInfos = Database.GetBookPopularityReport(firstDate, lastDate, out errorMessage);

            return report;
        }
    }
}
