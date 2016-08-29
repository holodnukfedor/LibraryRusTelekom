using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDAL.Model;
using System.Runtime.Serialization;

namespace LibraryBLL.DTO
{
     [DataContract]
    public class ReportBookPopularity
    {
         [DataMember]
        public DateTime FirstDate { set; get; }

         [DataMember]
        public DateTime LastDate { set; get; }

         [DataMember]
        public List<BookInfoForReport> BookInfos { set; get; }
        public ReportBookPopularity()
        {
            BookInfos = new List<BookInfoForReport>();
        }
    }
}
