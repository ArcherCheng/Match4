using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Helper
{
    public class PageList<T> : List<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)System.Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source, int pageCurrent, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PageList<T>(items, count, pageCurrent, pageSize);
        }

        public static PageList<T> Create(IEnumerable<T> source, int pageCurrent, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
            return new PageList<T>(items, count, pageCurrent, pageSize);
        }

        public static PageList<T> Create(List<T> source, int pageCurrent, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
            return new PageList<T>(items, count, pageCurrent, pageSize);
        }

    }
}
