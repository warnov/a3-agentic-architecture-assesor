using System.Text.Json.Serialization;

namespace a3.AgentCreatorConsole
{
    internal record AgentDefinition(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("instructions")] string Instructions);
}
