namespace a3.WinForms
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            RtbMeetingReport = new RichTextBox();
            RtbTranscript = new RichTextBox();
            label1 = new Label();
            BtnProcessTranscript = new Button();
            LbxWellCovered = new ListBox();
            LbxNotCovered = new ListBox();
            label2 = new Label();
            label3 = new Label();
            label5 = new Label();
            BtnClassifyTopics = new Button();
            SuspendLayout();
            // 
            // RtbMeetingReport
            // 
            RtbMeetingReport.AcceptsTab = true;
            RtbMeetingReport.Location = new Point(277, 32);
            RtbMeetingReport.Name = "RtbMeetingReport";
            RtbMeetingReport.Size = new Size(262, 341);
            RtbMeetingReport.TabIndex = 0;
            RtbMeetingReport.Text = "";
            // 
            // RtbTranscript
            // 
            RtbTranscript.Location = new Point(12, 32);
            RtbTranscript.Name = "RtbTranscript";
            RtbTranscript.Size = new Size(259, 341);
            RtbTranscript.TabIndex = 4;
            RtbTranscript.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 14);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 5;
            label1.Text = "Transcript";
            // 
            // BtnProcessTranscript
            // 
            BtnProcessTranscript.Location = new Point(153, 404);
            BtnProcessTranscript.Name = "BtnProcessTranscript";
            BtnProcessTranscript.Size = new Size(118, 23);
            BtnProcessTranscript.TabIndex = 6;
            BtnProcessTranscript.Text = "Process &Transcript";
            BtnProcessTranscript.UseVisualStyleBackColor = true;
            BtnProcessTranscript.Click += BtnProcessTranscript_Click;
            // 
            // LbxWellCovered
            // 
            LbxWellCovered.FormattingEnabled = true;
            LbxWellCovered.ItemHeight = 15;
            LbxWellCovered.Location = new Point(559, 32);
            LbxWellCovered.Name = "LbxWellCovered";
            LbxWellCovered.Size = new Size(221, 334);
            LbxWellCovered.TabIndex = 7;
            LbxWellCovered.Click += LbxWellCovered_Click;
            // 
            // LbxNotCovered
            // 
            LbxNotCovered.FormattingEnabled = true;
            LbxNotCovered.ItemHeight = 15;
            LbxNotCovered.Location = new Point(835, 32);
            LbxNotCovered.Name = "LbxNotCovered";
            LbxNotCovered.Size = new Size(249, 334);
            LbxNotCovered.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(277, 14);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 10;
            label2.Text = "Summary";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(559, 14);
            label3.Name = "label3";
            label3.Size = new Size(88, 15);
            label3.TabIndex = 11;
            label3.Text = "Topics Covered";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(835, 14);
            label5.Name = "label5";
            label5.Size = new Size(111, 15);
            label5.TabIndex = 13;
            label5.Text = "Topics Not Covered";
            // 
            // BtnClassifyTopics
            // 
            BtnClassifyTopics.Location = new Point(432, 404);
            BtnClassifyTopics.Name = "BtnClassifyTopics";
            BtnClassifyTopics.Size = new Size(107, 23);
            BtnClassifyTopics.TabIndex = 14;
            BtnClassifyTopics.Text = "&Classify Topics";
            BtnClassifyTopics.UseVisualStyleBackColor = true;
            BtnClassifyTopics.Click += BtnClassifyTopics_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1331, 875);
            Controls.Add(BtnClassifyTopics);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(LbxNotCovered);
            Controls.Add(LbxWellCovered);
            Controls.Add(BtnProcessTranscript);
            Controls.Add(label1);
            Controls.Add(RtbTranscript);
            Controls.Add(RtbMeetingReport);
            Name = "FrmMain";
            Text = "A3";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox RtbMeetingReport;
        private RichTextBox RtbTranscript;
        private Label label1;
        private Button BtnProcessTranscript;
        private ListBox LbxWellCovered;
        private ListBox LbxSlightlyCovered;
        private ListBox LbxNotCovered;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button BtnClassifyTopics;
    }

}
