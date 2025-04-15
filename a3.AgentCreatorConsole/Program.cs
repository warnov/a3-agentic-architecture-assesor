using System.Text.Json;
using a3.AgentCreatorConsole;
using Azure.AI.Projects;
using Azure.Identity;



AgentsClient _boss;

// Read connection string from environment variable
string? connectionString = Environment.GetEnvironmentVariable("a3-aif-cs");
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string not found. Please set the environment variable 'a3-aif-cs'.");
    return;
}

var _credential = new DefaultAzureCredential();
_boss = new AgentsClient(connectionString, _credential);

// Read and deserialize the JSON file
string jsonFilePath = Path.Combine(AppContext.BaseDirectory, "a3-crew-definition.json");
if (!File.Exists(jsonFilePath))
{
    Console.WriteLine($"JSON file not found at {jsonFilePath}");
    return;
}

string jsonContent = await File.ReadAllTextAsync(jsonFilePath);
AgentDefinition[]? agents = JsonSerializer.Deserialize<AgentDefinition[]>(jsonContent);

if (agents == null || agents.Length == 0)
{
    Console.WriteLine("No agents found in the JSON file.");
    return;
}

// Create agents from the JSON definitions
foreach (var agentDefinition in agents)
{
    try
    {
        var agent = await _boss.CreateAgentAsync(agentDefinition.Model, agentDefinition.Name, instructions: agentDefinition.Instructions);
        Console.WriteLine($"Agent {agent.Value.Id} ({agentDefinition.Name}) created successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to create agent {agentDefinition.Name}: {ex.Message}");
    }
}


