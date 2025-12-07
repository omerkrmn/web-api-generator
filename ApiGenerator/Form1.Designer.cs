namespace ApiGenerator
{
    partial class Form1
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
            btnAddProperty = new Button();
            btnGenerate = new Button();
            pnlProperties = new Panel();
            txtEntityName = new TextBox();
            txtProjectPath = new TextBox();
            btnSelectFolder = new Button();
            SuspendLayout();
            // 
            // btnAddProperty
            // 
            btnAddProperty.Location = new Point(612, 12);
            btnAddProperty.Name = "btnAddProperty";
            btnAddProperty.Size = new Size(75, 23);
            btnAddProperty.TabIndex = 0;
            btnAddProperty.Text = "btnAddProperty";
            btnAddProperty.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(612, 41);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(75, 23);
            btnGenerate.TabIndex = 1;
            btnGenerate.Text = "btnGenerate";
            btnGenerate.UseVisualStyleBackColor = true;
            // 
            // pnlProperties
            // 
            pnlProperties.AutoScroll = true;
            pnlProperties.Location = new Point(12, 70);
            pnlProperties.Name = "pnlProperties";
            pnlProperties.Size = new Size(594, 273);
            pnlProperties.TabIndex = 2;
            // 
            // txtEntityName
            // 
            txtEntityName.Location = new Point(12, 12);
            txtEntityName.Name = "txtEntityName";
            txtEntityName.Size = new Size(100, 23);
            txtEntityName.TabIndex = 3;
            // 
            // txtProjectPath
            // 
            txtProjectPath.Location = new Point(189, 12);
            txtProjectPath.Name = "txtProjectPath";
            txtProjectPath.Size = new Size(100, 23);
            txtProjectPath.TabIndex = 4;
            // 
            // btnSelectFolder
            // 
            btnSelectFolder.Location = new Point(295, 12);
            btnSelectFolder.Name = "btnSelectFolder";
            btnSelectFolder.Size = new Size(118, 23);
            btnSelectFolder.TabIndex = 5;
            btnSelectFolder.Text = "btnSelectFolder";
            btnSelectFolder.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSelectFolder);
            Controls.Add(txtProjectPath);
            Controls.Add(txtEntityName);
            Controls.Add(pnlProperties);
            Controls.Add(btnGenerate);
            Controls.Add(btnAddProperty);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAddProperty;
        private Button btnGenerate;
        private Panel pnlProperties;
        private TextBox txtEntityName;
        private TextBox txtProjectPath;
        private Button btnSelectFolder;
    }
}
