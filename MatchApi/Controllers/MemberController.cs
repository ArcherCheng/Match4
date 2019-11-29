using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MatchApi.Helper;
using MatchApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchApi.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [Route("api/[controller]/{userId}")]
    [ApiController]
    public class MemberController : ApiControllerBase
    {
        private readonly IMemberService _service;

        public MemberController(IMemberService service)
        {
            this._service = service;
        }

        [HttpGet("getMember")]
        public async Task<IActionResult> EditMember(int userId)
        {
            var httpContext = this.HttpContext;
            //if (!Security.CheckCurrentUser(userId,this.HttpContext))
            //    return Unauthorized();

            if (userId != int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var memberEditDto = await _service.GetMemberEdit(userId);
            return Ok(memberEditDto);
        }

        [HttpPost("updateMember")]
        public async Task<IActionResult> UpdateMember(int userId, MemberEditDto model)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var success = await _service.UpdateMember(userId, model);
            if (!success)
                return BadRequest("刪除相片失敗");

            return Ok("刪除相片成功");
        }

        [HttpGet("getPhotos")]
        public async Task<IActionResult> GetPhotos(int userId)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var photoDtos = await _service.GetUserPhotos(userId);
            return Ok(photoDtos);
        }

        [HttpGet("getPhoto/{photoId}")]
        public async Task<IActionResult> GetPhoto(int userId, int photoId)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var photoDto = await _service.GetPhoto(photoId);
            return Ok(photoDto);
        }

        [HttpPost("uploadPhotos")]
        public async Task<IActionResult> UploadPhotos(int userId, [FromForm]MemberPhotoCreateDto model)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var file = model.File;
            if (file == null || file.Length == 0)
                return BadRequest("未選取檔案,無法上傳");

            var success = await _service.UploadPhotos(userId, model);
            if (!success)
                return BadRequest("設定相片封面失敗");

            return Ok();
        }

        [HttpPost("setMainPhoto")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var success = await _service.SetMainPhoto(userId, id);
            if (!success)
                return BadRequest("設定相片封面失敗");

            return Ok("設定相片封面成功");
        }

        [HttpPost("deletePhoto/{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var success = await _service.DeletePhoto(userId, id);
            if (!success)
                return BadRequest("刪除相片失敗");

            return Ok("刪除相片成功");
        }


        [HttpGet("getLiker")]
        public async Task<IActionResult> GetMyLikerList(int userId, [FromQuery]MatchParameter param)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var liker = await _service.GetMyLikerList(userId, param);
            Response.AddPagination(liker.PageNumber, liker.PageSize,
                     liker.TotalCount, liker.TotalPages);

            return Ok(liker);
        }

        [HttpDelete("deleteLiker/{likeId}")]
        public async Task<IActionResult> DeleteMyLiker(int userId, int likeId)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            bool success = await _service.DeleteMyLiker(userId, likeId);
            if (!success)
                return BadRequest("取消好友失敗");

            // return NoContent();
            return Ok("取消我的好友成功");
        }

        [HttpPost("addLiker/{likeId}")]
        public async Task<IActionResult> AddMyLiker(int userId, int likeId)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var success = await _service.AddMyLiker(userId, likeId);
            if (!success)
                return BadRequest("加入好友失敗");

            return Ok("加入好友成功");
        }

        [HttpPost("getMessage/{msgId}")]
        public async Task<IActionResult> GetMessage(int userId, int msgId)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var message = await _service.GetMessage(msgId);
            return Ok(message);
        }

        [HttpGet("getAllMessages")]
        public async Task<IActionResult> GetAllMessages(int userId, [FromQuery]MatchParameter para)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var messages = await _service.GetAllMessages(userId, para);
            return Ok(messages);
        }

        [HttpGet("getUnreadMessages")]
        public async Task<IActionResult> GetUnreadMessages(int userId)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var messages = await _service.GetUnreadMessages(userId);
            return Ok(messages);
        }

        [HttpGet("threadMessage/{recipientId}")]
        public async Task<IActionResult> GetThreadMessage(int userId, int recipientId)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            var messages = await _service.GetThreadMessages(userId, recipientId);
            return Ok(messages);
        }

        [HttpPost("createMessage")]
        public async Task<IActionResult> CreateMessage(int userId, [FromBody]MessageCreateDto createDto)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            await _service.CreateMessage(userId, createDto);

            return Ok();
        }

        [HttpPost("deleteMessage/{msgId}")]
        public async Task<IActionResult> DeleteMessage(int userId, int msgId)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            await _service.DeleteMessage(userId, msgId);

            return Ok();
        }

        [HttpPost("markReadMessage/{msgId}")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int msgId)
        {
            if (!Security.CheckCurrentUser(userId, this.HttpContext))
                return Unauthorized();

            await _service.MarkReadMessage(userId, msgId);

            return Ok();
        }
    }
}