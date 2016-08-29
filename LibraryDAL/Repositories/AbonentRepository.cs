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
    public class AbonentRepository : IRepository<Abonent>
    {
        private LibraryRusTelekonEntities _context;

        public AbonentRepository(LibraryRusTelekonEntities context)
        {
            _context = context;
        }

        public void Create(Abonent abonent)
        {
            _context.Abonent.Add(abonent);
        }

        public void Update(Abonent abonent)
        {
            _context.Entry(abonent).State = System.Data.Entity.EntityState.Modified;
        }

        public Abonent Get(int id)
        {
            return _context.Abonent.Find(id);
        }

        public void Delete(int id)
        {
            Abonent abonent = Get(id);
            if (abonent != null)
                _context.Abonent.Remove(abonent);
        }

        public IQueryable<Abonent> GetAmount(PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.Abonent.OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.Abonent.OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }

        public IQueryable<Abonent> GetAll(String orderPropertyName = "Name", SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.DESC)
            {
                return _context.Abonent.OrderByDescending(orderPropertyName);
            }
            else return _context.Abonent.OrderBy(orderPropertyName);
        }

        public Int32 Count()
        {
            return _context.Abonent.Count();
        }

        public Int32 Count(Func<Abonent, bool> predicate)
        {
            return _context.Abonent.Count(predicate);
        }

        public IQueryable<Abonent> FindAmount(Func<Abonent, Boolean> predicate, PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.Abonent.Where(predicate).AsQueryable().OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.Abonent.Where(predicate).AsQueryable().OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }
    }
}
