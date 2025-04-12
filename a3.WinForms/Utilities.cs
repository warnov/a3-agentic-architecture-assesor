using Azure.AI.Projects;

namespace a3.WinForms
{
    internal static class Utilities
    {
        private static readonly char[] _separator = ['\n'];
        public static async Task<string> NewThreadAndResponse(AgentsClient agentBoss, string agentId, string? requestContent)
        {

            if (string.IsNullOrEmpty(requestContent))
            {
                return string.Empty;
            }

            //Create a thread to work with
            AgentThread thread = await agentBoss.CreateThreadAsync();


            // Add transcript to the thread
            await agentBoss.CreateMessageAsync(thread.Id, MessageRole.User, requestContent);
            var previousMessageCount = await ThreadMessagesCount(agentBoss, thread.Id);


            // Run the agent
            ThreadRun run = await agentBoss.CreateRunAsync(thread.Id, agentId);

            // Wait for the run to complete
            int currentMessageCount;
            do
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                run = await agentBoss.GetRunAsync(thread.Id, run.Id);
                currentMessageCount = await ThreadMessagesCount(agentBoss, thread.Id);
            }
            while (run.Status == RunStatus.Queued
            || run.Status == RunStatus.InProgress || currentMessageCount <= previousMessageCount);


            // Retrieve messages
            var messages = await agentBoss.GetMessagesAsync(thread.Id);

            // Get the response message
            var firstMessage = messages.Value.First();
            if (firstMessage == null)
            {
                return string.Empty;
            }
            var content = firstMessage.ContentItems[0];
            if (content is not MessageTextContent textContent)
            {
                return string.Empty;
            }

            //Clean up resources
            await agentBoss.DeleteThreadAsync(thread.Id);            


            return textContent.Text;

        }

        public static async Task<int> ThreadMessagesCount(AgentsClient agentsBoss, string threadId)
        {
            var messages = await agentsBoss.GetMessagesAsync(threadId);
            return messages.Value.Count();
        }

        public static Dictionary<string,string>? HireCrew()
        {
            var crewDirectoryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crew-directory.txt");
            var txtDirectory = File.ReadAllText(crewDirectoryFilePath);
            if (string.IsNullOrEmpty(txtDirectory))
            {
                return null;
            }

            Dictionary<string,string>crew = [];

            //Read each line and split by colon. The first part is the agent specialty, the second part is the agent id. With them, fill the _crew dictionary
            var lines = txtDirectory.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    var specialty = parts[0].Trim();
                    var agentId = parts[1].Trim();
                    crew.Add(specialty, agentId);
                }
                else
                {
                    MessageBox.Show($"Invalid line in crew directory: {line}");
                }
            }
            return crew;
        }
    }
}
