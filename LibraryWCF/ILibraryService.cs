using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LibraryWCF.DTO;
using LibraryDAL.Pagination;
using LibraryBLL.DTO;

namespace LibraryWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ILibraryService
    {
         [OperationContract]
        ReportBookPopularityDto GetBookPopularityReport(DateTime firstDate, DateTime lastDate);

        [OperationContract]
        BookDTOList GetBooksList(PaginationInfo paginationInfo);

        [OperationContract]
        UserBookDtoList GetBooksOfAbonent(int id, PaginationInfo paginationInfo);

        [OperationContract]
        string CreateBook(BookEditingInfo book, int publisherId, List<int> domainIds, List<int> authorIds);

        [OperationContract]
        string UpdateBook(BookEditingInfo book);

        [OperationContract]
        string DeleteBook(int id);

        [OperationContract]
        string GiveBookToAbonent(int bookId, int abonentId);

        [OperationContract]
        string ReturnBookFromAbonent(int bookId, int abonentId);


        [OperationContract]
        string AddAuthorToBook(int authorId, int bookId);

        [OperationContract]
        string RemoveAuthorFromBook(int authorId, int bookId);

         [OperationContract]
        string AddDomainToBook(int domainId, int bookId);

         [OperationContract]
         string RemoveDomainFromBook(int domainId, int bookId);

         [OperationContract]
        string ChangePublisher(int publisherId, int bookId);


        [OperationContract]
        AbonentDTOList GetAbonentsList(PaginationInfo paginationInfo);

        [OperationContract]
        string CreateAbonent(AbonentDTO abonent);

        [OperationContract]
        string UpdateAbonent(AbonentDTO abonent);

        [OperationContract]
        string DeleteAbonent(int id);

        [OperationContract]
        DomainListDTO GetDomainsList(PaginationInfo paginationInfo, params int[] domainInBookIds);

        [OperationContract]
        string CreateDomain(DomainDTO dto);

        [OperationContract]
        string UpdateDomain(DomainDTO dto);

        [OperationContract]
        string DeleteDomain(int id);


        [OperationContract]
        PublisherDTOList GetPublishersList(PaginationInfo paginationInfo);

        [OperationContract]
        string CreatePublisher(PublisherDTO dto);

        [OperationContract]
        string UpdatePublisher(PublisherDTO dto);

        [OperationContract]
        string DeletePublisher(int id);


        [OperationContract]
        AuthorDTOList GetAuthorsList(PaginationInfo paginationInfo, params int[] authorsInBookIds);

        [OperationContract]
        string CreateAuthor(AuthorDTO dto);

        [OperationContract]
        string UpdateAuthor(AuthorDTO dto);

        [OperationContract]
        string DeleteAuthor(int id);
    }

}
