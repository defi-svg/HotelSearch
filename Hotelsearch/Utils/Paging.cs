using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotelsearch.Utils
{
    public static class Paging
    {
        /// <summary>
        /// Function used to set up the paging algorithm on a list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>filtered list of hotels</returns>
        public static List<T> PageFilter<T>(List<T> list, int pageNumber, int pageSize = 2)
        {
            if (list is null)
            {
                return null;
            }

            if(pageNumber==0 && pageSize==0)
            {
                return list;
            }

            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
