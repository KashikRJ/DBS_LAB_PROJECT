using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace WindowsFormsApplication1
{
    public partial class ViewAuditLogForm : Form
    {
        OracleConnection conn;

        public ViewAuditLogForm()
        {
            InitializeComponent();
            ConnectDB();
            LoadAuditLogData();
            LoadThumbnail();
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
        public void ConnectDB()
        {
            conn = new OracleConnection("DATA SOURCE=192.168.56.1:1521/XE;USER ID=SYSTEM;PASSWORD=student");
            try
            {
                conn.Open();
                MessageBox.Show("Connected");
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error connecting to database: " + e1.Message);
            }
        }

        private void LoadAuditLogData()
        {
            if (conn.State == ConnectionState.Open)
            {
                using (OracleCommand command = new OracleCommand("SELECT * FROM audit_log", conn)) // Replace 'audit_log' with your actual audit log table name
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable; // Assuming your DataGridView is named dataGridView1
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            else
            {
                MessageBox.Show("Database connection is not established.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell click events if necessary
        }

        private void ViewAuditLogForm_Load(object sender, EventArgs e)
        {
            // Optional: Additional code for form load event
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
