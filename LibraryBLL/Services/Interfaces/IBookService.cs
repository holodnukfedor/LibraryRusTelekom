using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryBLL.DTO;
using LibraryDAL.Pagination;
using LibraryDAL.Sorting;

namespace LibraryBLL.Services.Interfaces
{
    public interface IBookService : IDisposable
    {
        List<BookDTO> GetBooks(PaginationInfo paginationInfo, out int count, out string errorMessage);
        ReportBookPopularity GetBookPopularityReport(DateTime firstDate, DateTime lastDate, out string errorMessage);
        string AddAuthor(int authorId, int bookId);
        string RemoveAuthor(int authorId, int bookId);
        string AddDomain(int domainId, int bookId);
        string RemoveDomain(int domainId, int bookId);
        string ChangePublisher(int publisherId, int bookId);
        string CreateBook(BookEditingInfo bookEditingInfo, int publisherId, List<int> domainIds, List<int> authorIds);
        string EditBook(BookEditingInfo bookEditingInfo);
        string DeleteBook(int id);
    }
}
