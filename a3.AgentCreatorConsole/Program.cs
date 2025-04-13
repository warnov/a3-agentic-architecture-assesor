using Azure.AI.Projects;
using Azure.Identity;


var SMARTEST_MODEL = "gpt-4o";
var FASTEST_MODEL = "gpt-4o-mini";

AgentsClient _boss;
//Read connection string from environment variable
string? connectionString = Environment.GetEnvironmentVariable("a3-aif-cs");
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string not found. Please set the environment variable 'a3-aif-cs'.");
    return;
}

var _credential = new DefaultAzureCredential();
_boss = new AgentsClient(connectionString, _credential);

var agent = await _boss.CreateAgentAsync(FASTEST_MODEL, "A3Summarizer", instructions: "You will be receiving meeting transcripts and your mission is create a formal narrative in English paragraphs of what happened in the meeting, detailing perfectly everything that was discussed without losing any information. You must ensure that all the technical services used at each moment during the conversation are clearly referenced.");

// See https://aka.ms/new-console-template for more information
Console.WriteLine($"Agent {agent.Value.Id} created successfully!");
