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
            BtnSend = new Button();
            RtbInput = new RichTextBox();
            RtbTranscript = new RichTextBox();
            label1 = new Label();
            BtnProcessTranscript = new Button();
            RtbCategorization = new RichTextBox();
            SuspendLayout();
            // 
            // RtbMeetingReport
            // 
            RtbMeetingReport.AcceptsTab = true;
            RtbMeetingReport.Location = new Point(395, 32);
            RtbMeetingReport.Name = "RtbMeetingReport";
            RtbMeetingReport.Size = new Size(405, 293);
            RtbMeetingReport.TabIndex = 0;
            RtbMeetingReport.Text = "";
            // 
            // BtnSend
            // 
            BtnSend.Location = new Point(713, 350);
            BtnSend.Name = "BtnSend";
            BtnSend.Size = new Size(75, 23);
            BtnSend.TabIndex = 1;
            BtnSend.Text = "&Send";
            BtnSend.UseVisualStyleBackColor = true;
            BtnSend.Click += BtnSend_Click;
            // 
            // RtbInput
            // 
            RtbInput.Location = new Point(19, 408);
            RtbInput.Name = "RtbInput";
            RtbInput.Size = new Size(769, 30);
            RtbInput.TabIndex = 3;
            RtbInput.Text = "";
            // 
            // RtbTranscript
            // 
            RtbTranscript.Location = new Point(12, 32);
            RtbTranscript.Name = "RtbTranscript";
            RtbTranscript.Size = new Size(377, 293);
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
            BtnProcessTranscript.Location = new Point(12, 331);
            BtnProcessTranscript.Name = "BtnProcessTranscript";
            BtnProcessTranscript.Size = new Size(118, 23);
            BtnProcessTranscript.TabIndex = 6;
            BtnProcessTranscript.Text = "Process &Transcript";
            BtnProcessTranscript.UseVisualStyleBackColor = true;
            BtnProcessTranscript.Click += BtnProcessTranscript_Click;
            // 
            // RtbCategorization
            // 
            RtbCategorization.Location = new Point(806, 32);
            RtbCategorization.Name = "RtbCategorization";
            RtbCategorization.Size = new Size(456, 293);
            RtbCategorization.TabIndex = 7;
            RtbCategorization.Text = "";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1331, 450);
            Controls.Add(RtbCategorization);
            Controls.Add(BtnProcessTranscript);
            Controls.Add(label1);
            Controls.Add(RtbTranscript);
            Controls.Add(RtbInput);
            Controls.Add(BtnSend);
            Controls.Add(RtbMeetingReport);
            Name = "FrmMain";
            Text = "A3";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox RtbMeetingReport;
        private Button BtnSend;
        private RichTextBox RtbInput;
        private RichTextBox RtbTranscript;
        private Label label1;
        private Button BtnProcessTranscript;
        private RichTextBox RtbCategorization;
    }

}
