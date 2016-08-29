using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using LibraryBLL.DTO;

namespace LibraryWCF.DTO
{
    [DataContract]
    public class DomainListDTO : HasErrorAndRowCountFieldAnswer
    {
        [DataMember]
        public List<DomainDTO> Domains { set; get; }
    }
}
