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
            txtProjectName = new TextBox();
            txtProjectPath = new TextBox();
            btnSelectFolder = new Button();
            label1 = new Label();
            label2 = new Label();
            txtEntityName = new TextBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // btnAddProperty
            // 
            btnAddProperty.Location = new Point(219, 77);
            btnAddProperty.Name = "btnAddProperty";
            btnAddProperty.Size = new Size(118, 23);
            btnAddProperty.TabIndex = 0;
            btnAddProperty.Text = "Yeni Property Ekle";
            btnAddProperty.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(12, 349);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(325, 89);
            btnGenerate.TabIndex = 1;
            btnGenerate.Text = "btnGenerate";
            btnGenerate.UseVisualStyleBackColor = true;
            // 
            // pnlProperties
            // 
            pnlProperties.AutoScroll = true;
            pnlProperties.Location = new Point(12, 106);
            pnlProperties.Name = "pnlProperties";
            pnlProperties.Size = new Size(325, 237);
            pnlProperties.TabIndex = 2;
            // 
            // txtProjectName
            // 
            txtProjectName.Location = new Point(78, 12);
            txtProjectName.Name = "txtProjectName";
            txtProjectName.Size = new Size(135, 23);
            txtProjectName.TabIndex = 3;
            // 
            // txtProjectPath
            // 
            txtProjectPath.Location = new Point(78, 41);
            txtProjectPath.Name = "txtProjectPath";
            txtProjectPath.ReadOnly = true;
            txtProjectPath.Size = new Size(135, 23);
            txtProjectPath.TabIndex = 4;
            // 
            // btnSelectFolder
            // 
            btnSelectFolder.Location = new Point(219, 41);
            btnSelectFolder.Name = "btnSelectFolder";
            btnSelectFolder.Size = new Size(118, 23);
            btnSelectFolder.TabIndex = 5;
            btnSelectFolder.Text = "Dosya Yolu Seç";
            btnSelectFolder.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 6;
            label1.Text = "Proje Adı";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 45);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 7;
            label2.Text = "Proje Yolu";
            // 
            // txtEntityName
            // 
            txtEntityName.Location = new Point(78, 77);
            txtEntityName.Name = "txtEntityName";
            txtEntityName.Size = new Size(135, 23);
            txtEntityName.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 80);
            label3.Name = "label3";
            label3.Size = new Size(58, 15);
            label3.TabIndex = 9;
            label3.Text = "Entity Adı";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(346, 450);
            Controls.Add(label3);
            Controls.Add(txtEntityName);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSelectFolder);
            Controls.Add(txtProjectPath);
            Controls.Add(txtProjectName);
            Controls.Add(pnlProperties);
            Controls.Add(btnGenerate);
            Controls.Add(btnAddProperty);
            Name = "Form1";
            Text = "ApiGenerator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAddProperty;
        private Button btnGenerate;
        private Panel pnlProperties;
        private TextBox txtProjectName;
        private TextBox txtProjectPath;
        private Button btnSelectFolder;
        private Label label1;
        private Label label2;
        private TextBox txtEntityName;
        private Label label3;
    }
}
