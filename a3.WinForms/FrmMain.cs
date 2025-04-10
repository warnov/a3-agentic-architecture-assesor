using Azure.AI.Projects;
using Azure.Identity;
using System.Text.Json;



namespace a3.WinForms
{
    public partial class FrmMain : Form
    {

        private AgentsClient _boss;
        private string? _meetingReport;
        private static readonly char[] _separator = ['\n'];
        private static Topic[]? _qDb;
        readonly string _summarizerAgentId = "asst_KfT1Tmy8zaD0mD4BBtZgdWRA";
        readonly string _classifierAgentId = "asst_DKccmkl4sfvQBEEb2KQUyDgO";
        readonly string _questionerAgentId = "asst_rFetlCBHEwUSFn6L9ZSlfYDN";

        readonly DefaultAzureCredential _credential;





        public FrmMain()
        {


            InitializeComponent();
            InitializeQuestions();


            var connectionString = "eastus.api.azureml.ms;e6940c2c-db8f-4bc5-8ba1-7cf9ebf3ba8b;RG_A3;ai-proj-a3";
            _credential = new DefaultAzureCredential();
            _boss = new AgentsClient(connectionString, _credential);


        }

        private static void InitializeQuestions()
        {
            var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "qdb.json");
            var json = File.ReadAllText(jsonFilePath);
            _qDb = JsonSerializer.Deserialize<Topic[]>(json);
        }



        private async void BtnProcessTranscript_Click(object sender, EventArgs e)
        {
            //If no transcript, return
            var transcript = RtbTranscript.Text;
            if (string.IsNullOrEmpty(transcript))
                return;

            _meetingReport = await Utilities.NewThreadAndResponse(_boss, _summarizerAgentId, transcript);
            RtbMeetingReport.Text = _meetingReport;
            //await ClassifyContent(_meetingReport);

        }


        private async void LbxWellCovered_Click(object sender, EventArgs e)
        {
            // Get the selected item
            var selectedValue = LbxWellCovered.SelectedValue?.ToString();
            if (selectedValue != null)
            {
                //Get the questions related to this item, from _qDB



                var unansweredQuestionsBlock = await Utilities.NewThreadAndResponse(_boss, _questionerAgentId, _meetingReport);
                var unansweredQuestions = unansweredQuestionsBlock.Split(_separator);
            }
        }


        private async Task ClassifyContent(string? report)
        {

            if (string.IsNullOrEmpty(report))
            {
                MessageBox.Show("No report to classify");
                return;
            }

            //Categorizing
            string categorization;
            int attempts = 0;
            do
            {
                await Task.Delay(1000);
                categorization = await Utilities.NewThreadAndResponse(_boss, _classifierAgentId, report);
                attempts++;
            }
            while (!categorization.StartsWith("```json") && attempts < 5);
            if (attempts > 5)
            {
                MessageBox.Show("Unable to categorize");
                return;
            }


            //Remove first and last line from categorization
            var lines = categorization.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 2)
            {
                categorization = string.Join(Environment.NewLine, lines.Skip(1).SkipLast(1));
            }
            else
            {
                categorization = string.Empty;
            }
            //Convert categorization json to an array of Workload
            Workload[]? workloads = null;
            try
            {
                workloads = JsonSerializer.Deserialize<Workload[]>(categorization);
            }
            catch
            {
                MessageBox.Show($"Unable to get JSON: {categorization.Substring(0, 100)}");
            }

            if (workloads != null)
            {
                var wellCoveredWorkloads = workloads.Where(w => w.Covering >= 1).ToList();
                LbxWellCovered.Items.Clear();
                foreach (var workload in wellCoveredWorkloads)
                {
                    LbxWellCovered.Items.Add(workload.Name);
                }


                var noCoveredWorkloads = workloads.Where(w => w.Covering < 1).ToList();
                LbxNotCovered.Items.Clear();
                foreach (var workload in noCoveredWorkloads)
                {
                    LbxNotCovered.Items.Add(workload.Name);
                }
            }

        }

        private async void BtnClassifyTopics_Click(object sender, EventArgs e)
        {
            await ClassifyContent(_meetingReport);
        }
    }
}
