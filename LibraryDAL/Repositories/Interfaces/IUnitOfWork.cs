using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDAL.Model;

namespace LibraryDAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Author> Authors { get; }
        IRepository<Abonent> Abonents { get; }
        IRepository<BookAbonent> BookAbonents { get; }
        IRepository<BookAuthor> BookAuthors { get; }
        IRepository<BookDomain> BookDomains { get; }
        IRepository<Book> Books { get; }
        IRepository<Domain> Domains { get; }
        IRepository<Publisher> Publishers { get; }
        List<BookInfoForReport> GetBookPopularityReport(DateTime firstDate, DateTime lastDate, out string errorMessage);
        void Save();
    }
}
