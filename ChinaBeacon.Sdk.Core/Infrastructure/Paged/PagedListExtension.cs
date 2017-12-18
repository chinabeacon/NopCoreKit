using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaBeacon.Sdk.Core.Infrastructure.Paged
{
    public static class PagedListExtension<T> where T : class
    {
        /// <summary>
        /// ToPagedList
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public static TuplePaged<T> ToPagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int totalCount = source.Count();
            List<T> list = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new TuplePaged<T>(list, totalCount);
        }
    }
}
