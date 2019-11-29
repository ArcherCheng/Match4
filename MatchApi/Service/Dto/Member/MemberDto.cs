using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Service
{
    public class MemberDto
    {
        //不顯示 個人資料法
        public int UserId { get; set; }
        public string NickName { get; set; }
        public int BirthYear { get; set; }

        //以下為個人必要的基本條件
        public int Sex { get; set; }
        public int Marry { get; set; }
        public int Education { get; set; }

        // 以下這些資料會印在聯誼名單的個人資料上
        public int Heights { get; set; }
        public int Weights { get; set; }
        public int Salary { get; set; }
        public string Blood { get; set; }
        public string Star { get; set; }
        public string City { get; set; }
        public string JobType { get; set; }
        public string Religion { get; set; }
        public string MainPhotoUrl { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime ActiveDate { get; set; }
    }

    public class MemberDetailDto
    {
        // 個人特別介紹資料,可在明細查詢,以加深他人印象
        public int UserId { get; set; }
        public string Introduction { get; set; }
        public string LikeCondition { get; set; }
    }

    public class MemberEditDto
    {
        public int UserId { get; set; }
        public string NickName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int BirthYear { get; set; }
        public int Sex { get; set; }
        public int Marry { get; set; }
        public int Education { get; set; }

        // 以下這些資料會印在聯誼名單的個人資料上
        public int Heights { get; set; }
        public int Weights { get; set; }
        public int Salary { get; set; }
        public string Blood { get; set; }
        public string Star { get; set; }
        public string City { get; set; }
        public string School { get; set; }
        public string Subjects { get; set; }
        public string JobType { get; set; }
        public string Religion { get; set; }

        //以下為個人的其他未顯示欄位
        public bool IsCloseData { get; set; }
        public bool IsClosePhoto { get; set; }
        public string LikeCondition { get; set; }
        public string Introduction { get; set; }

        //public string MainPhotoUrl { get; set; }
        //public virtual MemberDetailDto MemberDetail { get; set; }
        //public ICollection<MemberPhotoDto> Photos { get; set; }
    }

    public class MemberConditionDto
    {
        public int UserId { get; set; }
        public int Sex { get; set; }
        public int MatchSex { get; set; }
        public int MarryMin { get; set; }
        public int MarryMax { get; set; }
        public int YearMin { get; set; }
        public int YearMax { get; set; }
        public int EducationMin { get; set; }
        public int EducationMax { get; set; }
        public int HeightsMin { get; set; }
        public int HeightsMax { get; set; }
        public int WeightsMin { get; set; }
        public int WeightsMax { get; set; }
        public int SalaryMin { get; set; }
        public string BloodInclude { get; set; }
        public string StarInclude { get; set; }
        public string CityInclude { get; set; }
        public string JobTypeInclude { get; set; }
        public string ReligionInclude { get; set; }
    }

    public class MemberPhotoDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Descriptions { get; set; }
        public string PhotoUrl { get; set; }
        public string PublicId { get; set; }
        public bool IsMain { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class MemberPhotoCreateDto
    {
        public string PhotoUrl { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile File { get; set; }
        public string Descriptions { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
        //public MemberPhotoCreateDto()
        //{
        //    DateAdded = DateTime.Now;
        //}
    }
}
