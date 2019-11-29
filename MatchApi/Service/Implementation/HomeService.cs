using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileObjects.AgileMapper;
using MatchApi.Data;
using MatchApi.Helper;
using Microsoft.EntityFrameworkCore;

namespace MatchApi.Service
{
    public class HomeService : BaseService, IHomeService
    {
        public async Task<PageList<MemberDto>> GetUserList(MatchParameter para)
        {
            using (var db = NewDb())
            {
                var list = db.Member
                    .Where(x => !x.IsCloseData)
                    .OrderByDescending(x => x.LoginDate)
                    .Project().To<MemberDto>()
                    .AsQueryable();

                var pageList = await PageList<MemberDto>.CreateAsync(list, para.PageNumber, para.PageSize);
                return pageList;
            }
        }
 

        public async Task<MemberConditionDto> GetUserCondition(int userId)
        {
            using (var db = NewDb())
            {
                var memberCondition = await db.MemberCondition
                    .FirstOrDefaultAsync(x => x.UserId == userId);
                var dto = Mapper.Map(memberCondition).ToANew<MemberConditionDto>();
                return dto;
            }
        }

        public async Task<MemberDetailDto> GetUserDetail(int userId)
        {
            using (var db = NewDb())
            {
                var member = await db.MemberDetail
                    .FirstOrDefaultAsync(x => x.UserId == userId);
                var dto = Mapper.Map(member).ToANew<MemberDetailDto>();
                return dto;
            }
        }

        public async Task<IEnumerable<MemberPhotoDto>> GetUserPhotos(int userId)
        {
            using (var db = NewDb())
            {
                var list = await db.MemberPhoto
                    .Where(x => x.UserId == userId)
                    .ToListAsync();
                var listDto = Mapper.Map(list).ToANew<IEnumerable<MemberPhotoDto>>();
                return listDto;
            }
        }

        public async Task<MemberConditionDto> UpdateUserCondition(MemberConditionDto dtoModel)
        {
            using (var db = NewDb())
            {
                var condition = await db.MemberCondition
                    .FirstOrDefaultAsync(x => x.UserId == dtoModel.UserId);

                if(condition == null)
                {
                    condition = new MemberCondition();
                    condition.UserId = dtoModel.UserId;
                    Mapper.Map(dtoModel).Over<MemberCondition>(condition);
                    db.MemberCondition.Add(condition);
                }
                else
                {
                    Mapper.Map(dtoModel).Over<MemberCondition>(condition);
                    db.MemberCondition.Update(condition);
                }
                await db.SaveChangesAsync();
                return dtoModel;
            }
        }

        public async Task<PageList<MemberDto>> GetMatchList(MatchParameter para)
        {
            using (var db = NewDb())
            {
                var memberCondition = await db.MemberCondition.FirstOrDefaultAsync(x => x.UserId == para.UserId);
                if (memberCondition == null)
                {
                    //memberCondition = new MemberCondition();
                    //Mapper.Map(para.Condition).Over<MemberCondition>(memberCondition);
                    //Mapper.Map(para.Condition).OnTo<MemberCondition>(memberCondition);
                    memberCondition = Mapper.Map(para.Condition).ToANew<MemberCondition>();
                }

                var matchList = db.Member
                    .Where(x => !x.IsCloseData)
                    .OrderByDescending(x => x.LoginDate)
                    .AsQueryable();

                if (memberCondition.MatchSex > 0)
                {
                    matchList = matchList.Where(x => (x.Sex >= memberCondition.MatchSex));
                }

                if (memberCondition.MarryMin > 0 && memberCondition.MarryMax > 0)
                {
                    matchList = matchList.Where(x => (x.Marry >= memberCondition.MarryMin && x.Marry <= memberCondition.MarryMax));
                }

                if (memberCondition.YearMin > 0 && memberCondition.YearMax > 0)
                {
                    matchList = matchList.Where(x => (x.BirthYear >= memberCondition.YearMin && x.BirthYear <= memberCondition.YearMax));
                }

                if (memberCondition.EducationMin > 0 && memberCondition.EducationMax > 0)
                {
                    matchList = matchList.Where(x => (x.Education >= memberCondition.EducationMin && x.Education <= memberCondition.EducationMax));
                }

                if (memberCondition.HeightsMin > 0 && memberCondition.HeightsMax > 0)
                {
                    matchList = matchList.Where(x => (x.Heights >= memberCondition.HeightsMin && x.Heights <= memberCondition.HeightsMax));
                }

                if (memberCondition.WeightsMin > 0 && memberCondition.WeightsMax > 0)
                {
                    matchList = matchList.Where(x => (x.Weights >= memberCondition.WeightsMin && x.Weights <= memberCondition.WeightsMax));
                }

                if (!string.IsNullOrEmpty(memberCondition.BloodInclude))
                {
                    matchList = matchList.Where(x => memberCondition.BloodInclude.Contains(x.Blood));
                }

                if (!string.IsNullOrEmpty(memberCondition.StarInclude))
                {
                    matchList = matchList.Where(x => memberCondition.StarInclude.Contains(x.Star));
                }

                if (!string.IsNullOrEmpty(memberCondition.CityInclude))
                {
                    matchList = matchList.Where(x => memberCondition.CityInclude.Contains(x.City));
                }

                if (!string.IsNullOrEmpty(memberCondition.JobTypeInclude))
                {
                    matchList = matchList.Where(x => memberCondition.JobTypeInclude.Contains(x.JobType));
                }

                if (!string.IsNullOrEmpty(memberCondition.ReligionInclude))
                {
                    matchList = matchList.Where(x => memberCondition.ReligionInclude.Contains(x.Religion));
                }

                var listDto = matchList.Project().To<MemberDto>();

                var pageList = await PageList<MemberDto>.CreateAsync(listDto, para.PageNumber, para.PageSize);

                return pageList;
            }
        }
    }
}
