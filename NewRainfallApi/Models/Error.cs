namespace NewRainfallApi.Models
{
    public class Error
    {
        public string Message { get; set; } = null!;
        public List<ErrorDetail> Detail { get; set; } = null!;
    }
}
