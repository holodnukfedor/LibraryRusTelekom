using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDAL.Sorting;
using LibraryDAL.Pagination;

namespace LibraryDAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(String orderPropertyName, SortOrder sortOrder);
        IQueryable<T> GetAmount(PaginationInfo paginationInfo);
        IQueryable<T> FindAmount(Func<T, Boolean> predicate, PaginationInfo paginationInfo);
        T Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        Int32 Count();
        Int32 Count(Func<T, Boolean> predicate);
    }
}
