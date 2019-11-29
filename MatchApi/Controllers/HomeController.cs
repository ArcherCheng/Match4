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
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ApiControllerBase
    {
        private readonly IHomeService _service;

        public HomeController(IHomeService service)
        {
            this._service = service;
        }

        [HttpGet("UserList")]
        public async Task<IActionResult> GetUserList([FromQuery]MatchParameter param)
        {
            var userListDto = await _service.GetUserList(param);

            Response.AddPagination(userListDto.PageNumber, userListDto.PageSize,
                    userListDto.TotalCount, userListDto.TotalPages);

            return Ok(userListDto);
        }

        [HttpGet("userDetail/{userId}")]
        public async Task<IActionResult> GetUserDetail(int userId)
        {
            var memberDto = await _service.GetUserDetail(userId);
            return Ok(memberDto);
        }

        [HttpGet("userPhotos/{userId}")]
        public async Task<IActionResult> GetUserPhotos(int userId)
        {
            var photoListDto = await _service.GetUserPhotos(userId);
            return Ok(photoListDto);
        }

        //[ServiceFilter(typeof(LogUserActivity))]
        [HttpGet("userCondition/{userId}")]
        public async Task<IActionResult> GetUserCondition(int userId)
        {
            var memberConditionDto = await _service.GetUserCondition(userId);
            if (memberConditionDto == null)
            {
                memberConditionDto = new MemberConditionDto();
                memberConditionDto.UserId = userId;
                memberConditionDto.BloodInclude = "";
                memberConditionDto.StarInclude = "";
                memberConditionDto.CityInclude = "";
                memberConditionDto.JobTypeInclude = "";
                memberConditionDto.ReligionInclude = "";
            }
            return Ok(memberConditionDto);
        }

        [HttpPost("userMatchList")]
        public async Task<IActionResult> AnonymousMatchList([FromBody]MemberConditionDto condition, [FromQuery]MatchParameter param)
        {
            param.Condition = condition;
            param.UserId = 0;

            var memberDto = await _service.GetMatchList(param);
            Response.AddPagination(memberDto.PageNumber, memberDto.PageSize, memberDto.TotalCount, memberDto.TotalPages);

            return Ok(memberDto);
        }

        //已經登入者用
        [HttpGet("userMatchList/{userId}")]
        public async Task<IActionResult> GetMyMatchList(int userId, [FromQuery]MatchParameter param)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            param.UserId = userId;
            var memberListDto = await _service.GetMatchList(param);
            Response.AddPagination(memberListDto.PageNumber, memberListDto.PageSize, memberListDto.TotalCount, memberListDto.TotalPages);

            return Ok(memberListDto);
        }

        //已經登入者用
        [HttpPost("userCondition/update/{userId}")]
        public async Task<IActionResult> UpdateUserCondition(int userId, [FromBody]MemberConditionDto model)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            await _service.UpdateUserCondition(model);

            return Ok();

            //return BadRequest("配對條件存檔失敗");
        }

    }
}