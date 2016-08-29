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
    public class AuthorDTOList : HasErrorAndRowCountFieldAnswer
    {
        [DataMember]
        public List<AuthorDTO> Authors { set; get; }
    }
}
