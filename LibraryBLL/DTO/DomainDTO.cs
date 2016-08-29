using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LibraryBLL.DTO
{
    [DataContract]
    public class DomainDTO
    {
         [DataMember]
        public int Id { get; set; }
         [DataMember]
        public string Name { get; set; }
    }
}
