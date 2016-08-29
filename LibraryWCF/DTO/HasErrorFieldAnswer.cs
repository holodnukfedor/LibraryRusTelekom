using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace LibraryWCF.DTO
{
    [DataContract]
    public abstract class HasErrorAndRowCountFieldAnswer
    {
        [DataMember]
        public string ErrorMessage {get;set;}

         [DataMember]
        public int AllRowsCount { set; get; }
    }
}
