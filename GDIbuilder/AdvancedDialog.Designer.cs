namespace GDIbuilder
{
    partial class AdvancedDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnOK = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            txtVolume = new System.Windows.Forms.TextBox();
            txtSystem = new System.Windows.Forms.TextBox();
            txtVolumeSet = new System.Windows.Forms.TextBox();
            txtPublisher = new System.Windows.Forms.TextBox();
            txtDataPrep = new System.Windows.Forms.TextBox();
            txtApplication = new System.Windows.Forms.TextBox();
            lblVolume = new System.Windows.Forms.Label();
            lblSystem = new System.Windows.Forms.Label();
            lblVolSet = new System.Windows.Forms.Label();
            lblPublisher = new System.Windows.Forms.Label();
            lblDataPrep = new System.Windows.Forms.Label();
            lblApplication = new System.Windows.Forms.Label();
            chkTruncateMode = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(380, 98);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(81, 22);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(467, 98);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(81, 22);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Abbrechen";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtVolume
            // 
            txtVolume.Location = new System.Drawing.Point(103, 7);
            txtVolume.Name = "txtVolume";
            txtVolume.Size = new System.Drawing.Size(161, 23);
            txtVolume.TabIndex = 2;
            // 
            // txtSystem
            // 
            txtSystem.Location = new System.Drawing.Point(103, 31);
            txtSystem.Name = "txtSystem";
            txtSystem.Size = new System.Drawing.Size(161, 23);
            txtSystem.TabIndex = 3;
            // 
            // txtVolumeSet
            // 
            txtVolumeSet.Location = new System.Drawing.Point(103, 55);
            txtVolumeSet.Name = "txtVolumeSet";
            txtVolumeSet.Size = new System.Drawing.Size(161, 23);
            txtVolumeSet.TabIndex = 4;
            // 
            // txtPublisher
            // 
            txtPublisher.Location = new System.Drawing.Point(384, 7);
            txtPublisher.Name = "txtPublisher";
            txtPublisher.Size = new System.Drawing.Size(161, 23);
            txtPublisher.TabIndex = 5;
            // 
            // txtDataPrep
            // 
            txtDataPrep.Location = new System.Drawing.Point(384, 31);
            txtDataPrep.Name = "txtDataPrep";
            txtDataPrep.Size = new System.Drawing.Size(161, 23);
            txtDataPrep.TabIndex = 6;
            // 
            // txtApplication
            // 
            txtApplication.Location = new System.Drawing.Point(384, 55);
            txtApplication.Name = "txtApplication";
            txtApplication.Size = new System.Drawing.Size(161, 23);
            txtApplication.TabIndex = 7;
            // 
            // lblVolume
            // 
            lblVolume.AutoSize = true;
            lblVolume.Location = new System.Drawing.Point(33, 10);
            lblVolume.Name = "lblVolume";
            lblVolume.Size = new System.Drawing.Size(64, 15);
            lblVolume.TabIndex = 8;
            lblVolume.Text = "Volume ID:";
            // 
            // lblSystem
            // 
            lblSystem.AutoSize = true;
            lblSystem.Location = new System.Drawing.Point(35, 34);
            lblSystem.Name = "lblSystem";
            lblSystem.Size = new System.Drawing.Size(62, 15);
            lblSystem.TabIndex = 9;
            lblSystem.Text = "System ID:";
            // 
            // lblVolSet
            // 
            lblVolSet.AutoSize = true;
            lblVolSet.Location = new System.Drawing.Point(14, 58);
            lblVolSet.Name = "lblVolSet";
            lblVolSet.Size = new System.Drawing.Size(83, 15);
            lblVolSet.TabIndex = 10;
            lblVolSet.Text = "Volume Set ID:";
            // 
            // lblPublisher
            // 
            lblPublisher.AutoSize = true;
            lblPublisher.Location = new System.Drawing.Point(287, 10);
            lblPublisher.Name = "lblPublisher";
            lblPublisher.Size = new System.Drawing.Size(91, 15);
            lblPublisher.TabIndex = 11;
            lblPublisher.Text = "Herausgeber ID:";
            // 
            // lblDataPrep
            // 
            lblDataPrep.AutoSize = true;
            lblDataPrep.Location = new System.Drawing.Point(282, 34);
            lblDataPrep.Name = "lblDataPrep";
            lblDataPrep.Size = new System.Drawing.Size(96, 15);
            lblDataPrep.TabIndex = 12;
            lblDataPrep.Text = "Datenersteller ID:";
            // 
            // lblApplication
            // 
            lblApplication.AutoSize = true;
            lblApplication.Location = new System.Drawing.Point(287, 58);
            lblApplication.Name = "lblApplication";
            lblApplication.Size = new System.Drawing.Size(94, 15);
            lblApplication.TabIndex = 13;
            lblApplication.Text = "Anwendungs ID:";
            // 
            // chkTruncateMode
            // 
            chkTruncateMode.AutoSize = true;
            chkTruncateMode.Location = new System.Drawing.Point(103, 79);
            chkTruncateMode.Name = "chkTruncateMode";
            chkTruncateMode.Size = new System.Drawing.Size(230, 19);
            chkTruncateMode.TabIndex = 14;
            chkTruncateMode.Text = "Abgeschnittene Track03.bin generieren";
            chkTruncateMode.UseVisualStyleBackColor = true;
            // 
            // AdvancedDialog
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(561, 142);
            ControlBox = false;
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            Controls.Add(lblApplication);
            Controls.Add(txtApplication);
            Controls.Add(lblDataPrep);
            Controls.Add(txtDataPrep);
            Controls.Add(lblPublisher);
            Controls.Add(lblVolume);
            Controls.Add(lblSystem);
            Controls.Add(lblVolSet);
            Controls.Add(chkTruncateMode);
            Controls.Add(txtVolumeSet);
            Controls.Add(txtSystem);
            Controls.Add(txtVolume);
            Controls.Add(txtPublisher);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Name = "AdvancedDialog";
            Text = "Fortschrittlich";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtVolume;
        private System.Windows.Forms.TextBox txtSystem;
        private System.Windows.Forms.TextBox txtVolumeSet;
        private System.Windows.Forms.TextBox txtPublisher;
        private System.Windows.Forms.TextBox txtDataPrep;
        private System.Windows.Forms.TextBox txtApplication;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.Label lblSystem;
        private System.Windows.Forms.Label lblVolSet;
        private System.Windows.Forms.Label lblPublisher;
        private System.Windows.Forms.Label lblDataPrep;
        private System.Windows.Forms.Label lblApplication;
        private System.Windows.Forms.CheckBox chkTruncateMode;
    }
}