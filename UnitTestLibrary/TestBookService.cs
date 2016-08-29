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
    public class TestBookService
    {
        private IBookService _bookService;

        public TestBookService()
        {
            _bookService = new BookService(new EFUnitOfWork());
        }

        [TestMethod]
        public void GetBooks()
        {
            PaginationInfo paginationInfo = new PaginationInfo
            {
                Amount = 100,
                FromRow = 0,
                OrderPropertyName = "Name",
                SortOrder = SortOrder.ASC 
            };
            string errorMessage;
            int count;

            List<BookDTO> books = _bookService.GetBooks(paginationInfo, out count, out errorMessage);

            Assert.AreEqual(errorMessage, String.Empty);

            foreach (BookDTO book in books)
            {
                 System.Diagnostics.Debug.Write(book.Id + " " + book.Name + " ");
                 foreach (AuthorDTO author in book.Authors)
                 {
                      System.Diagnostics.Debug.Write(author.LastName + " " + author.FirstName.Substring(0, 1) + ". " + author.Patronymic.Substring(0, 1) + ". ");
                 }
                 foreach (DomainDTO domain in book.Domain)
                 {
                     System.Diagnostics.Debug.Write(domain.Name + " ");
                 } 

                 System.Diagnostics.Debug.WriteLine(book.Publisher + " " + book.YearOfPublication + " " + book.Count + " " + book.Annotation + " " + book.AmountOfFree);
            }
        }

        [TestMethod]
        public void CreateBook()
        {
            string errorMessage = _bookService.CreateBook(new BookEditingInfo { Name = "Гарри Поттер и философский камень", Annotation = "хорошая книга", YearOfPublication = 2000, Count = 10}, 3, new List<int>(), new List<int>());
            Assert.AreEqual(errorMessage, String.Empty);
        }

        [TestMethod]
        public void EditBook()
        {
            string errorMessage = _bookService.EditBook(new BookEditingInfo {Id=2, Name = "Гуюрри Поттер", Annotation = "хорошая книга", YearOfPublication = 2000, Count = 10 });
            Assert.AreEqual(errorMessage, String.Empty);
        }

        [TestMethod]
        public void DeleteBook()
        {
            string errorMessage = _bookService.DeleteBook(2);
            Assert.AreEqual(errorMessage, String.Empty);
        }

        [TestMethod]
        public void AddAuthorToBook()
        {
            string errorMessage = _bookService.AddAuthor(3,1);
            Assert.AreEqual(errorMessage, String.Empty);
        }

        [TestMethod]
        public void RemoveAuthorFromBook()
        {
            string errorMessage = _bookService.RemoveAuthor(1, 1);
            Assert.AreEqual(errorMessage, String.Empty);
        }

        [TestMethod]
        public void AddDomainToBook()
        {
            string errorMessage = _bookService.AddDomain(2, 1);
            _bookService.AddDomain(3, 1);
            Assert.AreEqual(errorMessage, String.Empty);
        }

        [TestMethod]
        public void RemoveDomainFromBook()
        {
            string errorMessage = _bookService.RemoveDomain(1, 1);
            Assert.AreEqual(errorMessage, String.Empty);
        }

        [TestMethod]
        public void ChangePublisherOfBook()
        {
            string errorMessage = _bookService.ChangePublisher(3, 1);
            Assert.AreEqual(errorMessage, String.Empty);
        }

        [TestMethod]
        public void GetBookPopularityReport()
        {
            string errorMessage;
            ReportBookPopularity report = _bookService.GetBookPopularityReport(new DateTime(2016, 08, 23), new DateTime(2016, 08, 23), out errorMessage);

            report.BookInfos.ForEach(i => System.Diagnostics.Debug.WriteLine("count = " + i.CountOfTaking + " " + i.Id + " " + i.Name + " " + i.Authors + " " + i.Domain + " " + i.Publisher + " " + i.YearOfPublication.Year + " " + i.Annotation));

            Assert.AreEqual(errorMessage, String.Empty);
        }
    }
}
