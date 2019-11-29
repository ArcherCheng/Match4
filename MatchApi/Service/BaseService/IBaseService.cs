using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Service
{
    public interface IBaseService
    {
        //void AddACustom<T>(T entity) where T : BaseEntity;

        //Task UpdateCustom<T>(T entity) where T : BaseEntity;

        //Task DeleteCustom<T>(T entity) where T : BaseEntity;

        Task<IEnumerable<AppKeyValueDto>> GetAppKeyValue(string appGroup);
    }
}
