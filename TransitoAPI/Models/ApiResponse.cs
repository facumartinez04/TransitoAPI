using System.Text.Json.Serialization;

public class ApiResponse<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }
}