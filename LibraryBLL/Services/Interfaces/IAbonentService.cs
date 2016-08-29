using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDAL.Pagination;
using LibraryDAL.Sorting;
using LibraryDAL;
using LibraryBLL.DTO;

namespace LibraryBLL.Services.Interfaces
{
    public interface IAbonentService : IDisposable
    {
        List<Abonent> GetAbonents(PaginationInfo paginationInfo, out int count, out string errorMessage);
        List<UserBookDTO> GetBooksOfAbonent(int id, PaginationInfo paginationInfo, out int count, out string errorMessage);
        string AddAbonent(Abonent abonent);
        string DeleteAbonent(int id);
        string EditAbonent(Abonent abonent);
        string GiveBook(int bookId, int abonentId);
        string RemoveBook(int bookId, int abonentId);
    }
}
