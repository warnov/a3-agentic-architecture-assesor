using Azure.AI.Projects;
using Azure.Core;
using Azure.Identity;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;


namespace a3.WinForms
{
    public partial class FrmMain : Form
    {

        private AgentsClient _boss;
        readonly string _summarizerAgentId = "asst_k80IAjP2A9TQ5YMGX5xwG3eu";
        readonly string _classificatorAgentId = "asst_6uvZ689eeHu3aUig5yGEs7CL";
        readonly DefaultAzureCredential _credential;
        




        public FrmMain()
        {


            InitializeComponent();

            
            
            var connectionString = "eastus.api.azureml.ms;e6940c2c-db8f-4bc5-8ba1-7cf9ebf3ba8b;RG_A3;ai-proj-a3";
            _credential = new DefaultAzureCredential();
            _boss = new AgentsClient(connectionString, _credential);


        }



        private async void BtnSend_Click(object sender, EventArgs e)
        {
            //await Process();
        }

      


        private async void BtnProcessTranscript_Click(object sender, EventArgs e)
        {
            //If no transcript, return
            var transcript = RtbTranscript.Text;
            if(string.IsNullOrEmpty(transcript))
                return;


            //Create a thread to work with
            AgentThread thread = await _boss.CreateThreadAsync();


            // Add transcript to the thread
            await _boss.CreateMessageAsync(thread.Id, MessageRole.User, transcript);

            // Run the agent
            ThreadRun run = await _boss.CreateRunAsync(thread.Id, _summarizerAgentId);

            // Wait for the run to complete
            do
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                run = await _boss.GetRunAsync(thread.Id, run.Id);
            }
            while (run.Status == RunStatus.Queued
            || run.Status == RunStatus.InProgress);


            // Retrieve messages
            var messages = await _boss.GetMessagesAsync(thread.Id);

            // Get the response message
            var firstMessage = messages.Value.First();
            if (firstMessage == null)
            {
                MessageBox.Show("No messages found.");
                return;
            }
            var content = firstMessage.ContentItems[0];
            if (content is MessageTextContent textContent)
            {
                var report = textContent.Text;
                RtbMeetingReport.AppendText(report);
                ClassifyContent(report);
            }
            else
            {
                MessageBox.Show("No text content found.");
            }
        }

        private async void ClassifyContent(string report)
        {
            //Create a thread to work with
            AgentThread thread = await _boss.CreateThreadAsync();


            // Add meeting report to the thread
            await _boss.CreateMessageAsync(thread.Id, MessageRole.User, report);

            // Run the agent
            ThreadRun run = await _boss.CreateRunAsync(thread.Id, _classificatorAgentId);


            // Wait for the run to complete
            do
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                run = await _boss.GetRunAsync(thread.Id, run.Id);
            }
            while (run.Status == RunStatus.Queued
            || run.Status == RunStatus.InProgress);


            // Retrieve messages
            var messages = await _boss.GetMessagesAsync(thread.Id);

            // Get the response message
            var firstMessage = messages.Value.First();
            if (firstMessage == null)
            {
                MessageBox.Show("No messages found.");
                return;
            }
            var content = firstMessage.ContentItems[0];
            if (content is MessageTextContent textContent)
            {
                var categorization = textContent.Text;
                //Remove first and last line from categorization
                var lines = categorization.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length > 2)
                {
                    categorization = string.Join(Environment.NewLine, lines.Skip(1).SkipLast(1));
                }
                else
                {
                    categorization = string.Empty;
                }
                //Convert categorization json to an array of Workload
                var workloads = JsonSerializer.Deserialize<Workload[]>(categorization);
                //Show the json representing the workloads in the RtbCategorization
                RtbCategorization.AppendText(JsonSerializer.Serialize(workloads, new JsonSerializerOptions { WriteIndented = true }));
                
            }
            else
            {
                MessageBox.Show("No text content found.");
            }
        }
    }
}
