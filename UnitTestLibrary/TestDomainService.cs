using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using LibraryBLL.DTO;
using LibraryBLL.Services;
using LibraryBLL.Services.Interfaces;
using LibraryDAL.Repositories;
using LibraryDAL.Repositories.Interfaces;
using LibraryDAL;
using System.Collections.Generic;
using LibraryBLL;
using LibraryDAL.Sorting;
using LibraryDAL.Pagination;
using System.Linq;

namespace UnitTestLibrary
{
    [TestClass]
    public class TestDomainService
    {
        private IDomainService _domainService;
        private static PaginationInfo allPaginationInfo = new PaginationInfo
        {
            FromRow = 0,
            Amount = int.MaxValue,
            SortOrder = SortOrder.ASC,
            OrderPropertyName = "Name"
        };

        public TestDomainService()
        {
            _domainService = new DomainService(new EFUnitOfWork());
        }

        [TestMethod]
        public void GetDomains()
        {
            string errorMessage = String.Empty;
            int count;

            List<DomainDTO> domains = _domainService.GetDomains(allPaginationInfo, new List<int> { 5, 2 }, out count, out errorMessage);

            Assert.AreEqual(errorMessage, String.Empty);

            foreach (DomainDTO domain in domains)
            {
                System.Diagnostics.Debug.WriteLine(domain.Id + " " + domain.Name);
            }
        }

        [TestMethod]
        public void AddDomain()
        {
            string errorMessage = _domainService.AddDomain(new DomainDTO { Name = "Детективы"});
            Assert.AreEqual(errorMessage, String.Empty);       
        }

        [TestMethod]
        public void DeleteDomain()
        {
            string errorMessage = _domainService.DeleteDomain(7);
            Assert.AreEqual(errorMessage, String.Empty);   
        }

        [TestMethod]
        public void EditDomain()
        {
            string errorMessage = _domainService.EditDomain(new DomainDTO { Id = 4, Name = "Научная фантастика"});
            Assert.AreEqual(errorMessage, String.Empty);
        }
    }
}
