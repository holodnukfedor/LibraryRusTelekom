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
    public class BookAuthorRepository : IRepository<BookAuthor>
    {
        private LibraryRusTelekonEntities _context;

        public BookAuthorRepository(LibraryRusTelekonEntities context)
        {
            _context = context;
        }

        public void Create(BookAuthor bookAuthor)
        {
            _context.BookAuthor.Add(bookAuthor);
        }

        public void Update(BookAuthor bookAuthor)
        {
            _context.Entry(bookAuthor).State = System.Data.Entity.EntityState.Modified;
        }

        public BookAuthor Get(int id)
        {
            return _context.BookAuthor.Find(id);
        }

        public void Delete(int id)
        {
            BookAuthor bookAuthor = Get(id);
            if (bookAuthor != null)
                _context.BookAuthor.Remove(bookAuthor);
        }

        public IQueryable<BookAuthor> GetAmount(PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.BookAuthor.OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.BookAuthor.OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }

        public IQueryable<BookAuthor> GetAll(String orderPropertyName = "Name", SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.DESC)
            {
                return _context.BookAuthor.OrderByDescending(orderPropertyName);
            }
            else return _context.BookAuthor.OrderBy(orderPropertyName);
        }

        public Int32 Count()
        {
            return _context.BookAuthor.Count();
        }

        public Int32 Count(Func<BookAuthor, bool> predicate)
        {
            return _context.BookAuthor.Count(predicate);
        }

        public IQueryable<BookAuthor> FindAmount(Func<BookAuthor, Boolean> predicate, PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.BookAuthor.Where(predicate).AsQueryable().OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.BookAuthor.Where(predicate).AsQueryable().OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }
    }
}
