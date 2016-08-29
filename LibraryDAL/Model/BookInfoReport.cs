using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LibraryDAL.Model
{
    [DataContract]
    public class BookInfoForReport
    {
         [DataMember]
        public int Id { get; set; }

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
        public DateTime YearOfPublication { get; set; }

         [DataMember]
        public int CountOfTaking { set; get; }
    }
}
