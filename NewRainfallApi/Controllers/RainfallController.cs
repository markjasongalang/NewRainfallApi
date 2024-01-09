using Microsoft.AspNetCore.Mvc;
using NewRainfallApi.Models;
using System.Text.Json;

namespace NewRainfallApi.Controllers
{
    [Route("api/rainfall")]
    [ApiController]
    public class RainfallController : ControllerBase
    {
        // HttpClient for making HTTP requests
        private readonly HttpClient _httpClient;

        // ErrorResponse property to store error details
        public ErrorResponse ErrorResponse { get; set; } = null!;

        // Constructor to initialize the HttpClient with a base address
        public RainfallController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://environment.data.gov.uk/flood-monitoring");
        }

        [HttpGet("id/{stationId}/readings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRainfallReadings(string stationId, int count = 10)
        {
            // Check if the 'count' parameter is within the valid range
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
                return BadRequest(ErrorResponse); // Return a 400 Bad Request response
            }

            try
            {
                // Construct the HTTP request to get rainfall readings
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_httpClient.BaseAddress}/id/stations/{stationId}/readings?_limit={count}");
                var response = await _httpClient.SendAsync(request);

                // Check if the response is successful
                if (!response.IsSuccessStatusCode)
                {
                    ErrorResponse = new ErrorResponse
                    {
                        Errors = new List<Error>
                        {
                            new Error { Message = "Base address or endpoint is incorrect." }
                        }
                    };
                    return BadRequest(ErrorResponse); // Return a 400 Bad Request response
                }

                // Deserialize the response into a RainfallReadingResponse object
                var rainfallReadingResponse = await JsonSerializer.DeserializeAsync<RainfallReadingResponse>(await response.Content.ReadAsStreamAsync());

                // Check if the response or readings are null
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
                    return NotFound(ErrorResponse); // Return a 404 Not Found response
                }

                return Ok(rainfallReadingResponse.Readings); // Return a 200 OK response
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

                return StatusCode(500, ErrorResponse); // Return a 500 Internal Server Error response
            }
        }
    }
}
