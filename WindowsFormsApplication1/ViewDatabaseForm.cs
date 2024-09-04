using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
namespace WindowsFormsApplication1
{
   
    public partial class ViewDatabaseForm : Form
    {
        OracleConnection conn;
        public ViewDatabaseForm()
        {
            InitializeComponent();
            ConnectDB();
            LoadThumbnail();
            PopulateTableNames();
            InitializeControls();
           
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
        
        private void InitializeControls()
        {
            // Set properties for the label
            label1.Text = "Select Table";
            label1.Font = new Font(label1.Font.FontFamily, 12, FontStyle.Bold| FontStyle.Underline); // Increase font size
            label1.AutoSize = true; // Adjust size based on content
            label1.ForeColor = Color.DarkBlue;

            // Set properties for the ComboBox
            comboBox1.Font = new Font(comboBox1.Font.FontFamily, 12, FontStyle.Regular); // Increase font size
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList; // Optional: Set style
        }
        

        private void PopulateTableNames()
        {
            List<string> tableNames = new List<string>
    {
        "users",
        "genre",
        "artists",
        "Albums",
        "songs",
        "Type",
        "works_in",
        "listens",
        "likes",
        "favourites"
    };

            comboBox1.DataSource = tableNames;
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
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTable = comboBox1.SelectedItem.ToString();
            LoadTableData(selectedTable);
        }
        private void LoadTableData(string tableName)
        {
            try
            {
                using (var command = new OracleCommand($"SELECT * FROM {tableName}", conn))
                {
                   
                    using (var adapter = new OracleDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close(); // Ensure the connection is closed
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ViewDatabaseForm_Load(object sender, EventArgs e)
        {

        }
    }
}
