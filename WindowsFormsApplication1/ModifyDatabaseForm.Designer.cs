namespace WindowsFormsApplication1
{
    partial class ModifyDatabaseForm
    {
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox comboBoxTableSelect;
        private System.Windows.Forms.Panel panelDynamicControls;
        private System.Windows.Forms.Button buttonSubmit;

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboBoxTableSelect = new System.Windows.Forms.ComboBox();
            this.panelDynamicControls = new System.Windows.Forms.Panel();
            this.buttonSubmit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(406, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(138, 119);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // comboBoxTableSelect
            // 
            this.comboBoxTableSelect.FormattingEnabled = true;
            this.comboBoxTableSelect.Location = new System.Drawing.Point(215, 175);
            this.comboBoxTableSelect.Name = "comboBoxTableSelect";
            this.comboBoxTableSelect.Size = new System.Drawing.Size(121, 24);
            this.comboBoxTableSelect.TabIndex = 1;
            this.comboBoxTableSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxTableSelect_SelectedIndexChanged);
            // 
            // panelDynamicControls
            // 
            this.panelDynamicControls.Location = new System.Drawing.Point(46, 284);
            this.panelDynamicControls.Name = "panelDynamicControls";
            this.panelDynamicControls.Size = new System.Drawing.Size(433, 275);
            this.panelDynamicControls.TabIndex = 2;
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.buttonSubmit.Location = new System.Drawing.Point(215, 230);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(121, 32);
            this.buttonSubmit.TabIndex = 3;
            this.buttonSubmit.Text = "Submit";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // ModifyDatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 600);
            this.Controls.Add(this.buttonSubmit);
            this.Controls.Add(this.panelDynamicControls);
            this.Controls.Add(this.comboBoxTableSelect);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ModifyDatabaseForm";
            this.Text = "Modify Database";
            this.Load += new System.EventHandler(this.ModifyDatabaseForm_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
