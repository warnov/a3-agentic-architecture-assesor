using System.Text.Json.Serialization;

namespace a3.WinForms
{
    public record Topic
    {
        [JsonPropertyName("topicName")]
        public required string TopicName { get; set; }

        [JsonPropertyName("questions")]
        public required string[] Questions { get; set; }
    }
}
