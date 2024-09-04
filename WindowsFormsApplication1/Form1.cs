using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        OracleConnection conn;
        public void ConnectDB()
        {
            conn = new OracleConnection("DATA SOURCE= 192.168.56.1:1521/XE;USER ID=SYSTEM;PASSWORD=student");
            try
            {
                conn.Open();
                MessageBox.Show("Connected");
            }
            catch (Exception e1)
            {
                // Handle the exception
            }
        }
        string userId = "1";
        public Form1(string id)
        {
            userId = id;
            InitializeComponent();
            ConnectDB();
            LoadGenreData();
            LoadGenreData2();
            InitializeDataGridView();
            InitializeDataGridView2();
            LoadThumbnail();
        }
        
        private void InitializeDataGridView()
        {
            // Create a DataGridViewCheckBoxColumn to display checkboxes
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.HeaderText = "Select Genre";
            checkboxColumn.Name = "CheckboxColumn";
            checkboxColumn.DataPropertyName = "IsSelected";

            // Add the checkbox column to the DataGridView
            dataGridView1.Columns.Add(checkboxColumn);

            // Handle the CellValueChanged event to track changes
            //dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
        }
        private void InitializeDataGridView2()
        {
            // Create a DataGridViewCheckBoxColumn to display checkboxes
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.HeaderText = "Select Artist";
            checkboxColumn.Name = "CheckboxColumn";
            checkboxColumn.DataPropertyName = "IsSelected";

            // Add the checkbox column to the DataGridView
            dataGridView2.Columns.Add(checkboxColumn);

            // Handle the CellValueChanged event to track changes
            //dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
        }
        public void LoadGenreData()
        {
            try
            {
                OracleCommand command = conn.CreateCommand();
                command.CommandText = "SELECT genre_id, genre_name FROM genre";
                command.CommandType = CommandType.Text;

                OracleDataAdapter adapter = new OracleDataAdapter(command);
                DataTable dataTable = new DataTable();

                // Fill the DataTable with the data
                adapter.Fill(dataTable);

                // Bind the DataTable to the DataGridView
                dataGridView1.DataSource = dataTable;

                // Refresh the DataGridView
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., display an error message.
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        public void LoadGenreData2()
        {
            try
            {
                OracleCommand command = conn.CreateCommand();
                command.CommandText = "SELECT artist_id, artist_name FROM artists";
                command.CommandType = CommandType.Text;

                OracleDataAdapter adapter = new OracleDataAdapter(command);
                DataTable dataTable = new DataTable();

                // Fill the DataTable with the data
                adapter.Fill(dataTable);

                // Bind the DataTable to the DataGridView
                dataGridView2.DataSource = dataTable;

                // Refresh the DataGridView
                dataGridView2.Refresh();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., display an error message.
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
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



        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Get the user_id (e.g., "user1")


            // Loop through the DataGridView rows to check which genres are selected
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["CheckboxColumn"].Value);

                if (isSelected)
                {
                    int genreId = Convert.ToInt32(row.Cells["genre_id"].Value);
                    // Insert the user_id and genre_id into the "likes" table
                    InsertLike(userId, genreId);
                }
            }
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["CheckboxColumn"].Value);

                if (isSelected)
                {
                    int artistId = Convert.ToInt32(row.Cells["artist_id"].Value);
                    // Insert the user_id and genre_id into the "likes" table
                    InsertLike2(userId, artistId);
                }
            }

            MessageBox.Show("Likes and listens have been added successfully.");
            Form2 frm = new Form2(userId);
            frm.Show();
            this.Hide();
        }

        private void InsertLike(string userId, int genreId)
        {
            try
            {
                OracleCommand command = conn.CreateCommand();
                command.CommandText = "INSERT INTO likes (user_id, genre_id) VALUES (:userId, :genreId)";
                command.CommandType = CommandType.Text;

                // Add parameters for user_id and genre_id
                command.Parameters.Add(new OracleParameter("userId", userId));
                command.Parameters.Add(new OracleParameter("genreId", genreId));

                // Execute the INSERT query
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., display an error message.
                MessageBox.Show("An error occurred while adding likes: " + ex.Message);
            }
        }
        private void InsertLike2(string userId, int artistId)
        {
            try
            {
                OracleCommand command = conn.CreateCommand();
                command.CommandText = "INSERT INTO listens (user_id, artist_id) VALUES (:userId, :artistId)";
                command.CommandType = CommandType.Text;

                // Add parameters for user_id and genre_id
                command.Parameters.Add(new OracleParameter("userId", userId));
                command.Parameters.Add(new OracleParameter("genreId", artistId));

                // Execute the INSERT query
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., display an error message.
                MessageBox.Show("An error occurred while adding likes: " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
