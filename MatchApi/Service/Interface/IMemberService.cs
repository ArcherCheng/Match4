using MatchApi.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Service
{
    public interface IMemberService : IBaseService
    {
        //使用者
        Task<MemberEditDto> GetMemberEdit(int userId);
        Task<bool> UpdateMember(int userId, MemberEditDto editDto);

        //photo
        Task<IEnumerable<MemberPhotoDto>> GetUserPhotos(int userId);
        Task<MemberPhotoDto> GetPhoto(int id);
        Task<MemberPhotoDto> GetMainPhoto(int userId);
        Task<bool> HasMainPhoto(int userId);
        Task<bool> UploadPhotos(int userId, MemberPhotoCreateDto modelDto);
        Task<bool> SetMainPhoto(int userId, int photoId);
        Task<bool> DeletePhoto(int userId, int photoId);

        //liker
        Task<bool> AddMyLiker(int userId, int likeId);
        Task<bool> DeleteMyLiker(int userId, int likeId);
        Task<PageList<MemberDto>> GetMyLikerList(int userId, MatchParameter para);
        Task<PageList<MemberDto>> GetLikeMeList(int userId, MatchParameter para);

        //message
        Task<MessageDto> GetMessage(int msgId);
        Task<bool> DeleteMessage(int userId, int msgId);
        Task<MessageDto> CreateMessage(int userId, MessageCreateDto createDto);
        Task<IEnumerable<MessageDto>> GetAllMessages(int userId, MatchParameter para);
        Task<IEnumerable<MessageDto>> GetThreadMessages(int userId, int recipientId);
        Task<IEnumerable<MessageDto>> GetUnreadMessages(int userId);
        Task<bool> MarkReadMessage(int userId, int msgId);
    }
}
