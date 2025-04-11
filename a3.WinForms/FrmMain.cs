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
        Dictionary<string, string> _crew = new Dictionary<string, string>();

        readonly DefaultAzureCredential _credential;
        string? _currentExpert;




        public FrmMain()
        {


            InitializeComponent();
            HireCrew();
            //InitializeQuestions();


            var connectionString = "eastus.api.azureml.ms;e6940c2c-db8f-4bc5-8ba1-7cf9ebf3ba8b;RG_A3;ai-proj-a3";
            _credential = new DefaultAzureCredential();
            _boss = new AgentsClient(connectionString, _credential);


        }

        private void HireCrew()
        {
            var crewDirectoryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crew-directory.txt");
            var txtDirectory = File.ReadAllText(crewDirectoryFilePath);
            if (string.IsNullOrEmpty(txtDirectory))
            {
                MessageBox.Show("No crew directory found");
                return;
            }

            _crew = [];

            //Read each line and split by colon. The first part is the agent specialty, the second part is the agent id. With them, fill the _crew dictionary
            var lines = txtDirectory.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    var specialty = parts[0].Trim();
                    var agentId = parts[1].Trim();
                    _crew.Add(specialty, agentId);
                }
                else
                {
                    MessageBox.Show($"Invalid line in crew directory: {line}");
                }
            }
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
                CallExpert(selectedValue);

            }
        }

        private async void CallExpert(string selectedValue)
        {
            //Get the expert of the selected value
            var expert = _crew.FirstOrDefault(x => x.Key == selectedValue);
            if (expert.Value == null)
            {
                MessageBox.Show($"No expert found for {selectedValue}");
                return;
            }
            //Call the expert
            var expertId = expert.Value;
            RtbExpertOpinion.Text = await Utilities.NewThreadAndResponse(_boss, expertId, _meetingReport);
        }

        private async Task ClassifyContent()
        {

            if (string.IsNullOrEmpty(_meetingReport = string.IsNullOrEmpty(_meetingReport) ? RtbMeetingReport.Text : _meetingReport))
            {
                MessageBox.Show("No report to classify");
                return;
            }


            //Categorizing
            string categorization;
            
            await Task.Delay(1000);
            categorization = await Utilities.NewThreadAndResponse(_boss, _classifierAgentId, _meetingReport);
            


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
            await ClassifyContent();
        }

        private void LbxWellCovered_SelectedValueChanged(object sender, EventArgs e)
        {
            _currentExpert = LbxWellCovered.SelectedItem?.ToString();

        }

        private void LbxWellCovered_DoubleClick(object sender, EventArgs e)
        {

        }

        private void LbxNotCovered_DoubleClick(object sender, EventArgs e)
        {
            //Get the selected value
            var selectedValue = LbxNotCovered.SelectedItem?.ToString();
            //Call the expert
            if (selectedValue != null)
            {
                CallExpert(selectedValue);
            }
        }
    }
}
