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
    public class TestAuthorService
    {
        private IAuthorService _authorService;
        private static PaginationInfo allPaginationInfo = new PaginationInfo
        {
            FromRow = 0,
            Amount = int.MaxValue,
            SortOrder = SortOrder.ASC,
            OrderPropertyName = "LastName"
        };

        public TestAuthorService()
        {
            _authorService = new AuthorService(new EFUnitOfWork());
        }

        [TestMethod]
        public void GetAuthors()
        {
            string errorMessage = String.Empty;
            int count;

            List<AuthorDTO> authors = _authorService.GetAuthors(allPaginationInfo, new List<int> { 1 }, out count, out errorMessage);

            Assert.AreEqual(errorMessage, String.Empty);

            foreach (AuthorDTO author in authors)
            {
                System.Diagnostics.Debug.WriteLine(author.Id + " " + author.LastName + " " + author.FirstName + " " + author.Patronymic);
            }
        }

        [TestMethod]
        public void AddAuthor()
        {
            string errorMessage = _authorService.AddAuthor(new AuthorDTO { FirstName = "Джон", LastName = "Толкин", Patronymic = "Рональд Руел" });
            Assert.AreEqual(errorMessage, String.Empty);       
        }

        [TestMethod]
        public void DeleteAuthor()
        {
            string errorMessage = _authorService.DeleteAuthor(2);
            Assert.AreEqual(errorMessage, String.Empty);   
        }

        [TestMethod]
        public void EditAuthor()
        {
            string errorMessage = _authorService.EditAuthor(new AuthorDTO { Id = 3, FirstName = "Джон", LastName = "Толкин", Patronymic = "Рональд Руел" });
            Assert.AreEqual(errorMessage, String.Empty);
        }
    }
}
