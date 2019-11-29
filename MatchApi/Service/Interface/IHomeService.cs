using MatchApi.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Service
{
    public interface IHomeService : IBaseService
    {
        Task<PageList<MemberDto>> GetUserList(MatchParameter para);
        Task<MemberDetailDto> GetUserDetail(int userId);
        Task<IEnumerable<MemberPhotoDto>> GetUserPhotos(int userId);

        //配對條件資料
        Task<MemberConditionDto> GetUserCondition(int userId);
        Task<MemberConditionDto> UpdateUserCondition(MemberConditionDto dtoModel);
        //配對列表
        Task<PageList<MemberDto>> GetMatchList(MatchParameter para);
    }
}
