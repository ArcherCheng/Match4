using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchApi.Helper;
using MatchApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchApi.Controllers
{
    //[ServiceFilter(typeof(LogUserActivity))]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        [HttpGet("getKeyValue/{appGroup}")]
        public async Task<IActionResult> GetAppKeyValue(string appGroup)
        {
            var service = new BaseService();
            var resultList = await service.GetAppKeyValue(appGroup);
            return Ok(resultList);
        }


        //protected void AddPaginationHeader<T>(PageList<T> pageList)
        //{
        //    Response.AddPagination(pageList.PageNumber, pageList.PageSize, pageList.TotalCount, pageList.TotalPages);
        //}

    }
}