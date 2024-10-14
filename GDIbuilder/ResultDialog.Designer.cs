namespace GDIbuilder
{
    partial class ResultDialog
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
            label1 = new System.Windows.Forms.Label();
            txtResult = new System.Windows.Forms.TextBox();
            btnOK = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(19, 15);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(405, 30);
            label1.TabIndex = 0;
            label1.Text = "GD-ROM Erstellung abgeschlossen.\nHier sind die Track Informationen, die du in der GDI Datei verwenden musst:";
            // 
            // txtResult
            // 
            txtResult.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtResult.Location = new System.Drawing.Point(19, 48);
            txtResult.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtResult.Multiline = true;
            txtResult.Name = "txtResult";
            txtResult.ReadOnly = true;
            txtResult.Size = new System.Drawing.Size(466, 132);
            txtResult.TabIndex = 1;
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(398, 188);
            btnOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(88, 27);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(19, 188);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(311, 30);
            label2.TabIndex = 3;
            label2.Text = "Wenn die Datei Disc.gdi im Ausgabeordner vorhanden ist,\nwurde diese automatisch für dich aktualisiert.";
            // 
            // ResultDialog
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(499, 228);
            ControlBox = false;
            Controls.Add(label2);
            Controls.Add(btnOK);
            Controls.Add(txtResult);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ResultDialog";
            Text = "Fertig";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label2;
    }
}