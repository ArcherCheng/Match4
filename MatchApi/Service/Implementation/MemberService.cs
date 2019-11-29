using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileObjects.AgileMapper;
using MatchApi.Data;
using MatchApi.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace MatchApi.Service
{
    public class MemberService : BaseService, IMemberService
    {
        private readonly IHostingEnvironment _env;

        public MemberService(IHostingEnvironment env)
        {
            this._env = env;
        }

        #region member
        public async Task<MemberEditDto> GetMemberEdit(int userId)
        {
            using (var db = NewDb())
            {
                var result = await db.Member
                    .FirstOrDefaultAsync(x => x.UserId == userId);
                var dto = Mapper.Map(result).ToANew<MemberEditDto>();
                return dto;
            }
        }

        public async Task<bool> UpdateMember(int userId, MemberEditDto editDto)
        {
            using (var db = NewDb())
            {
                var member = await db.Member
                    //.Include(x=>x.MemberDetail)
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                Mapper.Map(editDto).Over(member);
                db.Member.Update(member);

                var detail = await db.MemberDetail
                    .FirstOrDefaultAsync(x => x.UserId == userId);
                if (detail == null)
                {
                    detail = new MemberDetail()
                    {
                        UserId = userId,
                        Introduction = editDto.Introduction,
                        LikeCondition = editDto.LikeCondition
                    };
                    db.MemberDetail.Add(detail);
                }
                else
                {
                    detail.Introduction = editDto.Introduction;
                    detail.LikeCondition = editDto.LikeCondition;
                    db.MemberDetail.Update(detail);
                }
                var sqlCode = await db.SaveChangesAsync();
                return sqlCode > 0;
            }
        }
        #endregion

        #region MemberPhoto
        public async Task<IEnumerable<MemberPhotoDto>> GetUserPhotos(int userId)
        {
            using (var db = NewDb())
            {
                var result = await db.MemberPhoto
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                var dto = Mapper.Map(result).ToANew<IEnumerable<MemberPhotoDto>>();
                return dto;
            }
        }

        public async Task<MemberPhotoDto> GetMainPhoto(int userId)
        {
            using (var db = NewDb())
            {
                var result = await db.MemberPhoto
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.IsMain);
                var dto = Mapper.Map(result).ToANew<MemberPhotoDto>();
                return dto;
            }
        }

        public async Task<MemberPhotoDto> GetPhoto(int id)
        {
            using (var db = NewDb())
            {
                var result = await db.MemberPhoto
                    .FirstOrDefaultAsync(x => x.Id == id);
                var dto = Mapper.Map(result).ToANew<MemberPhotoDto>();
                return dto;
            }
        }

        public async Task<bool> HasMainPhoto(int userId)
        {
            using (var db = NewDb())
            {
                var result = await db.MemberPhoto
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.IsMain);

                return result != null;
            }
        }

        public async Task<bool> UploadPhotos(int userId, MemberPhotoCreateDto modelDto)
        {
            using(var db = NewDb())
            {
                var basePath = System.IO.Path.Combine(this._env.ContentRootPath, "PrivateFiles");
                var usersPath = System.IO.Path.Combine(basePath, "Users");                
                var file = modelDto.File;
                var photo = Mapper.Map(modelDto).ToANew<MemberPhoto>();
                photo.PhotoUrl = usersPath;
                photo.UserId = userId;
                photo.AddedDate = System.DateTime.Now;

                var hasMain = await HasMainPhoto(userId);
                if (!hasMain)
                {
                    photo.IsMain = true;
                    var member = await db.Member.FindAsync(userId);
                    member.MainPhotoUrl = photo.PhotoUrl;
                    db.Member.Update(member);
                }

                db.MemberPhoto.Add(photo);
                var sqlCode = await db.SaveChangesAsync();
                return sqlCode > 0;
            }
        }

        public async Task<bool> SetMainPhoto(int userId, int photoId)
        {
            using (var db = NewDb())
            {
                var results = db.MemberPhoto
                    .Where(x => x.UserId == userId && x.IsMain);
                foreach(var item in results)
                {
                    db.MemberPhoto.Remove(item);
                }

                var mainPhoto = await db.MemberPhoto
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == photoId);
                if (mainPhoto != null)
                {
                    mainPhoto.IsMain = true;
                    db.MemberPhoto.Update(mainPhoto);
                }

                var sqlCode = await db.SaveChangesAsync();
                return sqlCode > 0;
            }
        }

        public async Task<bool> DeletePhoto(int userId, int photoId)
        {
           using(var db = NewDb())
            {
                var result = await db.MemberPhoto
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == photoId);
                if (result == null)
                    return false;
                db.MemberPhoto.Remove(result);
                var sqlCode = await db.SaveChangesAsync();
                return sqlCode > 0;
            }
        }
        #endregion

        #region Liker
        public async Task<PageList<MemberDto>> GetMyLikerList(int userId, MatchParameter para)
        {
            using (var db = base.NewDb())
            {
                var member = db.Liker
                    .Where(x => x.SendId == userId && !x.IsDelete && !x.LikerNavigation.IsCloseData)
                    .Select(x => x.LikerNavigation)
                    .Project().To<MemberDto>()
                    .AsQueryable();

                return await PageList<MemberDto>.CreateAsync(member, para.PageNumber, para.PageSize);
            }
        }

        public async Task<PageList<MemberDto>> GetLikeMeList(int userId, MatchParameter para)
        {
            using (var db = base.NewDb())
            {
                var member = db.Liker
                    .Where(x => x.LikerId == userId && !x.IsDelete && !x.Send.IsCloseData)
                    .Select(x => x.Send)
                    .Project().To<MemberDto>()
                    .AsQueryable();

                return await PageList<MemberDto>.CreateAsync(member, para.PageNumber, para.PageSize);
            }
        }

        public async Task<bool> AddMyLiker(int userId, int likeId)
        {
            using (var db = NewDb())
            {
                var result = await db.Liker
                    .FirstOrDefaultAsync(x => x.SendId == userId && x.LikerId == likeId);

                if (result != null)
                {
                    result.IsDelete = false;
                    db.Liker.Update(result);
                }
                else
                {
                    result = new Liker()
                    {
                        SendId = userId,
                        LikerId = likeId,
                        AddedDate = System.DateTime.Now
                    };
                    db.Liker.Add(result);
                }
                var sqlCode = await db.SaveChangesAsync();
                return sqlCode > 0;
            }
        }

        public async Task<bool> DeleteMyLiker(int userId, int likeId)
        {
            using (var db = NewDb())
            {
                var result = await db.Liker
                    .FirstOrDefaultAsync(x => x.SendId == userId && x.LikerId == likeId);

                if (result == null)
                    return false;

                result.IsDelete = true;
                result.DeleteDate = System.DateTime.Now;

                db.Liker.Update(result);
                var sqlCode = await db.SaveChangesAsync();
                return sqlCode > 0;
            }
        }
        #endregion

        #region Message
        public async Task<MessageDto> GetMessage(int msgId)
        {
            using (var db = NewDb())
            {
                var result = await db.Message
                    .FirstOrDefaultAsync(x => x.Id == msgId);
                var dto = Mapper.Map(result).ToANew<MessageDto>();
                return dto;
            }
        }

        public async Task<IEnumerable<MessageDto>> GetAllMessages(int userId, MatchParameter para)
        {
            using (var db = NewDb())
            {
                var lastDate = System.DateTime.Now.AddMonths(-1);
                var messages = new List<Message>();

                switch (para.MessageContainer)
                {
                    case "Inbox":
                        messages = await db.Message
                           .Where(x => x.SendDate > lastDate && x.RecipientId == userId && x.RecipientDeleted == false)
                           .OrderByDescending(x => x.SendDate)
                           .Include(x => x.Sender)
                           .Include(x => x.Recipient)
                           .ToListAsync();
                        break;
                    case "Outbox":
                        messages = await db.Message
                           .Where(x => x.SendDate > lastDate && x.SenderId == userId && x.SenderDeleted == false)
                           .OrderByDescending(x => x.SendDate)
                           .Include(x => x.Sender)
                           .Include(x => x.Recipient)
                           .ToListAsync();
                        break;
                    default:
                        messages = await db.Message
                            .Where(x => x.SendDate > lastDate && x.RecipientId == userId && x.RecipientDeleted == false)
                            .OrderByDescending(x => x.SendDate)
                            .Include(x => x.Sender)
                            .Include(x => x.Recipient)
                            .ToListAsync();
                        break;
                }

                var dto = Mapper.Map(messages).ToANew<IEnumerable<MessageDto>>();
                return dto;
            }
        }

        public async Task<IEnumerable<MessageDto>> GetThreadMessages(int userId, int recipientId)
        {
            using (var db = NewDb())
            {
                var lastDate = System.DateTime.Now.AddMonths(-1);
                var messages = await db.Message
                    .Include(x => x.Sender)
                    .Include(x => x.Recipient)
                    .Where(p => p.RecipientId == userId
                        && p.SenderId == recipientId
                        && p.RecipientDeleted == false
                        && p.SendDate > lastDate
                        ||
                        p.RecipientId == recipientId
                        && p.SenderId == userId
                        && p.SenderDeleted == false
                        && p.SendDate > lastDate)
                    .OrderByDescending(x => x.SendDate)
                    .ToListAsync();

                var dto = Mapper.Map(messages).ToANew<IEnumerable<MessageDto>>();
                return dto;
            }
        }

        public async Task<IEnumerable<MessageDto>> GetUnreadMessages(int userId)
        {
            using (var db = NewDb())
            {
                var lastDate = System.DateTime.Now.AddMonths(-1);
                var messages = await db.Message
                    .Include(x => x.Sender)
                    .Include(x => x.Recipient)
                    .Where(x => x.RecipientId == userId && x.SendDate > lastDate && x.RecipientDeleted == false && x.IsRead == false)
                    .OrderByDescending(x => x.SendDate)
                    .ToListAsync();

                var dto = Mapper.Map(messages).ToANew<IEnumerable<MessageDto>>();
                return dto;
            }
        }

        public async Task<bool> MarkReadMessage(int userId, int msgId)
        {
            using (var db = NewDb())
            {
                var message = await db.Message
                    .FirstOrDefaultAsync(x => x.Id == msgId && x.SenderId == userId);

                if (message == null)
                    return false;

                message.IsRead = true;
                db.Entry(message).State = EntityState.Modified;
                var sqlCode = await db.SaveChangesAsync();
                return sqlCode > 0;
            }
        }

        public async Task<bool> DeleteMessage(int userId, int msgId)
        {
            using (var db = NewDb())
            {
                var result = await db.Message
                    .FirstOrDefaultAsync(x => x.Id == msgId);
                if (result == null)
                    return false;

                if(result.SenderId == userId)
                {
                    result.SenderDeleted = true;
                }
                else
                {
                    result.RecipientDeleted = true;
                }
                db.Message.Update(result);
                var sqlCode = await db.SaveChangesAsync();
                return sqlCode > 0;
            }
        }

        public async Task<MessageDto> CreateMessage(int userId, MessageCreateDto createDto)
        {
            using (var db = NewDb())
            {
                var result = Mapper.Map(createDto).ToANew<Message>();
                result.SenderId = userId;
                result.SendDate = System.DateTime.Now;

                db.Message.Add(result);
                var sqlCode = await db.SaveChangesAsync();

                var dto = Mapper.Map(result).ToANew<MessageDto>();
                return dto;
            }
        }

        #endregion
    }
}
