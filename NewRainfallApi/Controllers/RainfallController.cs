using Microsoft.AspNetCore.Mvc;
using NewRainfallApi.Models;
using System.Text.Json;

namespace NewRainfallApi.Controllers
{
    [Route("api/rainfall")]
    [ApiController]
    public class RainfallController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public RainfallController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://environment.data.gov.uk/flood-monitoring");
        }

        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadings(string stationId, int count = 10)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_httpClient.BaseAddress}/id/stations/{stationId}/readings?_limit={count}");
            
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var rainfallReadingResponse = await JsonSerializer.DeserializeAsync<RainfallReadingResponse>(await response.Content.ReadAsStreamAsync());

            return Ok(rainfallReadingResponse.Items);
        }
    }
}
