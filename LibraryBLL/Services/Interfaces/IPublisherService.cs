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
    public interface IPublisherService : IDisposable
    {
        List<PublisherDTO> GetPublishers(PaginationInfo paginationInfo, out int count, out string errorMessage);
        string AddPublisher(PublisherDTO publisher);
        string DeletePublisher(int id);
        string EditPublisher(PublisherDTO publisher);
    }
}
