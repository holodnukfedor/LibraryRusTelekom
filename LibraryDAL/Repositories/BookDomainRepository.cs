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
    public class BookDomainRepository : IRepository<BookDomain>
    {
         private LibraryRusTelekonEntities _context;

         public BookDomainRepository(LibraryRusTelekonEntities context)
        {
            _context = context;
        }

         public void Create(BookDomain bookDomain)
        {
            _context.BookDomain.Add(bookDomain);
        }

         public void Update(BookDomain bookDomain)
        {
            _context.Entry(bookDomain).State = System.Data.Entity.EntityState.Modified;
        }

         public BookDomain Get(int id)
        {
            return _context.BookDomain.Find(id);
        }

        public void Delete(int id)
        {
            BookDomain bookDomain = Get(id);
            if (bookDomain != null)
                _context.BookDomain.Remove(bookDomain);
        }

        public IQueryable<BookDomain> GetAmount(PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.BookDomain.OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.BookDomain.OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }

        public IQueryable<BookDomain> GetAll(String orderPropertyName = "Name", SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.DESC)
            {
                return _context.BookDomain.OrderByDescending(orderPropertyName);
            }
            else return _context.BookDomain.OrderBy(orderPropertyName);
        }

        public Int32 Count()
        {
            return _context.BookDomain.Count();
        }

        public Int32 Count(Func<BookDomain, bool> predicate)
        {
            return _context.BookDomain.Count(predicate);
        }

        public IQueryable<BookDomain> FindAmount(Func<BookDomain, Boolean> predicate, PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.BookDomain.Where(predicate).AsQueryable().OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.BookDomain.Where(predicate).AsQueryable().OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }
    }
}
