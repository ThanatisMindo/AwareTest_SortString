using AwareTest.Model.Model;
using AwareTest.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwareTest.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AwareTestController : ControllerBase
    {
        private readonly ISortStringService _sortStringService;
        public AwareTestController(ISortStringService sortStringService) 
        {
            _sortStringService = sortStringService;
        }

        [HttpPost("SortString")]
        public IActionResult SortString(string str)
        {
            try
            {
                var result = _sortStringService.GetSortString(str);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex}");
            }
        }
    }
}
