using System.Text.Json.Serialization;

namespace a3.WinForms
{
    public record Workload
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("covering")]
        public int Covering { get; set; }
        // Explanation is optional; it will be null if not provided in the JSON.
        [JsonPropertyName("explanation")]
        public string? Explanation { get; set; }
    }
}
