using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL.Sorting
{
    public class SortOrderConverter
    {
        public static SortOrder GetSortOrderFromString(String sortOrder)
        {
            if (sortOrder == "asc") return SortOrder.ASC;
            return SortOrder.DESC;
        }
    }
}
