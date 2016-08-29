using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDAL.Repositories.Interfaces;
using LibraryDAL.Sorting;
using LibraryDAL.Pagination;

namespace LibraryDAL.Repositories
{
    public class PublisherRepository : IRepository<Publisher>
    {
        private LibraryRusTelekonEntities _context;

        public PublisherRepository(LibraryRusTelekonEntities context)
        {
            _context = context;
        }

        public void Create(Publisher publisher)
        {
            _context.Publisher.Add(publisher);
        }

        public void Update(Publisher publisher)
        {
            _context.Entry(publisher).State = System.Data.Entity.EntityState.Modified;
        }

        public Publisher Get(int id)
        {
            return _context.Publisher.Find(id);
        }

        public void Delete(int id)
        {
            Publisher publisher = Get(id);
            if (publisher != null)
                _context.Publisher.Remove(publisher);
        }

        public IQueryable<Publisher> GetAmount(PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.Publisher.OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.Publisher.OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }

        public IQueryable<Publisher> GetAll(String orderPropertyName = "Name", SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.DESC)
            {
                return _context.Publisher.OrderByDescending(orderPropertyName);
            }
            else return _context.Publisher.OrderBy(orderPropertyName);
        }

        public Int32 Count()
        {
            return _context.Publisher.Count();
        }

        public Int32 Count(Func<Publisher, bool> predicate)
        {
            return _context.Publisher.Count(predicate);
        }

        public IQueryable<Publisher> FindAmount(Func<Publisher, Boolean> predicate, PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.Publisher.Where(predicate).AsQueryable().OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.Publisher.Where(predicate).AsQueryable().OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }
    }
}
