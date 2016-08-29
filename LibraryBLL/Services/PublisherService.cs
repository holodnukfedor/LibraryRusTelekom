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
    public class PublisherService : IPublisherService
    {
         IUnitOfWork Database { get; set; }

         public PublisherService(IUnitOfWork database)
        {
            Database = database;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public List<PublisherDTO> GetPublishers(PaginationInfo paginationInfo, out int count, out string errorMessage)
        {
            errorMessage = String.Empty;
            count = 0;

            try
            {
                count = Database.Publishers.Count();

                List<Publisher> publishers = Database.Publishers.GetAmount(paginationInfo).ToList();
                List<PublisherDTO> dtos = new List<PublisherDTO>();
                if (publishers != null)
                    publishers.ForEach(p => dtos.Add(new PublisherDTO { Id = p.Id, Info = p.Info, Name = p.Name}));
                return dtos;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }

        public string AddPublisher(PublisherDTO publisher)
        {
            try
            {
                Database.Publishers.Create(new Publisher { Id = publisher.Id, Name = publisher.Name, Info = publisher.Info});
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string DeletePublisher(int id)
        {
            try
            {
                Database.Publishers.Delete(id);
                Database.Save();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }

        public string EditPublisher(PublisherDTO publisher)
        {
            try
            {
                Database.Publishers.Update(new Publisher { Id = publisher.Id, Name = publisher.Name, Info = publisher.Info });
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
