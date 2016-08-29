using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LibraryBLL.DTO
{
     [DataContract]
    public class UserBookDTO
    {
          [DataMember]
         public int BookAbonentId { set; get; }

          [DataMember]
          public DateTime DateOfReceiving { set; get; } 
          [DataMember]
        public int BookId { get; set; }

          [DataMember]
        public string Name { get; set; }

          [DataMember]
        public string Domain { get; set; }

          [DataMember]
        public string Authors { set; get; }

          [DataMember]
        public string Annotation { get; set; }

          [DataMember]
        public string Publisher { get; set; }

          [DataMember]
        public int YearOfPublication { get; set; }

    }
}
