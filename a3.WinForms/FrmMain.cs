using Azure.AI.Projects;
using Azure.Identity;
using System.Text.Json;



namespace a3.WinForms
{
    public partial class FrmMain : Form
    {

        #region Initialization
        private readonly AgentsClient _boss;
        private string? _meetingReport;
        private static readonly char[] _separator = ['\n'];
        readonly Dictionary<string, string>? _crew;
        readonly Dictionary<string, string> _expertOpinions;


        private readonly DefaultAzureCredential _credential;
        private string? _currentExpert;




        public FrmMain()
        {


            InitializeComponent();
            _crew = Utilities.HireCrew();
            //InitializeQuestions();


            string? connectionString = Environment.GetEnvironmentVariable("a3-aif-cs");
            _credential = new DefaultAzureCredential();
            _boss = new AgentsClient(connectionString, _credential);
            _expertOpinions = [];

        }
        #endregion






        private async void BtnProcessTranscript_Click(object sender, EventArgs e)
        {
            //If no crew, return
            if (_crew == null)
            {
                MessageBox.Show("No crew found");
                return;
            }

            //If no transcript, return
            var transcript = RtbTranscript.Text;
            if (string.IsNullOrEmpty(transcript))
                return;



            _meetingReport = await Utilities.NewThreadAndResponse(_boss, _crew["Summarizer"], transcript);
            RtbMeetingReport.Text = _meetingReport;
            //await ClassifyContent(_meetingReport);

        }


        private async void BtnClassifyTopics_Click(object sender, EventArgs e)
        {
            await ClassifyContent();
        }


        private async void LbxWellCovered_DoubleClick(object sender, EventArgs e)
        {
            RtbExpertOpinion.Clear();
            // Get the selected item
            var selectedValue = LbxWellCovered.SelectedItem?.ToString();
            await GetOpinion(selectedValue);

        }

        private async void LbxNotCovered_DoubleClick(object sender, EventArgs e)
        {

            RtbExpertOpinion.Clear();
            //Get the selected value
            var selectedValue = LbxNotCovered.SelectedItem?.ToString();
            await GetOpinion(selectedValue);
        }






        private async Task GetOpinion(string? selectedValue)
        {
            if (selectedValue != null)
            {
                //Verify if this expert opinion is already in the dictionary
                if (_expertOpinions.TryGetValue(selectedValue, out string? value))
                {
                    FillOpinion(selectedValue, value);                    
                }
                await CallExpert(selectedValue);
            }
        }



        private async Task CallExpert(string selectedValue)
        {
            //if no crew, return
            if (_crew == null)
            {
                MessageBox.Show("No crew found");
                return;
            }

            //Get the expert of the selected value
            var expert = _crew.FirstOrDefault(x => x.Key == selectedValue);
            if (expert.Value == null)
            {
                MessageBox.Show($"No expert found for {selectedValue}");
                return;
            }
            //Call the expert
            var expertId = expert.Value;
            var expertOpinion = await Utilities.NewThreadAndResponse(_boss, expertId, _meetingReport);
            //Add the expert opinion to the dictionary
            if (!_expertOpinions.TryAdd(selectedValue, expertOpinion))
            {
                _expertOpinions[selectedValue] = expertOpinion;
            }
            FillOpinion(selectedValue, expertOpinion);

        }

        private async Task ClassifyContent()
        {
            //If no crew, return
            if (_crew == null)
            {
                MessageBox.Show("No crew found");
                return;
            }

            if (string.IsNullOrEmpty(_meetingReport = string.IsNullOrEmpty(_meetingReport) ? RtbMeetingReport.Text : _meetingReport))
            {
                MessageBox.Show("No report to classify");
                return;
            }


            //Categorizing
            string categorization;


            categorization = await Utilities.NewThreadAndResponse(_boss, _crew["Classifier"], _meetingReport);



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
                MessageBox.Show($"Unable to get JSON: {categorization[..100]}");
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


        private void FillOpinion(string topic, string opinion)
        {
            //Display the expert opinion
            RtbExpertOpinion.Text = $"{topic}:\n========================\n\n{opinion}";
        }
    }
}
