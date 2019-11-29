using AgileObjects.AgileMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Helper
{
    public static class MapperExtensions
    {
        public static TSource Clone<TSource>(this TSource source)
        {
            return Mapper.DeepClone(source);
        }

        public static TDestination Map<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map(source).ToANew<TDestination>();
        }

        public static TDestination Map<TDestination>(this object source)
        {
            return Mapper.Map(source).ToANew<TDestination>();
        }

        public static TDestination Map<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source).OnTo(destination);
        }

        public static IQueryable<TDestination> Project<TSource, TDestination>(this IQueryable<TSource> queryable)
        {
            return queryable.Project().To<TDestination>();
        }
    }

}
