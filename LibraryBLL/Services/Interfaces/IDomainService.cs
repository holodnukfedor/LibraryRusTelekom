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
    public interface IDomainService : IDisposable
    {
        List<DomainDTO> GetDomains(PaginationInfo paginationInfo, List<int> domainInBookIds, out int count, out string errorMessage);
        string AddDomain(DomainDTO domain);
        string DeleteDomain(int id);
        string EditDomain(DomainDTO domain);
    }
}
