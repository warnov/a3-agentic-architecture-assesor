using Azure.AI.Projects;

namespace a3.WinForms
{
    internal static class Utilities
    {
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
    }
}
