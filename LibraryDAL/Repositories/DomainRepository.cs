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
    public class DomainRepository : IRepository<Domain>
    {
        private LibraryRusTelekonEntities _context;

        public DomainRepository(LibraryRusTelekonEntities context)
        {
            _context = context;
        }

        public void Create(Domain domain)
        {
            _context.Domain.Add(domain);
        }

        public void Update(Domain domain)
        {
            _context.Entry(domain).State = System.Data.Entity.EntityState.Modified;
        }

        public Domain Get(int id)
        {
            return _context.Domain.Find(id);
        }

        public void Delete(int id)
        {
            Domain domain = Get(id);
            if (domain != null)
                _context.Domain.Remove(domain);
        }

        public IQueryable<Domain> GetAmount(PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.Domain.OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.Domain.OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }

        public IQueryable<Domain> GetAll(String orderPropertyName = "Name", SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.DESC)
            {
                return _context.Domain.OrderByDescending(orderPropertyName);
            }
            else return _context.Domain.OrderBy(orderPropertyName);
        }

        public Int32 Count()
        {
            return _context.Domain.Count();
        }

        public Int32 Count(Func<Domain, bool> predicate)
        {
            return _context.Domain.Count(predicate);
        }

        public IQueryable<Domain> FindAmount(Func<Domain, Boolean> predicate, PaginationInfo paginationInfo)
        {
            if (paginationInfo.SortOrder == SortOrder.DESC)
            {
                return _context.Domain.Where(predicate).AsQueryable().OrderByDescending(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
            }
            else return _context.Domain.Where(predicate).AsQueryable().OrderBy(paginationInfo.OrderPropertyName).Skip(paginationInfo.FromRow).Take(paginationInfo.Amount);
        }
    }
}
