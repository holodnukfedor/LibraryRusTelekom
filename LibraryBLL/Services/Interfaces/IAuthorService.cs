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
    public interface IAuthorService : IDisposable
    {
        List<AuthorDTO> GetAuthors(PaginationInfo paginationInfo, List<int> authorsInBookIds, out int count, out string errorMessage);
        string AddAuthor(AuthorDTO author);
        string DeleteAuthor(int id);
        string EditAuthor(AuthorDTO author);
    }
}
