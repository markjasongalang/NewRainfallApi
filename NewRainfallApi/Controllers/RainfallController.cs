using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NewRainfallApi.Controllers
{
    [Route("api/rainfall")]
    [ApiController]
    public class RainfallController : ControllerBase
    {
        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadings(string stationId, int count = 10)
        {

            return Ok();
        }
    }
}
