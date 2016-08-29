using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDAL.Repositories.Interfaces;
using LibraryDAL.Model;

namespace LibraryDAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private LibraryRusTelekonEntities _context;
        private bool _disposed = false;
        private AbonentRepository _abonentRepository;
        private AuthorRepository _authorRepository;
        private BookAbonentRepository _bookAbonentRepository;
        private BookAuthorRepository _bookAuthorRepository;
        private BookDomainRepository _bookDomainRepository;
        private BookRepository _bookRepository;
        private DomainRepository _domainRepository;
        private PublisherRepository _publisherRepository;

        public EFUnitOfWork()
        {
            _context = new LibraryRusTelekonEntities();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<Author> Authors
        {
            get
            {
                return _authorRepository ?? (_authorRepository = new AuthorRepository(_context));
            }
        }

        public IRepository<Abonent> Abonents
        {
            get
            {
                return _abonentRepository ?? (_abonentRepository = new AbonentRepository(_context));
            }
        }

        public IRepository<BookAbonent> BookAbonents
        {
            get
            {
                return _bookAbonentRepository ?? (_bookAbonentRepository = new BookAbonentRepository(_context));
            }
        }

        public IRepository<BookAuthor> BookAuthors
        {
            get
            {
                return _bookAuthorRepository ?? (_bookAuthorRepository = new BookAuthorRepository(_context));
            }
        }

        public IRepository<BookDomain> BookDomains
        {
            get
            {
                return _bookDomainRepository ?? (_bookDomainRepository = new BookDomainRepository(_context));
            }
        }

        public IRepository<Book> Books
        {
            get
            {
                return _bookRepository ?? (_bookRepository = new BookRepository(_context));
            }
        }

        public IRepository<Domain> Domains
        {
            get
            {
                return _domainRepository ?? (_domainRepository = new DomainRepository(_context));
            }
        }

        public IRepository<Publisher> Publishers
        {
            get
            {
                return _publisherRepository ?? (_publisherRepository = new PublisherRepository(_context));
            }
        }

        public List<BookInfoForReport> GetBookPopularityReport(DateTime firstDate, DateTime lastDate, out string errorMessage)
        {
            List<BookInfoForReport> bookReportInfos = null;
            errorMessage = String.Empty;

            try
            {
                System.Data.SqlClient.SqlParameter firstDateParametr = new System.Data.SqlClient.SqlParameter("@firstDate", firstDate);
                System.Data.SqlClient.SqlParameter lastDateParametr = new System.Data.SqlClient.SqlParameter("@lastDate", lastDate);
                bookReportInfos = _context.Database.SqlQuery<BookInfoForReport>("BookPopularityReport @firstDate, @lastDate", firstDateParametr, lastDateParametr).ToList();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return bookReportInfos;
        }
    }
}
