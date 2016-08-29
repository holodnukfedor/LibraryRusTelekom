using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LibraryWCF.DTO;
using LibraryBLL.Services.Interfaces;
using LibraryBLL.Services;
using LibraryDAL.Repositories.Interfaces;
using LibraryDAL.Repositories;
using LibraryDAL.Pagination;
using LibraryDAL;
using LibraryBLL.DTO;

namespace LibraryWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class LibraryService : ILibraryService
    {
        private IUnitOfWork _unitOfWork;

        public LibraryService ()
	    {
            _unitOfWork = new EFUnitOfWork();
	    }

        public ReportBookPopularityDto GetBookPopularityReport(DateTime firstDate, DateTime lastDate)
        {
            using (IBookService abonentService = new BookService(_unitOfWork))
            {
                ReportBookPopularityDto dto = new ReportBookPopularityDto();
                string errorMessage;
                dto.ReportBookPopularity = abonentService.GetBookPopularityReport(firstDate, lastDate, out errorMessage);
                dto.ErrorMessage = errorMessage;
                dto.AllRowsCount = dto.ReportBookPopularity.BookInfos.Count;
                return dto;
            }
        }

        public UserBookDtoList GetBooksOfAbonent(int id, PaginationInfo paginationInfo)
        {
            using (IAbonentService abonentService = new AbonentService(_unitOfWork))
            {
                string errorMessage = null;
                int count;
                List<UserBookDTO> books = abonentService.GetBooksOfAbonent(id, paginationInfo, out count, out errorMessage);
                UserBookDtoList bookListDTO = new UserBookDtoList
                {
                    ErrorMessage = errorMessage,
                    Books = books,
                    AllRowsCount = count
                };
                return bookListDTO;
            }
        }

        public BookDTOList GetBooksList(PaginationInfo paginationInfo)
        {
            using (IBookService _service = new BookService(_unitOfWork))
            {

                string errorMessage = null;
                int count;
                List<BookDTO> books = _service.GetBooks(paginationInfo, out count, out errorMessage);
                BookDTOList bookListDTO = new BookDTOList
                {
                    ErrorMessage = errorMessage,
                    Books = books,
                    AllRowsCount = count
                };

                return bookListDTO;
            }
        }

        public string GiveBookToAbonent(int bookId, int abonentId)
        {
            using (IAbonentService _service = new AbonentService(_unitOfWork))
            {
                return _service.GiveBook(bookId, abonentId);
            }
        }

        public string ReturnBookFromAbonent(int bookId, int abonentId)
        {
            using (IAbonentService _service = new AbonentService(_unitOfWork))
            {
                return _service.RemoveBook(bookId, abonentId);
            }
        }

        public string CreateBook(BookEditingInfo dto, int publisherId, List<int> domainIds, List<int> authorIds)
        {
            using (IBookService _service = new BookService(_unitOfWork))
            {
                BookEditingInfo book = new BookEditingInfo
                {
                    Name = dto.Name,
                    Annotation = dto.Annotation,
                    Count = dto.Count,
                    YearOfPublication = dto.YearOfPublication
                };

                return _service.CreateBook(book, publisherId, domainIds, authorIds);
            }
        }

        public string UpdateBook(BookEditingInfo dto)
        {
            using (IBookService _service = new BookService(_unitOfWork))
            {
                BookEditingInfo book = new BookEditingInfo
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Annotation = dto.Annotation,
                    Count = dto.Count,
                    YearOfPublication = dto.YearOfPublication
                };

                return _service.EditBook(book);
            }
        }

        public string DeleteBook(int id)
        {
            using (IBookService _service = new BookService(_unitOfWork))
            {
                return _service.DeleteBook(id);
            }
        }

        public string AddAuthorToBook(int authorId, int bookId)
        {
            using (IBookService _service = new BookService(_unitOfWork))
            {
                return _service.AddAuthor(authorId, bookId);
            }
        }

        public string RemoveAuthorFromBook(int authorId, int bookId)
        {
            using (IBookService _service = new BookService(_unitOfWork))
            {
                return _service.RemoveAuthor(authorId, bookId);
            }
        }

        public string AddDomainToBook(int domainId, int bookId)
        {
            using (IBookService _service = new BookService(_unitOfWork))
            {
                return _service.AddDomain(domainId, bookId);
            }
        }

        public string RemoveDomainFromBook(int domainId, int bookId)
        {
            using (IBookService _service = new BookService(_unitOfWork))
            {
                return _service.RemoveDomain(domainId, bookId);
            }
        }

        public string ChangePublisher(int publisherId, int bookId)
        {
            using (IBookService _service = new BookService(_unitOfWork))
            {
                return _service.ChangePublisher(publisherId, bookId);
            }
        }

        public AbonentDTOList GetAbonentsList(PaginationInfo paginationInfo)
        {
            using (IAbonentService _service = new AbonentService(_unitOfWork))
            { 
           
               string errorMessage = null;
               int count;
               List<Abonent> abonents = _service.GetAbonents(paginationInfo, out count, out errorMessage);
               AbonentDTOList abonentListDTO = new AbonentDTOList
               {
                   ErrorMessage = errorMessage,
                   Abonents = new List<AbonentDTO>(),
                   AllRowsCount = count
                   
               };

               abonents.ForEach(a => abonentListDTO.Abonents.Add(new AbonentDTO { Id = a.Id, FirstName = a.FirstName, LastName = a.LastName, Patronymic = a.Patronymic }));
               return abonentListDTO;
            }
        }


        public string CreateAbonent(AbonentDTO dto)
        {
            using (IAbonentService _service = new AbonentService(_unitOfWork))
            {
                Abonent abonent = new Abonent
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Patronymic = dto.Patronymic
                };

                return _service.AddAbonent(abonent);
            }
        }

        public string UpdateAbonent(AbonentDTO dto)
        {
            using (IAbonentService _service = new AbonentService(_unitOfWork))
            {
                Abonent abonent = new Abonent
                {
                    Id = dto.Id,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Patronymic = dto.Patronymic
                };

                return _service.EditAbonent(abonent);
            }
        }

        public string DeleteAbonent(int id)
        {
            using (IAbonentService _service = new AbonentService(_unitOfWork))
            {
                return _service.DeleteAbonent(id);
            }
        }

        public DomainListDTO GetDomainsList(PaginationInfo paginationInfo, params int [] ids)
        {
            using (IDomainService _service = new DomainService(_unitOfWork))
            {

                string errorMessage = null;
                int count;
                List<DomainDTO> domains = _service.GetDomains(paginationInfo, (ids == null ? null : ids.ToList()), out count, out errorMessage);
                DomainListDTO dominsListDTO = new DomainListDTO
                {
                    ErrorMessage = errorMessage,
                    Domains = domains,
                    AllRowsCount = count
                };

                return dominsListDTO;
            }
        }

        public string CreateDomain(DomainDTO dto)
        {
            using (IDomainService _service = new DomainService(_unitOfWork))
            {
                return _service.AddDomain(dto);
            }
        }

        public string UpdateDomain(DomainDTO dto)
        {
            using (IDomainService _service = new DomainService(_unitOfWork))
            {
                return _service.EditDomain(dto);
            }
        }

        public string DeleteDomain(int id)
        {
            using (IDomainService _service = new DomainService(_unitOfWork))
            {
                return _service.DeleteDomain(id);
            }
        }

        public PublisherDTOList GetPublishersList(PaginationInfo paginationInfo)
        {
            using (IPublisherService _service = new PublisherService(_unitOfWork))
            {

                string errorMessage = null;
                int count;
                List<PublisherDTO> publishers = _service.GetPublishers(paginationInfo, out count, out errorMessage);
                PublisherDTOList publishersListDTO = new PublisherDTOList
                {
                    ErrorMessage = errorMessage,
                    Publishers = publishers,
                    AllRowsCount = count
                };

                return publishersListDTO;
            }
        }

        public string CreatePublisher(PublisherDTO dto)
        {
            using (IPublisherService _service = new PublisherService(_unitOfWork))
            {
                return _service.AddPublisher(dto);
            }
        }

        public string UpdatePublisher(PublisherDTO dto)
        {
            using (IPublisherService _service = new PublisherService(_unitOfWork))
            {
                return _service.EditPublisher(dto);
            }
        }

        public string DeletePublisher(int id)
        {
            using (IPublisherService _service = new PublisherService(_unitOfWork))
            {
                return _service.DeletePublisher(id);
            }
        }

        public AuthorDTOList GetAuthorsList(PaginationInfo paginationInfo, params int[] ids)
        {
            using (IAuthorService _service = new AuthorService(_unitOfWork))
            {

                string errorMessage = null;
                int count;
                List<AuthorDTO> authors = _service.GetAuthors(paginationInfo, (ids == null ? null : ids.ToList()), out count, out errorMessage);
                AuthorDTOList AuthorsListDTO = new AuthorDTOList
                {
                    ErrorMessage = errorMessage,
                    Authors = authors,
                    AllRowsCount = count
                };

                return AuthorsListDTO;
            }
        }

        public string CreateAuthor(AuthorDTO dto)
        {
            using (IAuthorService _service = new AuthorService(_unitOfWork))
            {
                return _service.AddAuthor(dto);
            }
        }

        public string UpdateAuthor(AuthorDTO dto)
        {
            using (IAuthorService _service = new AuthorService(_unitOfWork))
            {
                return _service.EditAuthor(dto);
            }
        }

        public string DeleteAuthor(int id)
        {
            using (IAuthorService _service = new AuthorService(_unitOfWork))
            {
                return _service.DeleteAuthor(id);
            }
        }
    }
}
