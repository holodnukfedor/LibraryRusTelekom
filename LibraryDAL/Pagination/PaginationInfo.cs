using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDAL.Sorting;

namespace LibraryDAL.Pagination
{
    public struct PaginationInfo
    {
        //public static SortOrder GetSortOrderFromString(String sortOrderString)
        //{
        //    if (sortOrderString.ToLower() == "asc") return PaginationInfo.SortOrder.ASC;
        //    return PaginationInfo.SortOrder.DESC;
        //}
        public Int32 FromRow { set; get; }
        public Int32 Amount { set; get; }
        public String OrderPropertyName { set; get; }
        public SortOrder SortOrder { set; get; }

    }
}
