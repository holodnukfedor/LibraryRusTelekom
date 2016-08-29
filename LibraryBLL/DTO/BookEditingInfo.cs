using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LibraryBLL.DTO
{
     [DataContract]
    public class BookEditingInfo
    {
          [DataMember]
        public int Id { get; set; }

          [DataMember]
        public string Name { get; set; }

          [DataMember]
        public string Annotation { get; set; }

          [DataMember]
        public int YearOfPublication { get; set; }

          [DataMember]
        public int Count { get; set; }
    }
}
