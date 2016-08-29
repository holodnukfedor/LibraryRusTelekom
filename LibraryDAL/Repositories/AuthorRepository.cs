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
    public class AuthorRepository : IRepository<Author>
    {
        private LibraryRusTelekonEntities _context;

        public AuthorRepository(LibraryRusTelekonEntities context)
        {
            _context = context;
        }

        public void Create(Author author)
        {
            _context.Author.Add(author);
        }

        public void Update(Author author)
        {
            _context.Entry(author).State = System.Data.Entity.EntityState.Modified;
        }

        public Author Get(int id)
        {
            return _context.Author.Find(id);
        }

        public void Delete(int id)
        {
            Author author = Get(id);
            if (author != null)
                _context.Author.Remove(author);
        }

        public IQueryable<Author> GetAmount(PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.Author.OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.Author.OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }

        public IQueryable<Author> GetAll(String orderPropertyName = "Name", SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.DESC)
            {
                return _context.Author.OrderByDescending(orderPropertyName);
            }
            else return _context.Author.OrderBy(orderPropertyName);
        }

        public Int32 Count()
        {
            return _context.Author.Count();
        }

        public Int32 Count(Func<Author, bool> predicate)
        {
            return _context.Author.Count(predicate);
        }

        public IQueryable<Author> FindAmount(Func<Author, Boolean> predicate, PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.Author.Where(predicate).AsQueryable().OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.Author.Where(predicate).AsQueryable().OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }
    }
}
