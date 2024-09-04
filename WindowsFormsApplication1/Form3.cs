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
    public partial class Form3 : Form
    {
        string userId;
        OracleConnection conn;
        OracleDataAdapter adapter;
        DataTable songsTable;
        private void LoadLikedSongs()
        {
            // Query to select liked songs for the user
            string query = "SELECT s.song_name, s.duration, s.date_created, s.song_likes " +
                           "FROM songs s " +
                           "INNER JOIN favourites f ON s.song_id = f.song_id " +
                           "WHERE f.user_id = :userId";

            adapter = new OracleDataAdapter(query, conn);
            adapter.SelectCommand.Parameters.Add("userId", OracleDbType.Int32).Value = int.Parse(userId);

            songsTable = new DataTable();
            adapter.Fill(songsTable);
            dataGridView1.DataSource = songsTable;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
                // Handle the exception
            }
        }
        public Form3(string id)
        {
            userId = id;
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ConnectDB();
            LoadLikedSongs();
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
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
