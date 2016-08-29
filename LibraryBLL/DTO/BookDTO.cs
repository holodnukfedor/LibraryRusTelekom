using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDAL;
using System.Runtime.Serialization;

namespace LibraryBLL.DTO
{
    [DataContract]
    public class BookDTO
    {
         [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<DomainDTO> Domain { get; set; }

        [DataMember]
        public List<AuthorDTO> Authors { set; get; }

        [DataMember]
        public string Annotation { get; set; }

        [DataMember]
        public string Publisher { get; set; }

        [DataMember]
        public int YearOfPublication { get; set; }

        [DataMember]
        public int Count { get; set; }

         [DataMember]
        public int AmountOfFree { get; set; }
    }
}
