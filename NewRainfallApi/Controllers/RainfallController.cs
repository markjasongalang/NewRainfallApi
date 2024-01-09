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

        public ErrorResponse ErrorResponse { get; set; } = null!;

        public RainfallController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://environment.data.gov.uk/flood-monitoring");
        }

        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadings(string stationId, int count = 10)
        {
            if (count < 1 || count > 100)
            {
                ErrorResponse = new ErrorResponse
                {
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Message = "Count must be between 1 and 100.",
                            Detail = new List<ErrorDetail>
                            {
                                new ErrorDetail { PropertyName = "count", Message = "Invalid value." }
                            }
                        }
                    }
                };
                return BadRequest(ErrorResponse);
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_httpClient.BaseAddress}/id/stations/{stationId}/readings?_limit={count}");
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    ErrorResponse = new ErrorResponse
                    {
                        Errors = new List<Error>
                        {
                            new Error { Message = "Base address/endpoint is incorrect." }
                        }
                    };
                    return BadRequest(ErrorResponse);
                }

                var rainfallReadingResponse = await JsonSerializer.DeserializeAsync<RainfallReadingResponse>(await response.Content.ReadAsStreamAsync());
                if (rainfallReadingResponse == null || !rainfallReadingResponse.Readings.Any())
                {
                    ErrorResponse = new ErrorResponse
                    {
                        Errors = new List<Error>
                        {
                            new Error
                            {
                                Message = "No readings found for the specified stationId",
                                Detail = new List<ErrorDetail>
                                {
                                    new ErrorDetail { PropertyName = "stationId", Message = "No stationId found." }
                                }
                            }
                        }
                    };
                    return NotFound(ErrorResponse);
                }

                return Ok(rainfallReadingResponse.Readings);
            }
            catch (Exception)
            {
                ErrorResponse = new ErrorResponse
                {
                    Errors = new List<Error>
                    {
                        new Error { Message = "Internal server error." }
                    }
                };
                return StatusCode(500, ErrorResponse);
            }
        }
    }
}
