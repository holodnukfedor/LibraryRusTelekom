using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDAL;
using System.Runtime.Serialization;
using LibraryBLL.DTO;

namespace LibraryWCF.DTO
{
    [DataContract]
    public class AbonentDTOList : HasErrorAndRowCountFieldAnswer
    {
        [DataMember]
        public List<AbonentDTO> Abonents { set; get; }
    }
}
