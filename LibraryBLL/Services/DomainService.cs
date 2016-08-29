using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryBLL.Services.Interfaces;
using LibraryBLL.DTO;
using LibraryDAL.Pagination;
using LibraryDAL.Sorting;
using LibraryDAL.Repositories;
using LibraryDAL.Repositories.Interfaces;
using LibraryDAL;

namespace LibraryBLL.Services
{
    public class DomainService : IDomainService
    {
        IUnitOfWork Database { get; set; }

        public DomainService(IUnitOfWork database)
        {
            Database = database;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public List<DomainDTO> GetDomains(PaginationInfo paginationInfo, List<int> domainInBookIds, out int count, out string errorMessage)
        {
            errorMessage = String.Empty;
            count = 0;

            try
            {
                List<Domain> domains = null;

                if (domainInBookIds != null && domainInBookIds.Count > 0)
                {
                    domains = Database.Domains.FindAmount(d => !domainInBookIds.Contains(d.Id), paginationInfo).ToList();
                    count = Database.Domains.Count(d => !domainInBookIds.Contains(d.Id));
                }

                else
                {
                    domains = Database.Domains.GetAmount(paginationInfo).ToList();
                    count = Database.Domains.Count();
                }

                List<DomainDTO> dtos = new List<DomainDTO>();
                if (domains != null)
                    domains.ForEach(d => dtos.Add(new DomainDTO { Id = d.Id, Name = d.Name}));
                return dtos;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }

        public string AddDomain(DomainDTO domain)
        {
            try
            {
                Database.Domains.Create(new Domain { Id = domain.Id, Name = domain.Name});
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string DeleteDomain(int id)
        {
            try
            {
                Database.Domains.Delete(id);
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string EditDomain(DomainDTO domain)
        {
            try
            {
                Database.Domains.Update(new Domain { Id = domain.Id, Name = domain.Name });
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }
    }
}
