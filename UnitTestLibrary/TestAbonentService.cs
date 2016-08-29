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
    public class TestAbonentService
    {
        private IAbonentService _abonentService;
        private static PaginationInfo allPaginationInfo = new PaginationInfo
        {
            FromRow = 0,
            Amount = int.MaxValue,
            SortOrder = SortOrder.ASC,
            OrderPropertyName = "FirstName"
        };

        public TestAbonentService()
        {
            _abonentService = new AbonentService(new EFUnitOfWork());
        }

        [TestMethod]
        public void GetAbonents()
        {
            string errorMessage = String.Empty;
            int count;
            List<Abonent> abonents = _abonentService.GetAbonents(allPaginationInfo, out count, out errorMessage);

            Assert.AreEqual(errorMessage, String.Empty);

            foreach (Abonent abonent in abonents)
            {
                System.Diagnostics.Debug.WriteLine(abonent.Id + " " + abonent.FirstName + " " + abonent.LastName + " " + abonent.Patronymic);
            }
        }

        [TestMethod]
        public void AddAbonent()
        {
            string errorMessage = _abonentService.AddAbonent(new Abonent { FirstName = "Павел", LastName = "Холоднюк", Patronymic = "Ярославович" });
            Assert.AreEqual(errorMessage, String.Empty);       
        }

        [TestMethod]
        public void DeleteAbonent()
        {
            string errorMessage = _abonentService.DeleteAbonent(2);
            Assert.AreEqual(errorMessage, String.Empty);   
        }

        [TestMethod]
        public void EditAbonent()
        {
            string errorMessage = _abonentService.EditAbonent(new Abonent { Id = 2, FirstName = "Павел", LastName = "Холоднюк", Patronymic = "Ярославович" });
            Assert.AreEqual(errorMessage, String.Empty);
        }

        [TestMethod]
        public void GiveBookToAbonent()
        {
            string errorMessage = _abonentService.GiveBook(1, 3005);
            Assert.AreEqual(errorMessage, String.Empty);
        }

        [TestMethod]
        public void GetAbonentsBook()
        {
            allPaginationInfo.OrderPropertyName = "Name";
            string errorMessage;

            int count;
            List<UserBookDTO> books = _abonentService.GetBooksOfAbonent(3005, allPaginationInfo, out count, out errorMessage);
            foreach (UserBookDTO book in books)
            {
                System.Diagnostics.Debug.Write(book.BookId + " " + book.BookAbonentId + " "+ book.Name + " " + book.Authors + " " + book.Publisher + " " + book.YearOfPublication + " " + book.Domain + " " + book.DateOfReceiving);
            }

            allPaginationInfo.OrderPropertyName = "FirstName";
            Assert.AreEqual(errorMessage, String.Empty);
        }

        [TestMethod]
        public void RemoveBookFromAbonent()
        {
            string errorMessage = _abonentService.RemoveBook(1, 1);
            Assert.AreEqual(errorMessage, String.Empty);
        }
    }
}
