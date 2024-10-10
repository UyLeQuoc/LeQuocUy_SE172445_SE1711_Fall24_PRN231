using Newtonsoft.Json;

namespace RazorPagesFE
{
    public class ErrorResponse
    {
        public ErrorDetail Error { get; set; }
    }

    public class ErrorDetail
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class ODataResponse<T>
    {
        [JsonProperty("value")]
        public List<T> Value { get; set; }

        [JsonProperty("@odata.count")]
        public int Count { get; set; }
    }
}
