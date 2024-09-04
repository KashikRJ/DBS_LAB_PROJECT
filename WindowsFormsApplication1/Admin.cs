using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
            InitializeAdminLabel();
            LoadThumbnail();
            //this.button4.Click += new System.EventHandler(this.button4_Click);
            //this.button1.Click += new System.EventHandler(this.button1_Click);
           // this.button5.Click += new System.EventHandler(this.button5_Click);
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }
        private void LoadThumbnail()
        {
            try
            {
                string path = "C:\\songs\\AF.png";
                pictureBox1.Image = Image.FromFile(path);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading thumbnail: " + ex.Message);
            }
        }
        private void InitializeAdminLabel()
        {
            label1.Text = "Admin Dashboard";
            label1.Font = new Font("Microsoft Sans Serif", 18, FontStyle.Bold| FontStyle.Underline); // Increased font size
            label1.ForeColor = Color.DarkBlue;
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Dock = DockStyle.None;
            label1.AutoSize = false; // Disable AutoSize to manually adjust the size
            label1.Height = 60; // Adjust the height as needed
            label1.Width = this.Width; // Make the label as wide as the form

            // Position the label at the top center of the form
            label1.Location = new Point((this.Width - label1.Width) / 2, 10); // 10 for a little top margin

        }
        private void button4_Click(object sender, EventArgs e)
        {
            ViewDatabaseForm viewDatabaseForm = new ViewDatabaseForm();
            viewDatabaseForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ModifyDatabaseForm modifyDatabaseForm = new ModifyDatabaseForm();
            modifyDatabaseForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ViewAuditLogForm viewAuditLogForm = new ViewAuditLogForm();
            viewAuditLogForm.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
