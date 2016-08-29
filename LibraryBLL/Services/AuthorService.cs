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
    public class AuthorService : IAuthorService
    {
        IUnitOfWork Database { get; set; }

        public AuthorService(IUnitOfWork database)
        {
            Database = database;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public List<AuthorDTO> GetAuthors(PaginationInfo paginationInfo, List<int> authorsInBookIds, out int count, out string errorMessage)
        {
            errorMessage = String.Empty;
            count = 0;
            try
            {

                List<Author> autors = null;
                if (authorsInBookIds != null && authorsInBookIds.Count > 0)
                {
                    autors = Database.Authors.FindAmount(a => !authorsInBookIds.Contains(a.Id), paginationInfo).ToList();
                    count = Database.Authors.Count(a => !authorsInBookIds.Contains(a.Id));
                }

                else
                {
                    autors = Database.Authors.GetAmount(paginationInfo).ToList();
                    count = Database.Authors.Count();
                }

                List<AuthorDTO> dtos = new List<AuthorDTO>();
                if (autors != null)
                    autors.ForEach(a => dtos.Add(new AuthorDTO { Id = a.Id, LastName = a.LastName, Patronymic = a.Patronymic, FirstName = a.FirstName}));
                return dtos;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }

        public string AddAuthor(AuthorDTO author)
        {
            try
            {
                Database.Authors.Create(new Author { Id = author.Id, FirstName = author.FirstName, LastName = author.LastName, Patronymic = author.Patronymic });
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string DeleteAuthor(int id)
        {
            try
            {
                Database.Authors.Delete(id);
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string EditAuthor(AuthorDTO author)
        {
            try
            {
                Database.Authors.Update(new Author { Id = author.Id, FirstName = author.FirstName, LastName = author.LastName, Patronymic = author.Patronymic});
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
