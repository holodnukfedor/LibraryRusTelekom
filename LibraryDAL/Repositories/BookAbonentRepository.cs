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
    public class BookAbonentRepository : IRepository<BookAbonent>
    {
         private LibraryRusTelekonEntities _context;

         public BookAbonentRepository(LibraryRusTelekonEntities context)
        {
            _context = context;
        }

         public void Create(BookAbonent bookAbonent)
        {
            _context.BookAbonent.Add(bookAbonent);
        }

         public void Update(BookAbonent bookAbonent)
        {
            _context.Entry(bookAbonent).State = System.Data.Entity.EntityState.Modified;
        }

         public BookAbonent Get(int id)
        {
            return _context.BookAbonent.Find(id);
        }

        public void Delete(int id)
        {
            BookAbonent bookAbonent = Get(id);
            if (bookAbonent != null)
                _context.BookAbonent.Remove(bookAbonent);
        }

        public IQueryable<BookAbonent> GetAmount(PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.BookAbonent.OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.BookAbonent.OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }

        public IQueryable<BookAbonent> GetAll(String orderPropertyName = "Name", SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.DESC)
            {
                return _context.BookAbonent.OrderByDescending(orderPropertyName);
            }
            else return _context.BookAbonent.OrderBy(orderPropertyName);
        }

        public Int32 Count()
        {
            return _context.BookAbonent.Count();
        }

        public Int32 Count(Func<BookAbonent, bool> predicate)
        {
            return _context.BookAbonent.Count(predicate);
        }

        public IQueryable<BookAbonent> FindAmount(Func<BookAbonent, Boolean> predicate, PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.BookAbonent.Where(predicate).AsQueryable().OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.BookAbonent.Where(predicate).AsQueryable().OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }
    }
}
