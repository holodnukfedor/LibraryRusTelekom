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
    public class BookRepository : IRepository<Book>
    {
         private LibraryRusTelekonEntities _context;

         public BookRepository(LibraryRusTelekonEntities context)
        {
            _context = context;
        }

         public void Create(Book book)
        {
            _context.Book.Add(book);
        }

         public void Update(Book book)
        {
            _context.Entry(book).State = System.Data.Entity.EntityState.Modified;
        }

         public Book Get(int id)
        {
            return _context.Book.Find(id);
        }

        public void Delete(int id)
        {
            Book book = Get(id);
            if (book != null)
                _context.Book.Remove(book);
        }

        public IQueryable<Book> GetAmount(PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.Book.OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.Book.OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }

        public IQueryable<Book> GetAll(String orderPropertyName = "Name", SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.DESC)
            {
                return _context.Book.OrderByDescending(orderPropertyName);
            }
            else return _context.Book.OrderBy(orderPropertyName);
        }

        public Int32 Count()
        {
            return _context.Book.Count();
        }

        public Int32 Count(Func<Book, bool> predicate)
        {
            return _context.Book.Count(predicate);
        }

        public IQueryable<Book> FindAmount(Func<Book, Boolean> predicate, PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.Book.Where(predicate).AsQueryable().OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.Book.Where(predicate).AsQueryable().OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }
    }
}
