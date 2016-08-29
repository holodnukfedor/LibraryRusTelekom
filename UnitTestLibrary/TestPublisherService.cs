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
    public class TestPublisherService
    {
        private IPublisherService _publisherService;
        private static PaginationInfo allPaginationInfo = new PaginationInfo
        {
            FromRow = 0,
            Amount = int.MaxValue,
            SortOrder = SortOrder.ASC,
            OrderPropertyName = "Name"
        };

        public TestPublisherService()
        {
            _publisherService = new PublisherService(new EFUnitOfWork());
        }

        [TestMethod]
        public void GetPublishers()
        {
            string errorMessage = String.Empty;

            int count;
            List<PublisherDTO> publishers = _publisherService.GetPublishers(allPaginationInfo, out count, out errorMessage);

            Assert.AreEqual(errorMessage, String.Empty);

            foreach (PublisherDTO publisher in publishers)
            {
                System.Diagnostics.Debug.WriteLine(publisher.Id + " " + publisher.Name + " " + publisher.Info);
            }
        }

        [TestMethod]
        public void AddPublisher()
        {
            string errorMessage = _publisherService.AddPublisher(new PublisherDTO { Name = "Вильямс", Info = "Много о программировании" });
            Assert.AreEqual(errorMessage, String.Empty);       
        }

        [TestMethod]
        public void DeletePublisher()
        {
            string errorMessage = _publisherService.DeletePublisher(4);
            Assert.AreEqual(errorMessage, String.Empty);   
        }

        [TestMethod]
        public void EditPublisher()
        {
            string errorMessage = _publisherService.EditPublisher(new PublisherDTO { Id = 4, Name = "Вильямс", Info = "Много о программировании" });
            Assert.AreEqual(errorMessage, String.Empty);
        }
    }
}
